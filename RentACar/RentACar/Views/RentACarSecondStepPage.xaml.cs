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
    public partial class RentACarSecondStepPage : ContentPage
    {
        public RentACarSecondStepPage()
        {
            InitializeComponent();
           
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var bc = (RentACarViewModel)BindingContext;
            var itemSource = bc.Cmimet;
            var Cmimi = new RentACarAPI.Models.Cmimet { Dega = "Zgjedh Cmim Individual" };
            itemSource.Add(Cmimi);
            var firstItem = itemSource.First();
            if(firstItem == Cmimi)
            {
                cmimiPicker.ItemsSource = itemSource;
                return;
            }
            else
            {
                itemSource[0] = Cmimi;
                itemSource[itemSource.Count-1] = firstItem;
                cmimiPicker.ItemsSource = itemSource;
                return;
            }
        }
    }
}