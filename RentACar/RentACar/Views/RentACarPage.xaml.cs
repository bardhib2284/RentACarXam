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
    public partial class RentACarPage : ContentPage
    {
        public RentACarPage()
        {
            InitializeComponent();
            
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;
            if(e.Item != null)
            {
                var bc = (RentACarViewModel)BindingContext;
                bc.SelectedClient = e.Item as Client;
            }
            if (sender is ListView lv) lv.SelectedItem = null;
        }

        private void search_Focused(object sender, FocusEventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var height = gridrented.Height;
                await MainScroll.ScrollToAsync(0, height, true);
            });
        }
    }
}