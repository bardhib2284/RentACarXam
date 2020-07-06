﻿using Plugin.Media;
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
    public partial class CreateClientPage : ContentPage
    {
        public CreateClientPage()
        {
            InitializeComponent();
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
                    (BindingContext as ClientsViewModel).SelectedClient.ClientImageFront = ms.ToArray();
                }
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
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
                    (BindingContext as ClientsViewModel).SelectedClient.ClientImageBack = ms.ToArray();
                }
            }
        }
    }
}