using RentACar.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private bool _ready;
        public bool Ready
        {
            get { return _ready; }
            set { SetProperty(ref _ready, value); }
        }
        public ObservableCollection<Car> Cars => new ObservableCollection<Car> { SelectedCar };

        public ICommand GoToRentedCarDetailsCommand { get; set; }

        public RentACarViewModel(ObservableCollection<Client> Clients)
        {
            if (Clients == null)
            {

            }
            else
                this.Clients = Clients;

            GoToRentedCarDetailsCommand = new Command(async () => await GoToRentedCarDetailsAsync());
        }

        private Task GoToRentedCarDetailsAsync()
        {
            return Task.FromResult(0);
        }
    }
}
