using Acr.UserDialogs;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using RentACar.Dependencies;
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
using MenuItem = RentACar.Models.MenuItem;

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

        public ObservableCollection<Client> Clients { get; set; }

        public ObservableCollection<RentedCar> RentedCarsByRentId { get; set; }
        public ObservableCollection<RentedCar> LatestTransactions { get; set; }
        public ObservableCollection<RentedCar> LatestTransactionsOnGoing { get; set; }
        public ObservableCollection<RentedCar> LatestTransactionsLate { get; set; }
        public ObservableCollection<RentedCar> LatestTransactionsFinished { get; set; }

        public bool HasLatestTransactions { get; set; }
        public bool HasLatestTransactionsOnGoing { get; set; }
        public bool HasLatestTransactionsLate { get; set; }
        public bool HasLatestTransactionsFinished { get; set; }

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

        private decimal _teHyrat;
        public decimal TeHyrat
        {
            get { return _teHyrat; }
            set { SetProperty(ref _teHyrat, value); }
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
        private bool _IsSuccessfullRent;
        public bool IsSuccessfullRent
        {
            get { return _IsSuccessfullRent; }
            set { SetProperty(ref _IsSuccessfullRent, value); }
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
        private RentedCar _currentRentedCar;
        public RentedCar CurrentRentedCar
        {
            get { return _currentRentedCar; }
            set { SetProperty(ref _currentRentedCar, value); }
        }
        protected int tabPosition = 0;
        public int TabPosition
        {
            get { return tabPosition; }
            set { SetProperty(ref tabPosition, value); }
        }
        public virtual List<MenuItem> ProjectionItems { get ; protected set; }

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
        public ICommand GetPdfCommand { get; set; }
        public ICommand GoToDashboardCommand { get; set; }
        public ICommand KtheVeturenCommand { get; set; }
        public ICommand GoToPaymentsCommand { get; set; }
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
            GoToPaymentsCommand = new Command(async () => await GoToPaymentsPageAsync());
            GetPdfCommand = new Command(async () => await GetPdfAsync());
            GoToDashboardCommand = new Command(async () => await GoToDashboardAsync());
            KtheVeturenCommand = new Command(async () => await KtheVeturenAsync());
            var projectionItems = new List<MenuItem>() {
                    new MenuItem(){Name="transactions",TitleKey="Te gjitha", Parametar="all"},
                    new MenuItem(){Name="transactions",TitleKey="Perfunduara", Parametar="finished"},
                    new MenuItem(){Name="transactions",TitleKey="Ne Vazhdim", Parametar="ongoing"},
                    new MenuItem(){Name="transactions",TitleKey="Vonuara", Parametar="late"}
            };
            RentedCarsByRentId = new ObservableCollection<RentedCar>();
            ProjectionItems = projectionItems;
            LatestTransactions = new ObservableCollection<RentedCar>();
            LatestTransactionsLate = new ObservableCollection<RentedCar>();
            LatestTransactionsOnGoing = new ObservableCollection<RentedCar>();
            LatestTransactionsFinished = new ObservableCollection<RentedCar>();
            Clients = new ObservableCollection<Client>();
            RentedCarsByRentId = new ObservableCollection<RentedCar>();
            //Task.Run(LoadRents);
        }

        private async Task KtheVeturenAsync()
        {
            using (UserDialogs.Instance.Loading("Loading"))
            {
                
                var json = JsonConvert.SerializeObject(CurrentRentedCar);
                App.client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await App.client.PutAsync(App.API_URL_BASE + "rentedcars/update", httpContent);
                if (response.IsSuccessStatusCode)
                {
                    UserDialogs.Instance.Alert("Vetura u kthye me sukses", "Sukses", "OK");
                }
                else
                {
                    UserDialogs.Instance.Alert("Vetura nuk u kthye  me sukses", "Error", "OK");
                }
                var responseString = await response.Content.ReadAsStringAsync();
                RentedCar addedCar = JsonConvert.DeserializeObject<RentedCar>(responseString);
                IsSuccessfullRent = addedCar != null ? true : false;
                var page = new PostRentedCarPage();
                page.BindingContext = this;
                var car = TakenCars.FirstOrDefault(x => x.Id == addedCar.CarId);
                LatestTransactionsFinished.Add(addedCar);
                LatestTransactionsOnGoing.Remove(addedCar);
                if (LatestTransactionsFinished.Count > 5)
                {
                    App.instance.DashboardViewModel.LatestTransactions.RemoveAt(0);
                }
                RentedCarsByRentId.Remove(CurrentRentedCar);
                RentedCarsByRentId.Add(addedCar);
                var teHyrat = RentedCarsByRentId.ToList().FindAll(x => x.IsFinished && x.KohaELeshimit >= DateTime.Now.AddMonths(-1));
                decimal count = 0.00m;
                foreach (var item in teHyrat)
                {
                    count += (decimal)item.DitetELeshimit * item.CmimiDitor;
                }
                TeHyrat = count;
                OnPropertyChanged("TeHyrat");
                car.Statusi = Car.StatusTypes.Aktiv;
                AvailableCars.Add(car);
                TakenCars.Remove(car);
                var ongoing = LatestTransactionsOnGoing.FirstOrDefault(x => x.Id == addedCar.Id);
                if (ongoing != null)
                {
                    LatestTransactionsOnGoing.Remove(ongoing);
                }
                OnPropertyChanged("LatestTransactionsOnGoing");
                HasTakenCars = TakenCars.Any();
                HasAvailableCars = AvailableCars.Any();
                OnPropertyChanged("HasTakenCars");
                await Task.Delay(500);

                App.instance.ChangeDetailPage(page);
            }
        }
        public void PropertyChangedList()
        {
            HasLatestTransactions = LatestTransactions.Any();
            HasLatestTransactionsFinished = LatestTransactionsFinished.Any();
            HasLatestTransactionsLate = LatestTransactionsLate.Any();
            HasLatestTransactionsOnGoing = LatestTransactionsOnGoing.Any();
            OnPropertyChanged("TeHyrat");
            OnPropertyChanged("AvailableCars");
            OnPropertyChanged("TakenCars");
            OnPropertyChanged("UnregisteredCars");
            OnPropertyChanged("OnServiseCars");
            OnPropertyChanged("LatestTransactionsOnGoing");
            OnPropertyChanged("HasLatestTransactions");
            OnPropertyChanged("HasLatestTransactionsFinished");
            OnPropertyChanged("HasLatestTransactionsLate");
            OnPropertyChanged("HasLatestTransactionsOnGoing");
            HasTakenCars = TakenCars.Any();
            OnPropertyChanged("HasTakenCars");
        }
        private async Task GoToDashboardAsync()
        {
            DashboardPage dashboardPage = new DashboardPage();
            dashboardPage.BindingContext = this;
            App.instance.ChangeDetailPage(dashboardPage);
        }

        private async Task<object> GetPdfAsync()
        {
            //var statusread = await Permissions.RequestAsync<Permissions.StorageRead>();
            //var statuswrite = await Permissions.RequestAsync<Permissions.StorageWrite>();
            //if(statusread != Xamarin.Essentials.PermissionStatus.Granted || statuswrite != Xamarin.Essentials.PermissionStatus.Granted)
            //{
                //var cameraresults = await Permissions.RequestAsync<Permissions.StorageWrite>();
                //var storageResults = await Permissions.RequestAsync<Permissions.StorageWrite>();
            //}
            var response = await App.client.GetAsync(App.API_URL_BASE + "rentedcars/pdf/javore");
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                UserDialogs.Instance.Alert("Probleme me server, Provoni Perseri", "Error", "Ok");
                return null;
            }
            else
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var pdfcontent = JsonConvert.DeserializeObject<byte[]>(responseString);
                using(MemoryStream ms = new MemoryStream(pdfcontent))
                {
                    //await Xamarin.Forms.DependencyService.Get<ISave>().SaveAndView("Output.pdf", "application/pdf", ms);

                }
                DependencyService.Get<IFileLauncher>().Open(pdfcontent, $"Rent---.pdf");
                return null;
            }

        }
        private async Task GoToPaymentsPageAsync()
        {
            using (UserDialogs.Instance.Loading("Loading"))
            {
                PaymentOperationPage PaymentPage = new PaymentOperationPage();
                App.instance.PaymentsViewModel = new PaymentsViewModel();
                PaymentPage.BindingContext = App.instance.PaymentsViewModel;
                (App.instance.MainPage as MainPage).IsPresented = false;
                await App.instance.PushAsyncNewPage(PaymentPage);
            }
        }
        private async Task GoToClientsPageAsync()
        {
            using (UserDialogs.Instance.Loading("Loading"))
            {
                ClientsOperationPage ClientsPage = new ClientsOperationPage();
                App.instance.ClientsViewModel = new ClientsViewModel();
                ClientsPage.BindingContext = App.instance.ClientsViewModel;
                Clients = Clients.Any() ? Clients : await App.instance.ClientsViewModel.LoadClientsFromRent(CurrentRent);
                (App.instance.MainPage as MainPage).IsPresented = false;
                await App.instance.PushAsyncNewPage(ClientsPage);
            }
        }
        private async Task GetClients()
        {
            var clients = await App.instance.ClientsViewModel.LoadClientsFromRent(CurrentRent);
            foreach(var client in clients)
            {
                Clients.Add(client);
            }
        }
        private async Task LeshoMeQiraAsync()
        {
            using (UserDialogs.Instance.Loading("Loading"))
            {
                App.instance.ClientsViewModel = App.instance.ClientsViewModel ?? new ClientsViewModel();
                Clients = App.instance.ClientsViewModel.Clients;
                if (Clients == null || !Clients.Any())
                    Clients = await App.instance.ClientsViewModel.LoadClientsFromRent(CurrentRent);
                RentACarViewModel rc = new RentACarViewModel(Clients);
                rc.SelectedCar = SelectedCar;
                RentACarPage RentACarPage = new RentACarPage();
                RentACarPage.BindingContext = rc;

                await App.instance.PushAsyncNewPage(RentACarPage);
            }
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
            SelectedCar.RentId = CurrentRent.Id;
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
            OnPropertyChanged("Cars");
            OnPropertyChanged("HasCars");
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
                    //App.instance.ClientsViewModel = App.instance.ClientsViewModel ?? new ClientsViewModel();
                    //await GetClients();
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
            if(SelectedCar.Statusi == Car.StatusTypes.Zene)
            {
                CurrentRentedCar = RentedCarsByRentId.FirstOrDefault(x => x.CarId == SelectedCar.Id && x.IsFinished == false);
                CurrentRentedCar.CmimiTotal = (int)(CurrentRentedCar.KohaEKthimit - CurrentRentedCar.KohaELeshimit).TotalDays * CurrentRentedCar.CmimiDitor;
            }
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
                    var responseForRentedCars = await App.client.GetAsync(App.API_URL_BASE + "rentedcars/rent/" + Rent.Id);
                    RentedCarsByRentId = JsonConvert.DeserializeObject<ObservableCollection<RentedCar>>( await responseForRentedCars.Content.ReadAsStringAsync());
                    if(RentedCarsByRentId != null)
                    {
                        var list = RentedCarsByRentId.OrderBy(i => i.DitetELeshimit);
                        foreach (var item in list)
                        {
                            if (LatestTransactions.Count < 5)
                                LatestTransactions.Add(item);
                            if (item.KohaEKthimit > DateTime.Now && LatestTransactionsOnGoing.Count < 5 && !item.IsFinished)
                            {
                                LatestTransactionsOnGoing.Add(item);
                            }
                            if (item.KohaEKthimit < DateTime.Now && LatestTransactionsLate.Count < 5)
                            {
                                LatestTransactionsLate.Add(item);
                            }
                            if (item.IsFinished && LatestTransactionsFinished.Count < 5)
                            {
                                LatestTransactionsFinished.Add(item);
                            }
                        }
                        var teHyrat = RentedCarsByRentId.ToList().FindAll(x => x.IsFinished && x.KohaELeshimit >= DateTime.Now.AddMonths(-1));
                        decimal count = 0.00m;
                        foreach (var item in teHyrat)
                        {
                            count += (decimal)item.DitetELeshimit * item.CmimiDitor;
                        }
                        TeHyrat = count;
                    }
                    HasLatestTransactions = LatestTransactions.Any();
                    HasLatestTransactionsFinished = LatestTransactionsFinished.Any();
                    HasLatestTransactionsLate = LatestTransactionsLate.Any();
                    HasLatestTransactionsOnGoing = LatestTransactionsOnGoing.Any();
                    OnPropertyChanged("HasLatestTransactions");
                    OnPropertyChanged("HasLatestTransactionsFinished");
                    OnPropertyChanged("HasLatestTransactionsLate");
                    OnPropertyChanged("HasLatestTransactionsOnGoing");
                    
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

