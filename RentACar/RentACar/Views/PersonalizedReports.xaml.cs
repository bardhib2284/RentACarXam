using RentACar.Models;
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
    public partial class PersonalizedReports : ContentPage
    {
        public PersonalizedReports()
        {
            InitializeComponent();
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;
            if (e.Item != null)
            {
                var bc = (PaymentsViewModel)BindingContext;
                bc.SelectedClient = e.Item as Client;
            }
            if (sender is ListView lv) lv.SelectedItem = null;
        }
    }
}