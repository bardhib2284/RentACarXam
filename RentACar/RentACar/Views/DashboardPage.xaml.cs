using RentACar.Models;
using RentACar.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RentACar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DashboardPage : ContentPage
    {
        public DashboardPage()
        {
            InitializeComponent();
            BindingContext = App.instance.DashboardViewModel;
            
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            VeturatNeKerkim.IsVisible = true;
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                VeturatNeKerkim.IsVisible = false;
                VeturatELira.IsVisible = true;
                VeturatEParegjistruara.IsVisible = true;
                VeturatEZena.IsVisible = true;
                VeturatNeServis.IsVisible = true;
                VeturatNeKerkimLabel.IsVisible = false;
                return;
            }
            VeturatELira.IsVisible = false;
            VeturatEParegjistruara.IsVisible = false;
            VeturatEZena.IsVisible = false;
            VeturatNeServis.IsVisible = false;
            VeturatNeKerkimLabel.IsVisible = false;
            var bc = (DashboardViewModel)BindingContext;
            var ListOfCars = bc.Cars.Where(x => x.Name.Contains(e.NewTextValue));
            bc.SearchedCars.Clear();
            ObservableCollection<Car> searchedCars = new ObservableCollection<Car>();
            foreach (var car in ListOfCars)
            {
                searchedCars.Add(car);
            }
            VeturatNeKerkimItemSource.ItemsSource = searchedCars.ToArray();
            bc.SearchedCars = searchedCars;
            OnPropertyChanged("BindingContext.SearchedCars");
            if (!bc.SearchedCars.Any())
            {
                VeturatNeKerkimLabel.IsVisible = true;
            }
            else
                bc.HasSearchedCars = true;
        }
    }
}