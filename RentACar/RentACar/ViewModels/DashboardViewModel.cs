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

        public ObservableCollection<Car> SearchedCars { get; set; }

        public List<Car> ServiceCloseCars => Cars.OrderBy(x => x.KmForService).ToList();
        public List<Car> RegistrationCloseCars => Cars.OrderBy(x => x.NextDateOfCheck).ToList();

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

        private bool _hasSearchedCars;
        public bool HasSearchedCars
        {
            get { return _hasSearchedCars; }
            set { SetProperty(ref _hasSearchedCars, value); }
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
        public virtual List<MenuItem> ProjectionItemsCars { get; protected set; }
        public virtual List<MenuItem> ProjectionItemsCarsRegisterAndService { get; protected set; }

        public byte[] ImgByte;
        public List<string> CarImageOptions => new List<string> { "Imazh", "URL" };

        private bool _isSuccess;
        public bool IsSuccess
        {
            get { return _isSuccess; }
            set { SetProperty(ref _isSuccess, value); }
        }
        public string PostPageInfo => "Vetura u kthye me sukses";

        private bool _isAdming;
        public bool IsAdmin
        {
            get { return _isAdming; }
            set { SetProperty(ref _isAdming, value); }
        }
        public AuthenticationData LoggedInUser;

        private bool _HasUndreadMessages;
        public bool HasUndreadMessages
        {
            get { return _HasUndreadMessages; }
            set { SetProperty(ref _HasUndreadMessages, value); }
        }

        private string _UnreadMessagesCount;
        public string UnreadMessagesCount
        {
            get { return _UnreadMessagesCount; }
            set { SetProperty(ref _UnreadMessagesCount, value); }
        }
        #region Commands
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
        public ICommand GoToCarsCommand { get; set; }
        public ICommand GoToCarsStatusesCommand { get; set; }
        public ICommand GoToCarsRegisterAndServiceCommand { get; set; }
        public ICommand GoToCarsServicesTimeCommand { get; set; }
        public ICommand SendMessage { get; set; }
        public ICommand GoToClientsPageCommand { get; set; }
        public ICommand GoToRaportetPageCommand { get; set; }
        public ICommand OpenNotificationsCommand { get; set; }
        #endregion

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
            GoToCarsCommand = new Command(async () => await GoToCarsCommandPageAsync());
            GoToPaymentsCommand = new Command(async () => await GoToPaymentsPageAsync());
            GoToRaportetPageCommand = new Command(async () => await GoToRaportetPageAsync());

            GetPdfCommand = new Command(async () => await GetPdfAsync());
            GoToDashboardCommand = new Command(async () => await GoToDashboardAsync());
            KtheVeturenCommand = new Command(async () => await KtheVeturenAsync());
            GoToCarsStatusesCommand = new Command(async () => await GoToCarsStatusesAsync());
            GoToCarsRegisterAndServiceCommand = new Command(async () => await GoToCarsRegisterAndServiceAsync());
            GoToCarsServicesTimeCommand = new Command(async () => await GoToCarsServicesTimeAsync());
            GoToClientsPageCommand = new Command(async () => await GoToClientsPageAsync());
            SendMessage = new Command(async () => await SendMessageAsync());
            OpenNotificationsCommand = new Command(async () => await OpenNotificationsAsync());
            var projectionItems = new List<MenuItem>() {
                    new MenuItem(){Name="transactions",TitleKey="Te gjitha", Parametar="all"},
                    new MenuItem(){Name="transactions",TitleKey="Perfunduara", Parametar="finished"},
                    new MenuItem(){Name="transactions",TitleKey="Ne Vazhdim", Parametar="ongoing"},
                    new MenuItem(){Name="transactions",TitleKey="Vonuara", Parametar="late"}
            };
            var projectionItemsCars = new List<MenuItem>()
            {
                    new MenuItem(){Name="cars",TitleKey="TE GJITHA VETURAT", Parametar="all"},
                    new MenuItem(){Name="transactions",TitleKey="TE LIRA", Parametar="finished"},
                    new MenuItem(){Name="transactions",TitleKey="TE LESHUARA ME QIRA", Parametar="ongoing"},
                    new MenuItem(){Name="transactions",TitleKey="TE PAREGJISTRUARA", Parametar="late" },
                    new MenuItem(){Name="transactions",TitleKey="TEK SERVISI", Parametar="servis"}
            };
            var projectionItemsRegisterAndService = new List<MenuItem>() {
                    new MenuItem(){Name="transactions",TitleKey="Veturat per servisim", Parametar="all"},
                    new MenuItem(){Name="transactions",TitleKey="Veturat per regjistrim", Parametar="late"}
            };
            RentedCarsByRentId = new ObservableCollection<RentedCar>();
            ProjectionItems = projectionItems;
            ProjectionItemsCars = projectionItemsCars;
            ProjectionItemsCarsRegisterAndService = projectionItemsRegisterAndService;
            LatestTransactions = new ObservableCollection<RentedCar>();
            LatestTransactionsLate = new ObservableCollection<RentedCar>();
            LatestTransactionsOnGoing = new ObservableCollection<RentedCar>();
            LatestTransactionsFinished = new ObservableCollection<RentedCar>();
            Clients = new ObservableCollection<Client>();
            RentedCarsByRentId = new ObservableCollection<RentedCar>();
            SearchedCars = new System.Collections.ObjectModel.ObservableCollection<Models.Car>();
            HasSearchedCars = false;
            //Task.Run(LoadRents);
            LoggedInUser = new AuthenticationData();
            HasUndreadMessages = true;
            UnreadMessagesCount = " 3";
        }

        private async Task OpenNotificationsAsync()
        {
            using (UserDialogs.Instance.Loading("Duke ngarkuar mesazhet"))
            {
                MessagesPage MessagesPage = new MessagesPage();
                App.instance.MessagesViewModel = App.instance.MessagesViewModel != null ? App.instance.MessagesViewModel : new MessagesViewModel();
                await App.instance.MessagesViewModel.GetMessages();
                MessagesPage.BindingContext = App.instance.MessagesViewModel;
                (App.instance.MainPage as MainPage).IsPresented = false;
                await App.instance.PushAsyncNewPage(MessagesPage);
            }
        }

        protected async Task CheckForAlerts()
        {
            try
            {
                if (Cars.Any())
                {
                    foreach (var car in Cars)
                    {
                        if ((car.NextDateOfCheck - DateTime.Now).TotalDays <= 2)
                        {
                            var res = await UserDialogs.Instance.ConfirmAsync("Vetura " + car.Name + " me targat :" + car.Targa + " duhet te regjistrohet", "Koha per regjistrim", "Perfundo regjistrimin", "Me Vone");
                            if (res)
                            {
                                using (UserDialogs.Instance.Loading("Duke regjistruar veturen"))
                                {
                                    Notifications notifications = new Notifications
                                    {
                                        DateReceived = DateTime.Now,
                                        HasInteraction = true,
                                        IsImportant = true,
                                        IsRead = true,
                                        Message = "Vetura " + car.Name + " me targat :" + car.Targa + " duhet te regjistrohet",
                                        Title = "Regjistrimi: " + car.Targa,
                                        RentID = CurrentRent.Id
                                    };
                                    var serializedNotification = JsonConvert.SerializeObject(notifications);
                                    HttpContent httpContentNotification = new StringContent(serializedNotification, Encoding.UTF8, "application/json");
                                    var notificationAdded = await App.client.PostAsync(App.API_URL_BASE + "rents/notifications/add", httpContentNotification);
                                    if(!notificationAdded.IsSuccessStatusCode)
                                    {
                                        UserDialogs.Instance.Alert("Mesazhi deshtoi te shtohet ne databaze", "Deshtim Mesazhi");
                                    }
                                    var json = JsonConvert.SerializeObject(car);
                                    App.client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                                    HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                                    var response = await App.client.PutAsync(App.API_URL_BASE + "rents/cars/register", httpContent);
                                    if (response.IsSuccessStatusCode)
                                    {
                                        UserDialogs.Instance.Alert("Vetura " + car.Name + " me targat :" + car.Targa + " u regjistrua per 6 muaj me sukses", "Sukses", "OK");
                                    }
                                    else
                                    {
                                        UserDialogs.Instance.Alert("Vetura " + car.Name + " me targat :" + car.Targa + " nuk u regjistrua me sukses", "Error", "OK");
                                        continue;
                                    }
                                    var responseString = await response.Content.ReadAsStringAsync();
                                    var RegisteredCar = JsonConvert.DeserializeObject<Car>(responseString);
                                    car.NextDateOfCheck = RegisteredCar.NextDateOfCheck;
                                }
                            }
                            else
                            {
                                Notifications notifications = new Notifications
                                {
                                    DateReceived = DateTime.Now,
                                    HasInteraction = true,
                                    IsImportant = true,
                                    IsRead = false,
                                    Message = "Vetura " + car.Name + " me targat :" + car.Targa + " duhet te regjistrohet",
                                    Title = "Regjistrimi: " + car.Targa,
                                    RentID = CurrentRent.Id
                                };
                                var serializedNotification = JsonConvert.SerializeObject(notifications);
                                HttpContent httpContentNotification = new StringContent(serializedNotification, Encoding.UTF8, "application/json");
                                var notificationAdded = await App.client.PostAsync(App.API_URL_BASE + "rents/notifications/add", httpContentNotification);
                                if (!notificationAdded.IsSuccessStatusCode)
                                {
                                    UserDialogs.Instance.Alert("Mesazhi deshtoi te shtohet ne databaze", "Deshtim Mesazhi");
                                }
                            }
                        }
                    }
                }
            }catch(Exception e)
            {
                var res = e.Message;
            }
            
        }

        private async Task DisplayAlert(string v1, string v2,string v3 = "Okay")
        {
            await DisplayAlert(v1, v2, v3);
        }

        private async Task SendMessageAsync()
        {
            var message = new EmailMessage
            {
                Subject = "Subject",
                Body = "body",
                To = new List<string> { "ww" },
                //Cc = ccRecipients,
                //Bcc = bccRecipients
            };

            await Email.ComposeAsync(message);
        }

        private async Task GoToCarsServicesTimeAsync()
        {
            using (UserDialogs.Instance.Loading("Loading"))
            {
                CarsServiseTimePage CarsServiseTimePage = new CarsServiseTimePage();
                CarsServiseTimePage.BindingContext = this;
                (App.instance.MainPage as MainPage).IsPresented = false;
                await App.instance.PushAsyncNewPage(CarsServiseTimePage);
            }
        }

        private async Task GoToCarsRegisterAndServiceAsync()
        {
            using (UserDialogs.Instance.Loading("Loading"))
            {
                CarsTimeForRegistrationAndServisePage CarsTimeForRegistrationAndServisePage = new CarsTimeForRegistrationAndServisePage();
                CarsTimeForRegistrationAndServisePage.BindingContext = this;
                (App.instance.MainPage as MainPage).IsPresented = false;
                await App.instance.PushAsyncNewPage(CarsTimeForRegistrationAndServisePage);
            }
        }

        private async Task GoToCarsStatusesAsync()
        {
            using (UserDialogs.Instance.Loading("Loading"))
            {
                CarsStatusPage CarsStatusPage = new CarsStatusPage();
                CarsStatusPage.BindingContext = this;
                (App.instance.MainPage as MainPage).IsPresented = false;
                await App.instance.PushAsyncNewPage(CarsStatusPage);
            }
        }
        
        private async Task GoToCarsCommandPageAsync()
        {
            using (UserDialogs.Instance.Loading("Loading"))
            {
                CarsOperationPage CarsOperationPage = new CarsOperationPage();
                CarsOperationPage.BindingContext = this;
                (App.instance.MainPage as MainPage).IsPresented = false;
                await App.instance.PushAsyncNewPage(CarsOperationPage);
            }
        }

        private async Task KtheVeturenAsync()
        {
            using (UserDialogs.Instance.Loading("Loading"))
            {
                CurrentRentedCar.KohaAktualeEKthimit = DateTime.Now;
                var json = JsonConvert.SerializeObject(CurrentRentedCar);
                App.client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await App.client.PutAsync(App.API_URL_BASE + "rentedcars/update", httpContent);
                if (response.IsSuccessStatusCode)
                {
                    //UserDialogs.Instance.Alert("Vetura u kthye me sukses", "Sukses", "OK");
                }
                else
                {
                    //UserDialogs.Instance.Alert("Vetura nuk u kthye  me sukses", "Error", "OK");
                    IsSuccess = false;
                    return;
                }
                var responseString = await response.Content.ReadAsStringAsync();
                RentedCar addedCar = JsonConvert.DeserializeObject<RentedCar>(responseString);
                IsSuccessfullRent = addedCar != null ? true : false;
                var page = new PostRentedCarPage();
                page.BindingContext = this;
                IsSuccess = true;
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
                car.Km = CurrentRentedCar.KilometrazhiAktual;
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
            switch (Device.RuntimePlatform)
            {
                case Device.Android:
                    var statusread = await Permissions.RequestAsync<Permissions.StorageRead>();
                    var statuswrite = await Permissions.RequestAsync<Permissions.StorageWrite>();
                    if (statusread != Xamarin.Essentials.PermissionStatus.Granted || statuswrite != Xamarin.Essentials.PermissionStatus.Granted)
                    {
                        var cameraresults = await Permissions.RequestAsync<Permissions.StorageWrite>();
                        var storageResults = await Permissions.RequestAsync<Permissions.StorageWrite>();
                    }
                    break;
            }
            var filter = new FilterRaports();
            var json = JsonConvert.SerializeObject(filter);
            App.client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await App.client.PostAsync(App.API_URL_BASE + "rentedcars/pdf/filtered", httpContent);
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
            using (UserDialogs.Instance.Loading("Duke hapur pagesat"))
            {
                PaymentOperationPage PaymentPage = new PaymentOperationPage();
                App.instance.PaymentsViewModel = new PaymentsViewModel();
                PaymentPage.BindingContext = App.instance.PaymentsViewModel;
                (App.instance.MainPage as MainPage).IsPresented = false;
                await App.instance.PushAsyncNewPage(PaymentPage);
            }
        }

        private async Task GoToRaportetPageAsync()
        {
            using (UserDialogs.Instance.Loading("Duke hapur raportet"))
            {
                RaportsOperationPage RaportsOperationPage = new RaportsOperationPage();
                App.instance.PaymentsViewModel = new PaymentsViewModel();
                RaportsOperationPage.BindingContext = App.instance.PaymentsViewModel;
                (App.instance.MainPage as MainPage).IsPresented = false;
                await App.instance.PushAsyncNewPage(RaportsOperationPage);
            }
        }
        private async Task GoToClientsPageAsync()
        {
            App.instance.ClientsViewModel = App.instance.ClientsViewModel  == null ? new ClientsViewModel() : App.instance.ClientsViewModel;
            await App.instance.ClientsViewModel.GoToClientsPageAsync();
        }
        private async Task GetClients()
        {
            var clients = await App.instance.ClientsViewModel.LoadClientsFromRent(CurrentRent);
            if(clients != null && clients.Any())
            {
                foreach (var client in clients)
                {
                    Clients.Add(client);
                }
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
            using (UserDialogs.Instance.Loading("Loading"))
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
                if(Cars != null && !Cars.Any())
                {
                    Cars = new ObservableCollection<Car>();
                }
                Cars.Add(addedCar);
                HasCars = Cars.Any();
                OnPropertyChanged("Cars");
                OnPropertyChanged("HasCars");
                HasAvailableCars = true;
                AvailableCars.Add(addedCar);
                OnPropertyChanged("HasAvailableCars");
                OnPropertyChanged("AvailableCars");

            }
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
            try
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
                        Password = Password ?? "zZzWhite_1@"
                    });
                    App.client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                    HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await App.client.PostAsync(App.API_URL_BASE + "rents", httpContent);
                    if (response.IsSuccessStatusCode)
                    {
                    }
                    else
                    {
                        UserDialogs.Instance.Alert("Probleme me server", "Error", "OK");
                    }
                    AuthenticationData data = JsonConvert.DeserializeObject<AuthenticationData>(await response.Content.ReadAsStringAsync());
                    LoggedInUser = data;
                    IsAdmin = LoggedInUser.IsAdmin;
                    if (data != null)
                    {
                        DashboardPage dashboard = new DashboardPage();
                        dashboard.BindingContext = this;
                        UserDialogs.Instance.ShowLoading("Duke ngarkuar rentin");
                        CurrentRent = await LoadRentById(data.RentID);
                        UserDialogs.Instance.ShowLoading("Duke ngarkuar veturat");
                        if (CurrentRent != null) Cars = await LoadCarsFromRent(CurrentRent);
                        var notifications = await App.client.GetAsync(App.API_URL_BASE + "rents/notifications/count/" + CurrentRent.Id);
                        var result = await notifications.Content.ReadAsStringAsync();
                        UnreadMessagesCount = " " + result;
                        App.instance.ClientsViewModel = App.instance.ClientsViewModel ?? new ClientsViewModel();
                        UserDialogs.Instance.ShowLoading("Duke ngarkuar klientet");
                        await GetClients();
                        UserDialogs.Instance.HideLoading();
                        App.instance.ChangeDetailPage(dashboard);
                        await CheckForAlerts();
                    }
                    else
                    {
                        UserDialogs.Instance.Alert("Rishikoni kredencialet", "Error", "Ok");
                        return;
                    }
                }
            }
            catch(Exception e)
            {

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
                    Cars = cars;
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
                if(CurrentRentedCar == null)
                {

                }
                CurrentRentedCar.CmimiTotal = (int)(CurrentRentedCar.KohaEKthimit - CurrentRentedCar.KohaELeshimit).TotalDays * CurrentRentedCar.CmimiDitor;
                CurrentRentedCar.KaPesuarAksident = false;
                CurrentRentedCar.KaThyerNdonjeRregull = false;
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

