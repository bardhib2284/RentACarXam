using RentACar.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RentACar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CarDetailsPage : ContentPage
    {
        public CarDetailsPage()
        {
            InitializeComponent();
            BindingContext = App.instance.DashboardViewModel;
            aksidentPicker.ItemsSource = new List<string> { "Jo", "Po" };
            rregullPicker.ItemsSource = new List<string> { "Jo", "Po" };
            aksidentPicker.SelectedIndex = 0;
            rregullPicker.SelectedIndex = 0;

            rregullPicker.SelectedIndexChanged += RregullPicker_SelectedIndexChanged;
            aksidentPicker.SelectedIndexChanged += AksidentPicker_SelectedIndexChanged;
        }

        private void AksidentPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var bc = (DashboardViewModel)BindingContext;
            if (((Picker)sender).SelectedItem != null)
            {
                if(((Picker)sender).SelectedItem.ToString() == "Po")
                    bc.CurrentRentedCar.KaPesuarAksident = true;
            }
            if (((Picker)sender).SelectedItem != null)
            {
                if (((Picker)sender).SelectedItem.ToString() == "Jo")
                    bc.CurrentRentedCar.KaPesuarAksident = false;
            }
        }

        private void RregullPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var bc = (DashboardViewModel)BindingContext;
            if (((Picker)sender).SelectedItem != null)
            {
                if (((Picker)sender).SelectedItem.ToString() == "Po")
                    bc.CurrentRentedCar.KaThyerNdonjeRregull = true;
            }
            if (((Picker)sender).SelectedItem != null)
            {
                if (((Picker)sender).SelectedItem.ToString() == "Jo")
                    bc.CurrentRentedCar.KaThyerNdonjeRregull = false;
            }
        }
    }
}