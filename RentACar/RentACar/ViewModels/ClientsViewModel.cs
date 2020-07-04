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
        public ICommand GoToCreateClientCommand { get; set; }
        public ICommand CreateAClientCommand { get; set; }
        public ClientsViewModel()
        {
            Clients = new ObservableCollection<Client>();
            GoToCreateClientCommand = new Command(async () => await GoToCreateClientAsync());
            CreateAClientCommand = new Command(async () => await CreateAClientAsync());
        }

        private async Task CreateAClientAsync()
        {
            SelectedClient.RentId = App.instance.DashboardViewModel.CurrentRent.Id;
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

        private async Task GoToCreateClientAsync()
        {
            SelectedClient = new Client();
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
