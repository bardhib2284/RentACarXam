using Plugin.Media;
using Plugin.Media.Abstractions;
using RentACar.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RentACar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateACarPage : ContentPage
    {
        public CreateACarPage()
        {
            InitializeComponent();
            numberOfSeats.ItemsSource = new List<int> { 1, 2, 4, 5, 6 };
            fuelPicker.ItemsSource = new List<string> { "Diezel", "Benzin", "Rryme Elektrike" };
            transmissionPicker.ItemsSource = new List<string> { "Automatic", "Manual" };
            transmissionPicker.SelectedItem = "Automatic";
            fuelPicker.SelectedItem = "Diezel";
            numberOfSeats.SelectedItem = 4;
            servisPicker.ItemsSource = new List<string> { "Kilometrazh", "Kohor" };
            servisPicker.SelectedIndexChanged += ServisPicker_SelectedIndexChanged;
            kohorePicker.ItemsSource = new List<string> { "6 Mujor","9 Mujor", "Vjetor" };
            kohorePicker.IsVisible = false;
            servisEntry.IsVisible = false;
        }

        private void ServisPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(((Picker)sender).SelectedItem.ToString() == "Kilometrazh")
            {
                kohorePicker.IsVisible = false;
                servisEntry.IsVisible = true;
            }
            else if (((Picker)sender).SelectedItem.ToString() == "Vjetor")
            {
                kohorePicker.IsVisible = true;
                servisEntry.IsVisible = false;
            }
        }

        private async void Button_ClickedAsync(object sender, EventArgs e)
        {
            var cameraStatus = await Permissions.CheckStatusAsync<Permissions.Camera>();

            var storageStatus = await Permissions.CheckStatusAsync<Permissions.StorageRead>();

            if (cameraStatus != Xamarin.Essentials.PermissionStatus.Granted || storageStatus != Xamarin.Essentials.PermissionStatus.Granted)
            {
                var cameraresults = await Permissions.RequestAsync<Permissions.Camera>();
                var storageResults = await Permissions.RequestAsync<Permissions.StorageRead>();
            }
            cameraStatus = await Permissions.CheckStatusAsync<Permissions.Camera>();

            storageStatus = await Permissions.CheckStatusAsync<Permissions.StorageRead>();

            if (cameraStatus == Xamarin.Essentials.PermissionStatus.Granted && storageStatus == Xamarin.Essentials.PermissionStatus.Granted)
            {
                var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                {
                    MaxWidthHeight = 512,
                    CompressionQuality = 75,
                    PhotoSize = PhotoSize.MaxWidthHeight,
                });
                if (file == null)
                {
                    return;
                }
                using (MemoryStream ms = new MemoryStream())
                {
                    file.GetStream().CopyTo(ms);
                    (BindingContext as DashboardViewModel).SelectedCar.CarImage_1 = ms.ToArray();
                }
            }
        }

        private async void Button_Clicked_1Async(object sender, EventArgs e)
        {
            var cameraStatus = await Permissions.CheckStatusAsync<Permissions.Camera>();

            var storageStatus = await Permissions.CheckStatusAsync<Permissions.StorageRead>();

            if (cameraStatus != Xamarin.Essentials.PermissionStatus.Granted || storageStatus != Xamarin.Essentials.PermissionStatus.Granted)
            {
                var cameraresults = await Permissions.RequestAsync<Permissions.Camera>();
                var storageResults = await Permissions.RequestAsync<Permissions.StorageRead>();
            }
            cameraStatus = await Permissions.CheckStatusAsync<Permissions.Camera>();

            storageStatus = await Permissions.CheckStatusAsync<Permissions.StorageRead>();

            if (cameraStatus == Xamarin.Essentials.PermissionStatus.Granted && storageStatus == Xamarin.Essentials.PermissionStatus.Granted)
            {
                var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                {
                    MaxWidthHeight = 512,
                    CompressionQuality = 75,
                    PhotoSize = PhotoSize.MaxWidthHeight,
                });
                if (file == null)
                {
                    return;
                }
                using (MemoryStream ms = new MemoryStream())
                {
                    file.GetStream().CopyTo(ms);
                    (BindingContext as DashboardViewModel).SelectedCar.CarImage_2 = ms.ToArray();
                }
            }
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(sender is Picker g)
            {
                var bc = (DashboardViewModel)BindingContext;
                if (g.SelectedIndex == 0)
                {
                    bc.SelectedCar.ImageChoosen = true;
                }
                else
                    bc.SelectedCar.ImageChoosen = false;
            }
        }
    }
}