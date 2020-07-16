using Acr.UserDialogs;
using Newtonsoft.Json;
using RentACar.Models;
using RentACar.Views;
using RentACarAPI.Models;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
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
using Xamarin.Forms;

namespace RentACar.ViewModels
{
    public class RentACarViewModel : ViewModelBase
    {
        private Car _selectedCar;
        public Car SelectedCar
        {
            get { return _selectedCar; }
            set { SetProperty(ref _selectedCar, value); }
        }
        public ObservableCollection<Client> Clients { get; set; }
        private Client _selectedClient;
        public Client SelectedClient
        {
            get { return _selectedClient; }
            set 
            { 
                SetProperty(ref _selectedClient, value); 
                if(value != null)
                {
                    Ready = true;
                }
            }
        }

        public Rent CurrentRent => App.instance.DashboardViewModel.CurrentRent;
        private RentedCar _rentedCar;
        public RentedCar RentedCar
        {
            get { return _rentedCar; }
            set { SetProperty(ref _rentedCar, value); }
        }
        private bool _ready;
        public bool Ready
        {
            get { return _ready; }
            set { SetProperty(ref _ready, value); }
        }
        private bool _isTermin;
        public bool IsTermin
        {
            get { return _isTermin; }
            set { SetProperty(ref _isTermin, value); }
        }
        public ObservableCollection<Car> Cars => new ObservableCollection<Car> { SelectedCar };

        public ObservableCollection<Cmimet> Cmimet { get; set; }
        private bool _hasCmime;
        public bool HasCmime
        {
            get { return _hasCmime; }
            set { SetProperty(ref _hasCmime, value); }
        }

        public bool IsSuccessfullRent;
        public ICommand GoToRentedCarDetailsCommand { get; set; }
        public ICommand RentThisCarCommand { get; set; }
        public ICommand GeneratePdfCommand { get; set; }
        public ICommand GoToDashboardCommand { get; set; }

        public RentACarViewModel(ObservableCollection<Client> Clients)
        {
            if (Clients == null)
            {

            }
            else
                this.Clients = Clients;

            GoToRentedCarDetailsCommand = new Command(async () => await GoToRentedCarDetailsAsync());
            RentThisCarCommand = new Command(async () => await RentThisCarAsync());
            RentedCar = new RentedCar();
            RentedCar.KohaELeshimit = DateTime.Today;
            GeneratePdfCommand = new Command(async () => await GeneratePdfAsync());
            GoToDashboardCommand = new Command(async () => await GoToDashboardAsync());
        }

        private async Task GoToDashboardAsync()
        {
            DashboardPage page = new DashboardPage();
            page.BindingContext = App.instance.DashboardViewModel;
            App.instance.DashboardViewModel.PropertyChangedList();
            App.instance.ChangeDetailPage(page);
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
        private async Task<int> GeneratePdfAsync()
        {
            // Create a new PDF document
            PdfDocument document = new PdfDocument();

            //Add a page to the document
            PdfPage page = document.Pages.Add();

            //Create PDF graphics for the page
            PdfGraphics graphics = page.Graphics;

            //Set the standard font
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 22);

            //Draw the text
            graphics.DrawString("Hello World!!!", font, PdfBrushes.Black, new PointF(0, 0));
            graphics.DrawString("Hello awdawdawdawdawdawdawdawdawdghyhwerfgtdhwefewfewfewfewfewfewewfwefewfewfefw!!!", font, PdfBrushes.Black, new PointF(0, 1));

            //Save the document to the stream
            MemoryStream stream = new MemoryStream();
            document.Save(stream);

            //Close the document
            document.Close(true);

            //Save the stream as a file in the device and invoke it for viewing
            await Xamarin.Forms.DependencyService.Get<ISave>().SaveAndView("Output.pdf", "application/pdf", stream);
            return await Task.FromResult(0);
        }

        private async Task RentThisCarAsync()
        {
            using (UserDialogs.Instance.Loading("Loading"))
            {
                RentedCar.IsTermin = IsTermin;
                RentedCar.ClientName = SelectedClient.Name;
                RentedCar.CarName = SelectedCar.Name;
                RentedCar.RentId = App.instance.DashboardViewModel.CurrentRent.Id;
                var json = JsonConvert.SerializeObject(RentedCar);
                var g = json.Remove(1, 7);
                App.client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                HttpContent httpContent = new StringContent(g, Encoding.UTF8, "application/json");
                var response = await App.client.PostAsync(App.API_URL_BASE + "rentedcars", httpContent);
                if (response.IsSuccessStatusCode)
                {
                    UserDialogs.Instance.Alert("Vetura u shtua me sukses", "Sukses", "OK");
                }
                else
                {
                    UserDialogs.Instance.Alert("Vetura nuk u shtua me sukses", "Error", "OK");
                }
                var responseString = await response.Content.ReadAsStringAsync();
                RentedCar addedCar = JsonConvert.DeserializeObject<RentedCar>(responseString);
                IsSuccessfullRent = addedCar != null ? true : false;
                var page = new PostRentedCarPage();
                page.BindingContext = this;
                App.instance.DashboardViewModel.LatestTransactions?.Add(addedCar);
                var car = App.instance.DashboardViewModel.AvailableCars.FirstOrDefault(x => x.Id == addedCar.CarId);
                car.Statusi = Car.StatusTypes.Zene;
                App.instance.DashboardViewModel.AvailableCars.Remove(car);
                if (App.instance.DashboardViewModel.TakenCars.Count == 0)
                    App.instance.DashboardViewModel.TakenCars = new ObservableCollection<Car>();
                App.instance.DashboardViewModel.TakenCars.Add(car);
                if (App.instance.DashboardViewModel.LatestTransactions.Count > 5)
                {
                    App.instance.DashboardViewModel.LatestTransactions.RemoveAt(0);
                }
                App.instance.DashboardViewModel.RentedCarsByRentId?.Add(addedCar);
                App.instance.DashboardViewModel.PropertyChangedList();
                App.instance.DashboardViewModel.LatestTransactionsOnGoing.Add(addedCar);
                await Task.Delay(1000);
                App.instance.ChangeDetailPage(page);
            }
        }

        private async Task GoToRentedCarDetailsAsync()
        {
            RentedCar.CarId = SelectedCar.Id;
            RentedCar.ClientId = SelectedClient.Id;
            RentACarSecondStepPage rentedCarPage2 = new RentACarSecondStepPage();
            rentedCarPage2.BindingContext = this;
            Cmimet = await GetCmimet();
            OnPropertyChanged("Cmimet");
            await App.instance.PushAsyncNewPage(rentedCarPage2);
        }
    }
}
