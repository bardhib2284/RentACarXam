using RentACar.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using RentACar.Views;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;

namespace RentACar.ViewModels
{
    public class MessagesViewModel : ViewModelBase
    {
        public ObservableCollection<Notifications> Notifications { get; set; }

        protected int tabPosition = 0;
        public int TabPosition
        {
            get { return tabPosition; }
            set { SetProperty(ref tabPosition, value); }
        }
        public virtual List<Models.MenuItem> ProjectionItems { get; protected set; }

        private bool _hasMessages;
        public bool HasMessages
        {
            get { return _hasMessages; }
            set { SetProperty(ref _hasMessages, value); }
        }

        public ICommand GoToMessagesDetailsCommand { get; set; }

        public Notifications _notification;
        public Notifications SelectedMessage
        {
            get { return _notification; }
            set { SetProperty(ref _notification, value); }
        }
        public MessagesViewModel()
        {
            GoToMessagesDetailsCommand = new Command<Notifications>(async (c) => await GoToMessagesDetailsAsync(c));
            var projectionItems = new List<Models.MenuItem>() {
                    new Models.MenuItem(){Name="transactions",TitleKey="Te gjitha", Parametar="all"},
                    new Models.MenuItem(){Name="transactions",TitleKey="Te Lexuara", Parametar="read"},
                    new Models.MenuItem(){Name="transactions",TitleKey="Te Palexuara", Parametar="unread"},
            };
            Notifications = new ObservableCollection<Notifications>();
            ProjectionItems = projectionItems;
            HasMessages = Notifications.Count > 0;
        }

        private async System.Threading.Tasks.Task GoToMessagesDetailsAsync(object obj)
        {
            if(obj is Notifications message)
            {
                message.IsRead = true;

                var serializedNotification = JsonConvert.SerializeObject(message);
                HttpContent httpContentNotification = new StringContent(serializedNotification, Encoding.UTF8, "application/json");
                var response = await App.client.PutAsync(App.API_URL_BASE + "rents/notifications/update",httpContentNotification);
                var result = await response.Content.ReadAsStringAsync();
                var indexRemoved = Notifications.IndexOf(message);
                Notifications.Remove(message);
                message = JsonConvert.DeserializeObject<Notifications>(result);
                Notifications.Insert(indexRemoved, message);
                SelectedMessage = JsonConvert.DeserializeObject<Notifications>(result);
                MessagesDetailPage MessagesDetailPage = new MessagesDetailPage
                {
                    BindingContext = this
                };
                await App.instance.PushAsyncNewPage(MessagesDetailPage);
            }

            //throw new NotImplementedException();
        }

        public async Task GetMessages()
        {
            using(UserDialogs.Instance.Loading("Duke ngarkuar mesazhet"))
            {
                var response = await App.client.GetAsync(App.API_URL_BASE + "rents/notifications/" + App.instance.DashboardViewModel.CurrentRent.Id);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    UserDialogs.Instance.Alert("Probleme me server, Provoni Perseri", "Error", "Ok");
                }
                else
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var gNotifications = JsonConvert.DeserializeObject<ObservableCollection<Notifications>>(result);
                    Notifications = new ObservableCollection<Notifications>(gNotifications.OrderByDescending(x => x.DateReceived));
                    OnPropertyChanged("Notifications");
                    HasMessages = Notifications.Count > 0;
                }
            }
        }
    }
}
