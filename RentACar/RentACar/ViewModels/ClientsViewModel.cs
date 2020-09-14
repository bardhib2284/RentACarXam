using Acr.UserDialogs;
using Newtonsoft.Json;
using RentACar.Models;
using RentACar.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using MenuItem = RentACar.Models.MenuItem;

namespace RentACar.ViewModels
{
    public class ClientsViewModel : ViewModelBase
    {
        private ObservableCollection<Client> _clients;
        public ObservableCollection<Client> Clients
        {
            get { return _clients; }
            set { SetProperty(ref _clients, value); }
        }
        private ObservableCollection<Client> _unwantedClients;
        public ObservableCollection<Client> UnwantedClients
        {
            get { return _unwantedClients; }
            set { SetProperty(ref _unwantedClients, value); }
        }
        public ObservableCollection<RentedCar> ClientsTransactions { get; set; }
        private bool _hasUnwantedClients;
        public bool HasUnwantedClients
        {
            get { return _hasUnwantedClients; }
            set { SetProperty(ref _hasUnwantedClients, value); }
        }

        private bool _hasClients;
        public bool HasClients
        {
            get { return _hasClients; }
            set { SetProperty(ref _hasClients, value); }
        }

        private Client _SelectedClient;
        public Client SelectedClient
        {
            get { return _SelectedClient; }
            set { SetProperty(ref _SelectedClient, value); }
        }

        protected int tabPosition = 0;
        public int TabPosition
        {
            get { return tabPosition; }
            set { SetProperty(ref tabPosition, value); }
        }
        private bool isEditing;
        public bool IsEditing
        {
            get { return isEditing; }
            set { SetProperty(ref isEditing, value); }
        }
        public virtual List<MenuItem> ProjectionItems { get; protected set; }

        public ICommand GoToCreateClientCommand { get; set; }
        public ICommand CreateAClientCommand { get; set; }
        public ICommand GoToClientsPageCommand { get; set; }
        public ICommand GoToClientsUnwantedPageCommand { get; set; }
        public ICommand GoToClientDetailsCommand { get; set; }
        public ICommand DeleteClientCommand { get; set; }
        public ICommand EditClientCommand { get; set; }
        public ClientsViewModel()
        {
            Clients = new ObservableCollection<Client>();
            GoToCreateClientCommand = new Command(async () => await GoToCreateClientAsync());
            CreateAClientCommand = new Command(async () => await CreateAClientAsync());
            GoToClientsPageCommand = new Command(async () => await GoToClientsPageAsync());
            GoToClientsUnwantedPageCommand = new Command(async () => await GoToClientsUnwantedPageAsync());
            GoToClientDetailsCommand = new Command(async (c) => await GoToClientDetailsAsync((c as Client)));
            DeleteClientCommand = new Command(async () => await DeleteClientAsync());
            EditClientCommand = new Command(async () => await EditClientAsync());
            var projectionItems = new List<MenuItem>() {
                new MenuItem(){Name="transactions",TitleKey="Detajet e klientit", Parametar="all"},
                new MenuItem(){Name="transactions",TitleKey="Transaksionet", Parametar="finished"},
            };
            ProjectionItems = projectionItems;
            ClientsTransactions = new ObservableCollection<RentedCar>();
        }

        private async Task EditClientAsync()
        {
            IsEditing = true;
            CreateClientPage CreateAClientPage = new CreateClientPage();
            CreateAClientPage.BindingContext = this;
            (App.instance.MainPage as MainPage).IsPresented = false;
            await App.instance.PushAsyncNewPage(CreateAClientPage);
        }

        private async Task DeleteClientAsync()
        {
            using (UserDialogs.Instance.Loading("Loading"))
            {
                SelectedClient.RentId = App.instance.DashboardViewModel.CurrentRent.Id;
                var json = JsonConvert.SerializeObject(SelectedClient);
                App.client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await App.client.DeleteAsync(App.API_URL_BASE + "rents/clients/delete/"+SelectedClient.RentId + "/" + SelectedClient.Id);
                if (response.IsSuccessStatusCode)
                {
                    UserDialogs.Instance.Alert("Clienti u fshi me sukses", "Sukses", "OK");
                    Clients.Remove(SelectedClient);
                    SelectedClient = new Client();
                    HasClients = Clients.Any();
                    await App.instance.GoToPreviousPage();
                }
                else
                {
                    UserDialogs.Instance.Alert("Clienti nuk u fshi me sukses", "Error", "OK");
                    return;
                }
            }
        }

        public async Task GoToClientDetailsAsync(Client c)
        {
            using (UserDialogs.Instance.Loading("Loading"))
            {
                ClientDetailPage ClientsPage = new ClientDetailPage();
                ClientsPage.BindingContext = this;
                SelectedClient = c;
                ClientsTransactions = new ObservableCollection<RentedCar>();
                if (App.instance.DashboardViewModel.HasLatestTransactions)
                {
                    var list = App.instance.DashboardViewModel.RentedCarsByRentId.OrderByDescending(i => i.KohaELeshimit);
                    foreach (var transaction in list)
                    {
                        if (transaction.ClientId == SelectedClient.Id)
                        {
                            ClientsTransactions.Add(transaction);
                        }
                    }
                }
                OnPropertyChanged("ClientsTransactions");
                await Task.Delay(1000);
                await App.instance.PushAsyncNewPage(ClientsPage);
            }
        }

        private async Task GoToClientsPageAsync()
        {
            using (UserDialogs.Instance.Loading("Loading"))
            {
                ClientsPage ClientsPage = new ClientsPage();
                App.instance.ClientsViewModel = new ClientsViewModel();
                ClientsPage.BindingContext = App.instance.ClientsViewModel;
                Clients = Clients.Any() ? Clients : await App.instance.ClientsViewModel.LoadClientsFromRent(App.instance.DashboardViewModel.CurrentRent);
                (App.instance.MainPage as MainPage).IsPresented = false;
                await App.instance.PushAsyncNewPage(ClientsPage);
            }
        }

        private async Task GoToClientsUnwantedPageAsync()
        {
            using (UserDialogs.Instance.Loading("Loading"))
            {
                ClientsUnwantedPage ClientsPage = new ClientsUnwantedPage();
                App.instance.ClientsViewModel = new ClientsViewModel();
                ClientsPage.BindingContext = App.instance.ClientsViewModel;
                Clients = Clients.Any() ? Clients : await App.instance.ClientsViewModel.LoadClientsFromRent(App.instance.DashboardViewModel.CurrentRent);
                foreach (var item in Clients)
                {
                    UnwantedClients = new ObservableCollection<Client>();
                    if (item.IsUnwanted)
                    {
                        UnwantedClients.Add(item);
                    }
                }
                HasUnwantedClients = UnwantedClients.Any();
                (App.instance.MainPage as MainPage).IsPresented = false;
                await App.instance.PushAsyncNewPage(ClientsPage);
            }
        }

        private async Task CreateAClientAsync()
        {
            if(IsEditing)
            {
                using (UserDialogs.Instance.Loading("Loading"))
                {
                    SelectedClient.RentId = App.instance.DashboardViewModel.CurrentRent.Id;
                    SelectedClient.Signature = ".";
                    var json = JsonConvert.SerializeObject(SelectedClient);
                    App.client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                    HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await App.client.PutAsync(App.API_URL_BASE + "rents/clients/" + SelectedClient.RentId, httpContent);
                    if (response.IsSuccessStatusCode)
                    {
                        Clients.Remove(SelectedClient);
                        UserDialogs.Instance.Alert("Clienti u editua me sukses", "Sukses", "OK");
                    }
                    else
                    {
                        UserDialogs.Instance.Alert("Clienti nuk u editua me sukses", "Error", "OK");
                    }
                    var responseString = await response.Content.ReadAsStringAsync();
                    Client addedClient = JsonConvert.DeserializeObject<Client>(responseString);
                    SelectedClient = addedClient;
                    Clients.Add(SelectedClient);
                    HasClients = true;
                }
                return;
            }
            using (UserDialogs.Instance.Loading("Loading"))
            {
                SelectedClient.RentId = App.instance.DashboardViewModel.CurrentRent.Id;
                SelectedClient.Signature = ".";
                var json = JsonConvert.SerializeObject(SelectedClient);
                var g = json.Remove(1, 7);
                App.client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                HttpContent httpContent = new StringContent(g, Encoding.UTF8, "application/json");
                var response = await App.client.PostAsync(App.API_URL_BASE + "rents/clients", httpContent);
                if (response.IsSuccessStatusCode)
                {
                    UserDialogs.Instance.Alert("Clienti u shtua me sukses", "Sukses", "OK");
                }
                else
                {
                    UserDialogs.Instance.Alert("Clienti nuk u shtua me sukses", "Error", "OK");
                }
                var responseString = await response.Content.ReadAsStringAsync();
                Client addedClient = JsonConvert.DeserializeObject<Client>(responseString);
                Clients.Add(addedClient);
                HasClients = Clients.Any();
            }
            
        }

        private async Task GoToCreateClientAsync()
        {
            SelectedClient = new Client();
            IsEditing = false;
            CreateClientPage CreateAClientPage = new CreateClientPage();
            CreateAClientPage.BindingContext = this;
            (App.instance.MainPage as MainPage).IsPresented = false;
            await App.instance.PushAsyncNewPage(CreateAClientPage);
        }

        public async Task<ObservableCollection<Client>> LoadClientsFromRent(Rent currentRent)
        {
            try
            {
                var response = await App.client.GetAsync(App.API_URL_BASE + "rents/clients/" + currentRent.Id);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    UserDialogs.Instance.Alert("Probleme me server, Provoni Perseri", "Error", "Ok");
                    return null;
                }
                else
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var clients = JsonConvert.DeserializeObject<ObservableCollection<Client>>(responseString);
                    Clients = clients;
                    HasClients = Clients.Any();
                    return Clients;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
