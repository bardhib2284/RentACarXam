using RentACar.ViewModels;
using RentACarAPI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RentACar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DailyReportsPage : ContentPage
    {
        public DailyReportsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var bc = (PaymentsViewModel)BindingContext;
            if(bc.IsSeason)
            {
                ObservableCollection<string> EmratESezoneve = new ObservableCollection<string>();
                foreach(var sezoni in bc.Sezonet)
                {
                    EmratESezoneve.Add(sezoni.Emri);
                }
                seasonsPicker.ItemsSource = EmratESezoneve;
            }
        }
    }
}