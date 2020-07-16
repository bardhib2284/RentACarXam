using RentACar.ViewModels;
using RentACarAPI.Models;
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
    public partial class CreateACmimetPage : ContentPage
    {
        public CreateACmimetPage()
        {
            InitializeComponent();
            klasaEVetures.ItemsSource = new List<string> { "Ekonomik", "Luksoz" };
        }

        private void dataefillimit_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var bc = (PaymentsViewModel)BindingContext;
            List<string> sezonets = new List<string>();
            foreach(var sezon in bc.Sezonet)
            {
                sezonets.Add(sezon.Emri);
            }
            dataefillimit.ItemsSource = sezonets;
        }
    }
}