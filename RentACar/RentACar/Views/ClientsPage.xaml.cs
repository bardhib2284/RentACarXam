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
    public partial class ClientsPage : ContentPage
    {
        public ClientsPage()
        {
            InitializeComponent();
        }

        private async void ListView_ItemTappedAsync(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;
            if (e.Item != null)
            {
                var bc = (ClientsViewModel)BindingContext;
                bc.SelectedClient = e.Item as Client;
                await bc.GoToClientDetailsAsync(bc.SelectedClient);
            }
            if (sender is ListView lv) lv.SelectedItem = null;
        }
    }
}