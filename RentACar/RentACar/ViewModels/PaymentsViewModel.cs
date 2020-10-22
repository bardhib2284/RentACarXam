using Acr.UserDialogs;
using Newtonsoft.Json;
using RentACar.Dependencies;
using RentACar.Models;
using RentACar.Views;
using RentACarAPI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace RentACar.ViewModels
{
    public class PaymentsViewModel : ViewModelBase
    {
        private int _CarPosition;
        public int CarPosition
        {
            get { return _CarPosition; }
            set { SetProperty(ref _CarPosition, value); }
        }
        public ObservableCollection<Client> Clients { get; set; }
        public ObservableCollection<Car> Cars { get; private set; }
        public FilterRaports FilterRaports { get; set; }
        public ICommand GoToCmimetCommand { get; set; }
        public ICommand GoToTarifatCommand { get; set; }
        public ICommand GoToSezonetCommand { get; set; }
        public ICommand GoToZbritjetCommand { get; set; }
        public ICommand AddASeasonCommand { get; set; }
        public ICommand GoToCreateSeazonCommand { get; set; }
        public ICommand GoToPersonalizedReportCommand { get; set; }
        public ICommand GeneratePdfWithFilterCommand { get; set; }
        public ICommand GeneratePdfWeeklyCommand { get; set; }
        public ICommand GeneratePdfDailyCommand { get; set; }
        public ICommand GeneratePdfMonthlyCommand { get; set; }
        public ICommand AddACmimCommand { get; set; }
        public ICommand GoToCreateCmimeCommand { get; set; }
        public ICommand GoToDailyRaportsCommand { get; set; }
        public ICommand GeneratePdfSeasonCommand { get; set; }

        private DateTime _dailyRaports;
        public DateTime DailyRaports
        {
            get { return _dailyRaports; }
            set { SetProperty(ref _dailyRaports, value); }
        }

        #region Cmimet
        public Cmimet SelectedCmimi
        {
            get; set;
        }
        public ObservableCollection<Cmimet> Cmimet { get; private set; }
        private bool hasCmime;
        public bool HasCmime
        {
            get { return hasCmime; }
            set { SetProperty(ref hasCmime, value); }
        }
        #endregion
        public Sezonet SelectedSezoni
        {
            get;set;
        }
        public ObservableCollection<Sezonet> Sezonet { get; private set; }
        private bool hasSezone;
        public bool HasSezone
        {
            get { return hasSezone; }
            set { SetProperty(ref hasSezone, value); }
        }
        public Client SelectedClient { get; set; }
        private bool _isDaily;
        public bool IsDaily
        {
            get { return _isDaily; }
            set { SetProperty(ref _isDaily, value); }
        }
        private bool _isSeason;
        public bool IsSeason
        {
            get { return _isSeason; }
            set { SetProperty(ref _isSeason, value); }
        }

        public PaymentsViewModel()
        {
            GoToCmimetCommand = new Command(async () => await GoToCmimetAsync());
            GoToTarifatCommand = new Command(async () => await GoToTarifatAsync());
            GoToZbritjetCommand = new Command(async () => await GoToZbritjetAsync());
            GoToSezonetCommand = new Command(async () => await GoToSezonetAsync());
            AddASeasonCommand = new Command(async () => await CreateASeasonAsync());
            AddACmimCommand = new Command(async () => await CreateACmimAsync());
            GoToCreateCmimeCommand = new Command(async () => await GoToCreateCmimetAsync());
            GoToCreateSeazonCommand = new Command(async () => await GoToCreateSeazonAsync());
            GoToPersonalizedReportCommand = new Command(async () => await GoToPersonalizedReportAsync());
            GeneratePdfWithFilterCommand = new Command(async () => await GeneratePdfWithFilterAsync());
            GeneratePdfWeeklyCommand = new Command(async () => await GeneratePdfWeekly());
            GeneratePdfDailyCommand = new Command(async () => await GeneratePdfDailyAsync());
            GeneratePdfMonthlyCommand = new Command(async () => await GeneratePdfMonthlyAsync());
            GoToDailyRaportsCommand = new Command(async (c) => await GoToDailyRaportsAsync(c.ToString()));
            GeneratePdfSeasonCommand = new Command(async () => await GeneratePdfSeasonAsync());
            SelectedSezoni = new Sezonet();
            SelectedCmimi = new Cmimet();
            Sezonet = new ObservableCollection<Sezonet>();
            Cmimet = new ObservableCollection<Cmimet>();
            DailyRaports = DateTime.Now;
        }

        private async Task GeneratePdfSeasonAsync()
        {
            using (UserDialogs.Instance.Loading("Loading"))
            {
                DateTime reuqestedDate = DateTime.Now;
                var selectedSeason = Sezonet.FirstOrDefault(x => x.Emri == SelectedSezoni.Emri);
                var json = JsonConvert.SerializeObject(selectedSeason);
                App.client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await App.client.PostAsync(App.API_URL_BASE + "rentedcars/pdf/season", httpContent);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    UserDialogs.Instance.Alert("Probleme me server, Provoni Perseri", "Error", "Ok");
                }
                else
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var pdfcontent = JsonConvert.DeserializeObject<byte[]>(responseString);
                    using (MemoryStream ms = new MemoryStream(pdfcontent))
                    {
                        //await Xamarin.Forms.DependencyService.Get<ISave>().SaveAndView("Output.pdf", "application/pdf", ms);

                    }
                    DependencyService.Get<IFileLauncher>().Open(pdfcontent, selectedSeason.Emri +"-"+ DateTime.Now.Date + "--.pdf");
                }
            }
        }

        private async Task GoToDailyRaportsAsync(string name)
        {
            using (UserDialogs.Instance.Loading("Loading"))
            {
                if(name == "Daily")
                {
                    DailyReportsPage DailyReportsPage = new DailyReportsPage();
                    DailyReportsPage.BindingContext = this;
                    IsDaily = true;
                    IsSeason = false;
                    await App.instance.PushAsyncNewPage(DailyReportsPage);
                }
                else if(name == "Season")
                {
                    DailyReportsPage DailyReportsPage = new DailyReportsPage();
                    DailyReportsPage.BindingContext = this;
                    IsSeason = true;
                    IsDaily = false;
                    await GetSezonet();
                    await App.instance.PushAsyncNewPage(DailyReportsPage);
                }
               
            }
        }

        private async Task CreateACmimAsync()
        {
            SelectedCmimi.SeasonID = SelectedSezoni.id;
            var json = JsonConvert.SerializeObject(SelectedCmimi);
            var g = json.Remove(1, 7);
            App.client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
            HttpContent httpContent = new StringContent(g, Encoding.UTF8, "application/json");
            var response = await App.client.PostAsync(App.API_URL_BASE + "payments/cmimet", httpContent);
            if (response.IsSuccessStatusCode)
            {
                UserDialogs.Instance.Alert("Cmimi u shtua me sukses", "Sukses", "OK");
            }
            else
            {
                UserDialogs.Instance.Alert("Cmimi nuk u shtua me sukses", "Error", "OK");
            }
            var responseString = await response.Content.ReadAsStringAsync();
            Cmimet addedCmimet = JsonConvert.DeserializeObject<Cmimet>(responseString);
            Cmimet.Add(addedCmimet);
            HasCmime = Cmimet.Any();
            OnPropertyChanged("HasCmime");
        }
        private async Task GeneratePdfMonthlyAsync()
        {
            using (UserDialogs.Instance.Loading("Loading"))
            {
                DateTime reuqestedDate = DateTime.Now;
                var json = JsonConvert.SerializeObject(reuqestedDate);
                App.client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await App.client.PostAsync(App.API_URL_BASE + "rentedcars/pdf/monthly", httpContent);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    UserDialogs.Instance.Alert("Probleme me server, Provoni Perseri", "Error", "Ok");
                }
                else
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var pdfcontent = JsonConvert.DeserializeObject<byte[]>(responseString);
                    using (MemoryStream ms = new MemoryStream(pdfcontent))
                    {
                        //await Xamarin.Forms.DependencyService.Get<ISave>().SaveAndView("Output.pdf", "application/pdf", ms);

                    }
                    DependencyService.Get<IFileLauncher>().Open(pdfcontent, $"Rent---.pdf");
                }
            }
        }
        private async Task GeneratePdfDailyAsync()
        {
            using (UserDialogs.Instance.Loading("Loading"))
            {
                DateTime reuqestedDate = new DateTime(DailyRaports.Year, DailyRaports.Month, DailyRaports.Day, 0, 0, 0, DateTimeKind.Utc);
                var json = JsonConvert.SerializeObject(reuqestedDate);
                App.client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await App.client.PostAsync(App.API_URL_BASE + "rentedcars/pdf/ditore", httpContent);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    UserDialogs.Instance.Alert("Probleme me server, Provoni Perseri", "Error", "Ok");
                }
                else
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var pdfcontent = JsonConvert.DeserializeObject<byte[]>(responseString);
                    using (MemoryStream ms = new MemoryStream(pdfcontent))
                    {
                        //await Xamarin.Forms.DependencyService.Get<ISave>().SaveAndView("Output.pdf", "application/pdf", ms);

                    }
                    DependencyService.Get<IFileLauncher>().Open(pdfcontent, $"Rent---.pdf");
                }
            }
        }
        private async Task GeneratePdfWithFilterAsync()
        {
            using (UserDialogs.Instance.Loading("Loading"))
            {
                FilterRaports.Client = SelectedClient;
                FilterRaports.Car = Cars[CarPosition];
                FilterRaports.RentID = App.instance.DashboardViewModel.CurrentRent.Id;
                var json = JsonConvert.SerializeObject(FilterRaports);
                App.client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await App.client.PostAsync(App.API_URL_BASE + "rentedcars/pdf/filtered", httpContent);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    UserDialogs.Instance.Alert("Probleme me server, Provoni Perseri", "Error", "Ok");
                }
                else
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var pdfcontent = JsonConvert.DeserializeObject<byte[]>(responseString);
                    using (MemoryStream ms = new MemoryStream(pdfcontent))
                    {
                        //await Xamarin.Forms.DependencyService.Get<ISave>().SaveAndView("Output.pdf", "application/pdf", ms);

                    }
                    DependencyService.Get<IFileLauncher>().Open(pdfcontent, $"Rent---.pdf");
                }
            }
        }

        private async Task GeneratePdfWeekly()
        {
            using (UserDialogs.Instance.Loading("Loading"))
            {
                var response = await App.client.GetAsync(App.API_URL_BASE + "rentedcars/pdf/javore");
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    UserDialogs.Instance.Alert("Probleme me server, Provoni Perseri", "Error", "Ok");
                }
                else
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var pdfcontent = JsonConvert.DeserializeObject<byte[]>(responseString);
                    using (MemoryStream ms = new MemoryStream(pdfcontent))
                    {
                        //await Xamarin.Forms.DependencyService.Get<ISave>().SaveAndView("Output.pdf", "application/pdf", ms);

                    }
                    DependencyService.Get<IFileLauncher>().Open(pdfcontent, $"Rent---.pdf");
                }
            }
        }

        private async Task GoToPersonalizedReportAsync()
        {
            using (UserDialogs.Instance.Loading("Loading"))
            {
                PersonalizedReports PersonalizedReports = new PersonalizedReports();
                PersonalizedReports.BindingContext = this;
                FilterRaports = new FilterRaports();
                Clients = App.instance.DashboardViewModel.Clients;
                Cars = App.instance.DashboardViewModel.Cars;
                OnPropertyChanged("Clients");
                OnPropertyChanged("Cars");
                await App.instance.PushAsyncNewPage(PersonalizedReports);
            }
        }

        private async Task GoToCreateSeazonAsync()
        {
            using (UserDialogs.Instance.Loading("Loading"))
            {
                SezonetCreatePage SezonetCreatePage = new SezonetCreatePage();
                SezonetCreatePage.BindingContext = this;
                await App.instance.PushAsyncNewPage(SezonetCreatePage);
            }
        }
        private async Task GoToCreateCmimetAsync()
        {
            using (UserDialogs.Instance.Loading("Loading"))
            {
                CreateACmimetPage CreateACmimetPage = new CreateACmimetPage();
                CreateACmimetPage.BindingContext = this;
                await App.instance.PushAsyncNewPage(CreateACmimetPage);
            }
        }
        private async Task<ObservableCollection<Sezonet>> GetSezonet()
        {
            try
            {
                var response = await App.client.GetAsync(App.API_URL_BASE + "payments/sezonet");
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    UserDialogs.Instance.Alert("Probleme me server, Provoni Perseri", "Error", "Ok");
                    return null;
                }
                else
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var sezonet = JsonConvert.DeserializeObject<ObservableCollection<Sezonet>>(responseString);
                    Sezonet = sezonet;
                    HasSezone = Sezonet.Any();
                    return Sezonet;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
        private async Task CreateASeasonAsync()
        {
            var json = JsonConvert.SerializeObject(SelectedSezoni);
            var g = json.Remove(1, 7);
            App.client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
            HttpContent httpContent = new StringContent(g, Encoding.UTF8, "application/json");
            var response = await App.client.PostAsync(App.API_URL_BASE + "payments/sezonet", httpContent);
            if (response.IsSuccessStatusCode)
            {
                UserDialogs.Instance.Alert("Sezoni u shtua me sukses", "Sukses", "OK");
            }
            else
            {
                UserDialogs.Instance.Alert("Sezoni nuk u shtua me sukses", "Error", "OK");
            }
            var responseString = await response.Content.ReadAsStringAsync();
            Sezonet addedSeason = JsonConvert.DeserializeObject<Sezonet>(responseString);
            Sezonet.Add(addedSeason);
            HasSezone = Sezonet.Any();
            OnPropertyChanged("HasSezone");
        }
        private async Task GoToSezonetAsync()
        {
            using (UserDialogs.Instance.Loading("Loading"))
            {
                SezonetPage SezonetPage = new SezonetPage();
                SezonetPage.BindingContext = this;
                await GetSezonet();
                await App.instance.PushAsyncNewPage(SezonetPage);
            }
        }

        private async Task GoToZbritjetAsync()
        {
            using (UserDialogs.Instance.Loading("Loading"))
            {
                ZbritjetPage ZbritjetPage = new ZbritjetPage();
                ZbritjetPage.BindingContext = this;
                await App.instance.PushAsyncNewPage(ZbritjetPage);
            }
        }

        private async Task GoToTarifatAsync()
        {
            using (UserDialogs.Instance.Loading("Loading"))
            {
                TarifatPage TarifatPage = new TarifatPage();
                TarifatPage.BindingContext = this;
                await App.instance.PushAsyncNewPage(TarifatPage);
            }
        }

        private async Task GoToCmimetAsync()
        {
            using (UserDialogs.Instance.Loading("Loading"))
            {
                CmimetPage CmimetPage = new CmimetPage();
                await GetSezonet();
                await GetCmimet();
                CmimetPage.BindingContext = this;
                await App.instance.PushAsyncNewPage(CmimetPage);
            }
        }

        private async Task<ObservableCollection<Cmimet>> GetCmimet()
        {
            try
            {
                var response = await App.client.GetAsync(App.API_URL_BASE + "payments/cmimet");
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    UserDialogs.Instance.Alert("Probleme me server, Provoni Perseri", "Error", "Ok");
                    return null;
                }
                else
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var cmimet = JsonConvert.DeserializeObject<ObservableCollection<Cmimet>>(responseString);
                    Cmimet = cmimet;
                    HasCmime = Cmimet.Any();
                    OnPropertyChanged("HasCmime");
                    return Cmimet;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
