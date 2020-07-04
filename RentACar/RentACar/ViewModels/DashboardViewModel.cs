using Acr.UserDialogs;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using RentACar.Models;
using RentACar.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RentACar.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        private ObservableCollection<Car> _Cars;
        public ObservableCollection<Car> Cars
        {
            get { return _Cars; }
            set { SetProperty(ref _Cars, value); }
        }
        private ObservableCollection<Car> _AvailableCars;

        public ObservableCollection<Car> AvailableCars
        {
            get { return _AvailableCars; }
            set { SetProperty(ref _AvailableCars, value); }
        }
        private ObservableCollection<Car> _TakenCars;

        public ObservableCollection<Car> TakenCars
        {
            get { return _TakenCars; }
            set { SetProperty(ref _TakenCars, value); }
        }
        private ObservableCollection<Car> _UnregisteredCars;

        public ObservableCollection<Car> UnregisteredCars
        {
            get { return _UnregisteredCars; }
            set { SetProperty(ref _UnregisteredCars, value); }
        }
        private ObservableCollection<Car> _OnServiseCars;

        public ObservableCollection<Car> OnServiseCars
        {
            get { return _OnServiseCars; }
            set { SetProperty(ref _OnServiseCars, value); }
        }

        public ObservableCollection<Client> Clients;

        private int _carsCount;
        public int CarsCount
        {
            get { return _carsCount; }
            set { SetProperty(ref _carsCount, value); }
        }

        private Car _selectedCar;
        public Car SelectedCar
        {
            get { return _selectedCar; }
            set { SetProperty(ref _selectedCar, value); }
        }
        private string _email;
        public string EmailAddress
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }
        private bool _hasCars;
        public bool HasCars
        {
            get { return _hasCars; }
            set { SetProperty(ref _hasCars, value); }
        }

        private bool _hasAvailableCars;
        public bool HasAvailableCars
        {
            get { return _hasAvailableCars; }
            set { SetProperty(ref _hasAvailableCars, value); }
        }

        private bool _hasTakenCars;
        public bool HasTakenCars
        {
            get { return _hasTakenCars; }
            set { SetProperty(ref _hasTakenCars, value); }
        }

        private bool _hasUnregisteredCards;
        public bool HasUnregisteredCars
        {
            get { return _hasUnregisteredCards; }
            set { SetProperty(ref _hasUnregisteredCards, value); }
        }

        private bool _hasOnServiceCars;
        public bool HasOnServiceCars
        {
            get { return _hasOnServiceCars; }
            set { SetProperty(ref _hasOnServiceCars, value); }
        }

        private Rent _currentRent;
        public Rent CurrentRent
        {
            get { return _currentRent; }
            set { SetProperty(ref _currentRent, value); }
        }
        public byte[] ImgByte;
        public List<string> CarImageOptions => new List<string> { "Imazh", "URL" };
        public ICommand CarDetailsCommand { get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand PickPhotoCommand { get; set; }
        public ICommand CreateACarCommand { get; set; }
        public ICommand GoToCreateACarCommand { get; set; }
        public ICommand CarSettingsCommand { get; set; }
        public ICommand LeshoMeQiraCommand { get; set; }
        public ICommand GoToClientsCommand { get; set; }
        public DashboardViewModel()
        {
            CarDetailsCommand = new Command<Car>(async (c) => await CarDetailsAsync(c));
            LoginCommand = new Command(async () => await LoginAsync());
            PickPhotoCommand = new Command(async () => await PickPhotoAsync());
            CreateACarCommand = new Command(async () => await CreateACarAsync());
            GoToCreateACarCommand = new Command(async () => await GoToCreateACarAsync());
            AvailableCars = new ObservableCollection<Car>();
            TakenCars = new ObservableCollection<Car>();
            UnregisteredCars = new ObservableCollection<Car>();
            OnServiseCars = new ObservableCollection<Car>();
            CarSettingsCommand = new Command(async () => await GoToCreateACarAsync());
            LeshoMeQiraCommand = new Command(async () => await LeshoMeQiraAsync());
            GoToClientsCommand = new Command(async () => await GoToClientsPageAsync());
            //Task.Run(LoadRents);
        }

        private async Task GoToClientsPageAsync()
        {
            ClientsPage ClientsPage = new ClientsPage();
            App.instance.ClientsViewModel = new ClientsViewModel();
            ClientsPage.BindingContext = App.instance.ClientsViewModel;
            Clients = await App.instance.ClientsViewModel.LoadClientsFromRent(CurrentRent);
            (App.instance.MainPage as MainPage).IsPresented = false;
            await App.instance.PushAsyncNewPage(ClientsPage);
        }

        private async Task LeshoMeQiraAsync()
        {
            App.instance.ClientsViewModel = App.instance.ClientsViewModel ?? new ClientsViewModel();
            Clients = App.instance.ClientsViewModel.Clients;
            if(Clients == null || !Clients.Any())
                Clients = await App.instance.ClientsViewModel.LoadClientsFromRent(CurrentRent);
            RentACarViewModel rc = new RentACarViewModel(Clients);
            rc.SelectedCar = SelectedCar;
            RentACarPage RentACarPage = new RentACarPage();
            RentACarPage.BindingContext = rc;
            
            await App.instance.PushAsyncNewPage(RentACarPage);
        }

        private async Task GoToCarSettingsAsync()
        {
            await Task.FromResult(0);
        }

        private async Task GoToCreateACarAsync()
        {
            SelectedCar = new Car();
            CreateACarPage CreateACarPage = new CreateACarPage();
            CreateACarPage.BindingContext = this;
            (App.instance.MainPage as MainPage).IsPresented = false;
            await App.instance.PushAsyncNewPage(CreateACarPage);
        }

        private async Task CreateACarAsync()
        {
            var json = JsonConvert.SerializeObject(SelectedCar);
            var g = json.Remove(1, 7);
            App.client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
            HttpContent httpContent = new StringContent(g, Encoding.UTF8, "application/json");
            var response = await App.client.PostAsync(App.API_URL_BASE + "cars", httpContent);
            if (response.IsSuccessStatusCode)
            {
                UserDialogs.Instance.Alert("Vetura u shtua me sukses", "Sukses", "OK");
            }
            else
            {
                UserDialogs.Instance.Alert("Vetura nuk u shtua me sukses", "Error", "OK");
            }
            var responseString = await response.Content.ReadAsStringAsync();
            Car addedCar = JsonConvert.DeserializeObject<Car>(responseString);
            Cars.Add(addedCar);
            HasCars = Cars.Any();
        }

        private async Task PickPhotoAsync()
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
                    ImgByte = ms.ToArray();
                    SelectedCar.CarImage_1 = ImgByte;
                    
                    var mainpage = (MasterDetailPage)App.instance.CurrentPage;
                    //((MainPage.MainPage)mainpage).UpdateProfileImageSource();
                    //var detail = (SfidaDetailsPage)((NavigationPage)mainpage.Detail).CurrentPage;
                    //detail.UpdateSfidaPicture(file);
                    //PictureTaken = true;
                    var json = JsonConvert.SerializeObject(SelectedCar);
                    var g = json.Remove(1, 7);
                    App.client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                    HttpContent httpContent = new StringContent(g, Encoding.UTF8, "application/json");
                    var response = await App.client.PostAsync(App.API_URL_BASE + "cars", httpContent);
                    var responseString = await response.Content.ReadAsStringAsync();
                }
            }
            else
            {
                UserDialogs.Instance.Alert("Unable to take photos.", "Permissions Denied", "OK");
                //On iOS you may want to send your user to the settings screen.
                //CrossPermissions.Current.OpenAppSettings();
            }
        }

        private async Task LoginAsync()
        {
            using (UserDialogs.Instance.Loading("Duke u kyçur"))
            {
                var values = new Dictionary<string, string>
                {
                    { "emailaddress", EmailAddress },
                    { "password", Password }
                };
                var json = JsonConvert.SerializeObject(new AuthenticationData
                {
                    EmailAddress = EmailAddress ?? "bardhib",
                    Password = Password ?? "rent.123"
                });
                App.client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await App.client.PostAsync(App.API_URL_BASE + "rents", httpContent);
                AuthenticationData data = JsonConvert.DeserializeObject<AuthenticationData>(await response.Content.ReadAsStringAsync());
                if(data != null)
                {
                    DashboardPage dashboard = new DashboardPage();
                    dashboard.BindingContext = this;
                    CurrentRent = await LoadRentById(data.RentID);
                    if(CurrentRent != null) Cars = await LoadCarsFromRent(CurrentRent);
                    App.instance.ChangeDetailPage(dashboard);
                }
                else
                {
                    UserDialogs.Instance.Alert("Rishikoni kredencialet", "Error", "Ok");
                    return;
                }
            }
        }

        private async Task<ObservableCollection<Car>> LoadCarsFromRent(Rent currentRent)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                var response = await App.client.GetAsync(App.API_URL_BASE + "rents/cars/" + currentRent.Id);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    UserDialogs.Instance.Alert("Probleme me server, Provoni Perseri", "Error", "Ok");
                    return null;
                }
                else
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var cars = JsonConvert.DeserializeObject<ObservableCollection<Car>>(responseString);
                    AvailableCars = new ObservableCollection<Car>(cars.Where(x=> x.Statusi == Car.StatusTypes.Aktiv).ToList());
                    TakenCars = new ObservableCollection<Car>(cars.Where(x => x.Statusi == Car.StatusTypes.Zene).ToList());
                    UnregisteredCars = new ObservableCollection<Car>(cars.Where(x => x.Statusi == Car.StatusTypes.Pa_Regjistruar).ToList());
                    OnServiseCars = new ObservableCollection<Car>(cars.Where(x => x.Statusi == Car.StatusTypes.NeServis).ToList());
                    HasCars = cars.Any();
                    HasAvailableCars = AvailableCars.Any();
                    HasTakenCars = TakenCars.Any();
                    HasUnregisteredCars = UnregisteredCars.Any();
                    HasOnServiceCars = OnServiseCars.Any();
                    return cars;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task CarDetailsAsync(Car c)
        {
            SelectedCar = c;
            CarDetailsPage carDetailsPage = new CarDetailsPage();
            await App.instance.PushAsyncNewPage(carDetailsPage);
        }

        public async Task LoadRents()
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                var response = await App.client.GetAsync(App.API_URL_BASE + "rents");
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    UserDialogs.Instance.Alert("Probleme me server, Provoni Perseri", "Error", "Ok");
                    return;
                }
                else
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("Debuging : " + responseString);
                }
            }
            catch(Exception e)
            {

            }
        }

        public async Task<Rent> LoadRentById(int id)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                var response = await App.client.GetAsync(App.API_URL_BASE + "rents/" + id);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    UserDialogs.Instance.Alert("Probleme me server, Provoni Perseri", "Error", "Ok");
                    return null;
                }
                else
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var Rent = JsonConvert.DeserializeObject<Rent>(responseString);
                    Debug.WriteLine("Debuging : " + responseString);
                    UserDialogs.Instance.Alert(responseString, "Error", "Ok");
                    return Rent;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
