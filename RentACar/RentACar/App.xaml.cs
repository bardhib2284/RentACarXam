using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using RentACar.Services;
using RentACar.Views;
using RentACar.ViewModels;
using Plugin.Connectivity;
using Acr.UserDialogs;
using System.Threading.Tasks;
using System.Net.Http;

namespace RentACar
{
    public partial class App : Application
    {
        public static App instance;

        public DashboardViewModel DashboardViewModel;
        public ClientsViewModel ClientsViewModel;
        public static readonly HttpClient client = new HttpClient();
        public const string API_URL_BASE = "https://rentacarapi.conveyor.cloud/api/";
        public Page CurrentPage;

        public App()
        {
            if (instance != null)
            {
                if (instance != this)
                {
                    instance = this;
                }
            }
            else
                instance = this;
            InitializeComponent();
            DashboardViewModel = new DashboardViewModel();
            DependencyService.Register<MockDataStore>();
            MainPage mainPage = new MainPage();
            (mainPage as MasterDetailPage).Detail = new NavigationPage(new LoginPage());
            MainPage = mainPage;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
        public void ChangeDetailPage(Page page = null, NavigationPage navigationPage = null)
        {
            if (page == null && navigationPage == null)
            {
                return;
            }
            var masterPage = (MainPage as MasterDetailPage);
            var currentPage = masterPage.Detail.Navigation.NavigationStack[0];
            if (page != null)
            {
                if (currentPage.GetType() == page.GetType())
                {
                    masterPage.IsPresented = false;
                    return;
                }
            }
            if (masterPage.IsPresented == true)
            {
                masterPage.IsPresented = false;
            }
            if (navigationPage != null)
            {
                masterPage.Detail = navigationPage;
                return;
            }
            masterPage.Detail = new NavigationPage(page)
            {
                BarTextColor = Color.FromHex("#2CC6E1"),
                BarBackgroundColor = Color.White,
            };
        }

        public async Task PushAsyncNewPage(Page page)
        {
            await (MainPage as MasterDetailPage).Detail.Navigation.PushAsync(page);
        }

        public void ChangeMainPage(Page page)
        {
            if (page is MasterDetailPage)
            {
                var p = new MainPage()
                {
                    Detail = new NavigationPage(new DashboardPage())
                    {
                        BarTextColor = Color.FromHex("#2CC6E1"),
                        BarBackgroundColor = Color.White,
                    }
                };
                MainPage = p;
                CurrentPage = p;
            }
            else
            {
                MainPage = page;
                CurrentPage = page;
            }
        }

        public bool DoIHaveInternet()
        {
            var connected = CrossConnectivity.Current.IsConnected;
            if (connected) return true;
            else
                UserDialogs.Instance.Alert("Ju lutem kontrolloni interrnetin", "Error", "Ok");
            return false;
        }

        public bool DoBack
        {
            get
            {
                MasterDetailPage mainPage = App.Current.MainPage as MasterDetailPage;

                if (mainPage != null)
                {
                    bool canDoBack = mainPage.Detail.Navigation.NavigationStack.Count > 1 || mainPage.IsPresented;

                    // we are on a top level page and the Master menu is NOT showing
                    if (!canDoBack)
                    {
                        // don't exit the app just show the Master menu page
                        mainPage.IsPresented = true;
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                return true;
            }
        }

        public async Task GoToPreviousPage()
        {
            await (MainPage as MasterDetailPage).Detail.Navigation.PopAsync();
        }
    }
}
