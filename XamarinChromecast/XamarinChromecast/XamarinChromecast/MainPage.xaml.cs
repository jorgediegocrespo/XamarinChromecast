using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace XamarinChromecast
{
    public partial class MainPage : ContentPage
    {
        public List<string> ChromecastDevices { get; set; }

        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            FrChromecastDevices.TranslationY = DeviceDisplayInfo.ScreenHeight + 5;
        }

        private void BtCast_Clicked(object sender, EventArgs e)
        {
            ChromecastDevices = new List<string> { "Device1", "Device2" };
            CvChromecastDevices.ItemsSource = ChromecastDevices;

            FrChromecastDevices.IsVisible = true;
            FrChromecastDevices.TranslateTo(
                0, 
                DeviceDisplayInfo.ScreenHeight - (ChromecastDevices.Count * 50) - On<iOS>().SafeAreaInsets().Bottom - FrChromecastDevices.Padding.VerticalThickness - 50, 
                250);
        }
    }
}
