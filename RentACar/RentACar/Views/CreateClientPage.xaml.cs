using Acr.UserDialogs;
using Plugin.Media;
using Plugin.Media.Abstractions;
using RentACar.Components;
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
            gjinia.ItemsSource = new List<string>(){ "Mashkull", "Femer" };
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            var bc = (ClientsViewModel)BindingContext;
            if(bc != null)
            {
                if(bc.IsEditing)
                {
                    (NavigationPage.GetTitleView(this) as NavTitleView).SubTitle = "Edito " + bc.SelectedClient.Name;
                }
            }
        }
        private async void Button_ClickedAsync(object sender, EventArgs e)
        {
            var result = await UserDialogs.Instance.ConfirmAsync("Zgjidhni opcionin per fotografi", "Fotografia", "Fotografo nga telefoni", "Zgjedh nga libraria");
            var cameraStatus = await Permissions.CheckStatusAsync<Permissions.Camera>();

            var storageStatus = await Permissions.CheckStatusAsync<Permissions.StorageRead>();

            if (cameraStatus != Xamarin.Essentials.PermissionStatus.Granted || storageStatus != Xamarin.Essentials.PermissionStatus.Granted)
            {
                var cameraresults = await Permissions.RequestAsync<Permissions.Camera>();
                var storageResults = await Permissions.RequestAsync<Permissions.StorageRead>();
            }
            cameraStatus = await Permissions.CheckStatusAsync<Permissions.Camera>();

            storageStatus = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
            if (cameraStatus != Xamarin.Essentials.PermissionStatus.Granted || storageStatus != Xamarin.Essentials.PermissionStatus.Granted)
            {
                var cameraresults = await Permissions.RequestAsync<Permissions.Camera>();
                var storageResults = await Permissions.RequestAsync<Permissions.StorageRead>();
            }
            cameraStatus = await Permissions.CheckStatusAsync<Permissions.Camera>();

            storageStatus = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
            if (result)
            {
                if (cameraStatus == Xamarin.Essentials.PermissionStatus.Granted && storageStatus == Xamarin.Essentials.PermissionStatus.Granted)
                {
                    var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
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
            else
            {
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
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var result = await UserDialogs.Instance.ConfirmAsync("Zgjidhni opcionin per fotografi", "Fotografia", "Fotografo nga telefoni", "Zgjedh nga libraria");
            var cameraStatus = await Permissions.CheckStatusAsync<Permissions.Camera>();

            var storageStatus = await Permissions.CheckStatusAsync<Permissions.StorageRead>();

            if (cameraStatus != Xamarin.Essentials.PermissionStatus.Granted || storageStatus != Xamarin.Essentials.PermissionStatus.Granted)
            {
                var cameraresults = await Permissions.RequestAsync<Permissions.Camera>();
                var storageResults = await Permissions.RequestAsync<Permissions.StorageRead>();
            }
            cameraStatus = await Permissions.CheckStatusAsync<Permissions.Camera>();

            storageStatus = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
            if (cameraStatus != Xamarin.Essentials.PermissionStatus.Granted || storageStatus != Xamarin.Essentials.PermissionStatus.Granted)
            {
                var cameraresults = await Permissions.RequestAsync<Permissions.Camera>();
                var storageResults = await Permissions.RequestAsync<Permissions.StorageRead>();
            }
            cameraStatus = await Permissions.CheckStatusAsync<Permissions.Camera>();

            storageStatus = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
            if (result)
            {
                if (cameraStatus == Xamarin.Essentials.PermissionStatus.Granted && storageStatus == Xamarin.Essentials.PermissionStatus.Granted)
                {
                    var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
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
            else
            {
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
        private async void Button_Clicked_patent(object sender, EventArgs e)
        {
            var result = await UserDialogs.Instance.ConfirmAsync("Zgjidhni opcionin per fotografi", "Fotografia", "Fotografo nga telefoni", "Zgjedh nga libraria");
            var cameraStatus = await Permissions.CheckStatusAsync<Permissions.Camera>();

            var storageStatus = await Permissions.CheckStatusAsync<Permissions.StorageRead>();

            if (cameraStatus != Xamarin.Essentials.PermissionStatus.Granted || storageStatus != Xamarin.Essentials.PermissionStatus.Granted)
            {
                var cameraresults = await Permissions.RequestAsync<Permissions.Camera>();
                var storageResults = await Permissions.RequestAsync<Permissions.StorageRead>();
            }
            cameraStatus = await Permissions.CheckStatusAsync<Permissions.Camera>();

            storageStatus = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
            if (cameraStatus != Xamarin.Essentials.PermissionStatus.Granted || storageStatus != Xamarin.Essentials.PermissionStatus.Granted)
            {
                var cameraresults = await Permissions.RequestAsync<Permissions.Camera>();
                var storageResults = await Permissions.RequestAsync<Permissions.StorageRead>();
            }
            cameraStatus = await Permissions.CheckStatusAsync<Permissions.Camera>();

            storageStatus = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
            if (result)
            {
                if (cameraStatus == Xamarin.Essentials.PermissionStatus.Granted && storageStatus == Xamarin.Essentials.PermissionStatus.Granted)
                {
                    var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
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
                        (BindingContext as ClientsViewModel).SelectedClient.ClientPatentShofer = ms.ToArray();
                    }
                }
            }
            else
            {
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
                        (BindingContext as ClientsViewModel).SelectedClient.ClientPatentShofer = ms.ToArray();
                    }
                }
            }
        }
    }
}