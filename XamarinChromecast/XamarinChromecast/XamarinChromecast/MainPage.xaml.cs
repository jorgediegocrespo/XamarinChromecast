using GoogleCast;
using GoogleCast.Channels;
using GoogleCast.Models.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace XamarinChromecast
{
    public partial class MainPage : ContentPage
    {
        private readonly DeviceLocator deviceLocator;
        private List<IReceiver> chromecastDevices;

        public MainPage()
        {
            deviceLocator = new DeviceLocator();
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            FrChromecastDevices.TranslationY = DeviceDisplayInfo.ScreenHeight + 5;
        }

        private async void BtCast_Clicked(object sender, EventArgs e)
        {
            await LoadReceivers();

            FrChromecastDevices.IsVisible = true;
            await FrChromecastDevices.TranslateTo(
                0, 
                DeviceDisplayInfo.ScreenHeight - (chromecastDevices.Count * 50) - On<iOS>().SafeAreaInsets().Bottom - FrChromecastDevices.Padding.VerticalThickness - 100, 
                250);
        }

        private async Task LoadReceivers()
        {
            chromecastDevices = null;
            chromecastDevices = (await new DeviceLocator().FindReceiversAsync()).ToList();
            CvChromecastDevices.ItemsSource = chromecastDevices;
        }

        private async void CvChromecastDevices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await PlayVideo();
        }

        private async Task PlayVideo()
        {
            IReceiver selectedCastDevice = CvChromecastDevices.SelectedItem as IReceiver;
            if (selectedCastDevice == null)
                return;

            Sender ccSender = new Sender();
            await ccSender.ConnectAsync(selectedCastDevice);

            IMediaChannel mediaChannel = ccSender.GetChannel<IMediaChannel>();
            await ccSender.LaunchAsync(mediaChannel);

            MediaStatus mediaStatus = await mediaChannel.LoadAsync(new MediaInformation() { ContentId = EnUrl.Text });

            CvChromecastDevices.SelectedItem = null;
            await CloseReceiversList();
        }

        private async void BtClose_Clicked(object sender, EventArgs e)
        {
            await CloseReceiversList();
        }

        private async Task CloseReceiversList()
        {
            await FrChromecastDevices.TranslateTo(0, DeviceDisplayInfo.ScreenHeight + 5, 250);
            FrChromecastDevices.IsVisible = false;
        }
    }
}
