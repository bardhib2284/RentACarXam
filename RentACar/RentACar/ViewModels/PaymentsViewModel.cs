using Acr.UserDialogs;
using Newtonsoft.Json;
using RentACar.Views;
using RentACarAPI.Models;
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
    public class PaymentsViewModel : ViewModelBase
    {
        public ICommand GoToCmimetCommand { get; set; }
        public ICommand GoToTarifatCommand { get; set; }
        public ICommand GoToSezonetCommand { get; set; }
        public ICommand GoToZbritjetCommand { get; set; }
        public ICommand AddASeasonCommand { get; set; }
        public ICommand GoToCreateSeazonCommand { get; set; }
        public Sezonet SelectedSezoni
        {
            get;set;
        }
        public ObservableCollection<Sezonet> Sezonet { get; private set; }
        public bool HasSezone { get; private set; }

        public PaymentsViewModel()
        {
            GoToCmimetCommand = new Command(async () => await GoToCmimetAsync());
            GoToTarifatCommand = new Command(async () => await GoToTarifatAsync());
            GoToZbritjetCommand = new Command(async () => await GoToZbritjetAsync());
            GoToSezonetCommand = new Command(async () => await GoToSezonetAsync());
            AddASeasonCommand = new Command(async () => await CreateASeasonAsync());
            GoToCreateSeazonCommand = new Command(async () => await GoToCreateSeazonAsync());
            SelectedSezoni = new Sezonet();
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
                CmimetPage.BindingContext = this;
                await App.instance.PushAsyncNewPage(CmimetPage);
            }
        }
    }
}
