﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeSharp;

namespace DayTradeScanner
{
	class Program
	{
		static void Main(string[] args)
		{
			var bgColor = Console.BackgroundColor;
			var fgColor = Console.ForegroundColor;
			Console.WriteLine("Daytrader scanner 1.0 for Bitfinex");
			Console.WriteLine("Construct list of USD symbols with enough volume");
			Task.Run(async () =>
		   {
			   var api = new ExchangeBitfinexAPI();
			   var allSymbols = await api.GetSymbolsAsync();


			   var symbols = new List<string>();
			   foreach (var symbol in allSymbols)
			   {
				   if (!symbol.Contains("USD"))
				   {
					   // only scan USD pairs
					   continue;
				   }
				   var ticker = await api.GetTickerAsync(symbol);
				   if (ticker.Volume.ConvertedVolume < 500000)
				   {
					   // skip coins with a 24hr trading volume < $1,000,000 dollar
					   continue;
				   }
				   symbols.Add(symbol);
				   Console.WriteLine($"Adding {symbol} with 24hr volume of $ {ticker.Volume.ConvertedVolume} to scan list");
			   }

			   Console.WriteLine($"List constructed.. Now starting scanning of 5 min charts for {symbols.Count} symbols...");
			   var date = DateTime.Now;
			   while (true)
			   {
				   Console.WriteLine($"scanning...");
				   date = DateTime.Now;
				   foreach (var symbol in symbols)
				   {
					   var candles = (await api.GetCandlesAsync(symbol, 60 * 5, DateTime.Now.AddMinutes(-5 * 100))).Reverse().ToList();

					   var bbands = new BollingerBands(candles);
					   var stoch = new Stochastics(candles);

					   if (bbands.Bandwidth >= 2m)
					   {
						   if (stoch.K < 20 && stoch.D < 20)
						   {
							   if (candles[0].ClosePrice < bbands.Lower)
							   {
								   Console.Beep();
								   Console.BackgroundColor = ConsoleColor.Red;
								   Console.ForegroundColor = ConsoleColor.White;
								   Console.WriteLine($"{symbol} long signal found");
								   Console.BackgroundColor = bgColor;
								   Console.ForegroundColor = fgColor;
							   }
						   }
						   else if (stoch.K > 80 && stoch.D > 80)
						   {
							   if (candles[0].ClosePrice > bbands.Upper)
							   {
								   Console.Beep();
								   Console.BackgroundColor = ConsoleColor.Red;
								   Console.ForegroundColor = ConsoleColor.White;
								   Console.WriteLine($"{symbol} short signal found");
								   Console.BackgroundColor = bgColor;
								   Console.ForegroundColor = fgColor;
							   }
						   }
					   }
				   }

				   Console.WriteLine("waiting...");
				   while (true)
				   {
					   var ts = DateTime.Now - date;
					   if (ts.TotalMinutes >= 1) break;
					   await Task.Delay(5000);
				   }
			   }
		   }).Wait();
		}
	}
}
