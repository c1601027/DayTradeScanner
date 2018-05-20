﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DayTradeScanner;
using System.Collections.ObjectModel;

namespace DayTrader
{
	public class SettingsDialog : Window
	{
		private DropDown _dropDown;
		public ObservableCollection<string> Exchanges { get; set; }

		public SettingsDialog()
		{
			DataContext = this;
			Exchanges = new ObservableCollection<string>();
			Exchanges.Add("Bitfinex");
			Exchanges.Add("Bittrex");
			Exchanges.Add("Binance");
			Exchanges.Add("GDax");
			Exchanges.Add("HitBTC");
			Exchanges.Add("Kraken");

			InitializeComponent();
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoaderPortableXaml.Load(this);
			this.AttachDevTools();
			_dropDown = this.Find<DropDown>("dropExchange");

			var btnSave = this.Find<Button>("btnSave");
			btnSave.Click += BtnSave_Click;

			var settings = SettingsStore.Load();
			CurrencyUSD = settings.USD;
			CurrencyETH = settings.ETH;
			CurrencyEUR = settings.EUR;
			CurrencyBNB = settings.BNB;
			CurrencyBTC = settings.BTC;
			AllowShorts = settings.AllowShorts;
			MaxPanic = settings.MaxPanic.ToString();
			MaxFlatCandles = settings.MaxFlatCandles.ToString();
			MaxFlatCandleCount = settings.MaxFlatCandleCount.ToString();
			BollingerBandWidth = settings.MinBollingerBandWidth.ToString();

			Volume = settings.Min24HrVolume.ToString();
			_dropDown.SelectedIndex = Exchanges.IndexOf(settings.Exchange);
		}

		private void BtnSave_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			var settings = SettingsStore.Load();
			settings.USD = CurrencyUSD;
			settings.ETH = CurrencyETH;
			settings.EUR = CurrencyEUR;
			settings.BNB = CurrencyBNB;
			settings.BTC = CurrencyBTC;
			settings.AllowShorts = AllowShorts;

			long l;
			if (long.TryParse(Volume, out l))
			{
				settings.Min24HrVolume = l;
			}

			int i;
			if (int.TryParse(MaxFlatCandles, out i))
			{
				settings.MaxFlatCandles = i;
			}
			if (int.TryParse(MaxFlatCandleCount, out i))
			{
				settings.MaxFlatCandleCount = i;
			}



			double d;
			if (double.TryParse(MaxPanic, out d))
			{
				settings.MaxPanic = d;
			}
			if (double.TryParse(BollingerBandWidth, out d))
			{
				settings.MinBollingerBandWidth = d;
			}
			settings.Exchange = Exchanges[_dropDown.SelectedIndex];
			SettingsStore.Save(settings);
			this.Close();
		}

		public static readonly AvaloniaProperty<bool> CurrencyUSDProperty = AvaloniaProperty.Register<SettingsDialog, bool>("CurrencyUSD", inherits: true);

		public bool CurrencyUSD
		{
			get { return this.GetValue(CurrencyUSDProperty); }
			set { this.SetValue(CurrencyUSDProperty, value); }
		}

		public static readonly AvaloniaProperty<bool> CurrencyEURProperty = AvaloniaProperty.Register<SettingsDialog, bool>("CurrencyEUR", inherits: true);

		public bool CurrencyEUR
		{
			get { return this.GetValue(CurrencyEURProperty); }
			set { this.SetValue(CurrencyEURProperty, value); }
		}

		public static readonly AvaloniaProperty<bool> CurrencyETHProperty = AvaloniaProperty.Register<SettingsDialog, bool>("CurrencyETH", inherits: true);

		public bool CurrencyETH
		{
			get { return this.GetValue(CurrencyETHProperty); }
			set { this.SetValue(CurrencyETHProperty, value); }
		}

		public static readonly AvaloniaProperty<bool> CurrencyBNBProperty = AvaloniaProperty.Register<SettingsDialog, bool>("CurrencyBNB", inherits: true);

		public bool CurrencyBNB
		{
			get { return this.GetValue(CurrencyBNBProperty); }
			set { this.SetValue(CurrencyBNBProperty, value); }
		}

		public static readonly AvaloniaProperty<bool> CurrencyBTCProperty = AvaloniaProperty.Register<SettingsDialog, bool>("CurrencyBTC", inherits: true);

		public bool CurrencyBTC
		{
			get { return this.GetValue(CurrencyBTCProperty); }
			set { this.SetValue(CurrencyBTCProperty, value); }
		}

		public static readonly AvaloniaProperty<bool> AllowShortsProperty = AvaloniaProperty.Register<SettingsDialog, bool>("AllowShorts", inherits: true);

		public bool AllowShorts
		{
			get { return this.GetValue(AllowShortsProperty); }
			set { this.SetValue(AllowShortsProperty, value); }
		}

		public static readonly AvaloniaProperty<string> VolumeProperty = AvaloniaProperty.Register<SettingsDialog, string>("Volume", inherits: true);

		public string Volume
		{
			get { return this.GetValue(VolumeProperty); }
			set { this.SetValue(VolumeProperty, value); }
		}


		public static readonly AvaloniaProperty<string> BollingerBandWidthProperty = AvaloniaProperty.Register<SettingsDialog, string>("BollingerBandWidth", inherits: true);

		public string BollingerBandWidth
		{
			get { return this.GetValue(BollingerBandWidthProperty); }
			set { this.SetValue(BollingerBandWidthProperty, value); }
		}

		public static readonly AvaloniaProperty<string> MaxFlatCandlesProperty = AvaloniaProperty.Register<SettingsDialog, string>("MaxFlatCandles", inherits: true);

		public string MaxFlatCandles
		{
			get { return this.GetValue(MaxFlatCandlesProperty); }
			set { this.SetValue(MaxFlatCandlesProperty, value); }
		}


		public static readonly AvaloniaProperty<string> MaxFlatCandleCountProperty = AvaloniaProperty.Register<SettingsDialog, string>("MaxFlatCandleCount", inherits: true);

		public string MaxFlatCandleCount
		{
			get { return this.GetValue(MaxFlatCandleCountProperty); }
			set { this.SetValue(MaxFlatCandleCountProperty, value); }
		}

		public static readonly AvaloniaProperty<string> MaxPanicProperty = AvaloniaProperty.Register<SettingsDialog, string>("MaxPanic", inherits: true);

		public string MaxPanic
		{
			get { return this.GetValue(MaxPanicProperty); }
			set { this.SetValue(MaxPanicProperty, value); }
		}
	}
}