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
    public partial class SezonetCreatePage : ContentPage
    {
        public SezonetCreatePage()
        {
            InitializeComponent();
            dataefillimit.MinimumDate = new DateTime(DateTime.Now.Year - 1, DateTime.Now.Month, DateTime.Now.Day);
            dataembarimit.MinimumDate = new DateTime(DateTime.Now.Year - 1, DateTime.Now.Month, DateTime.Now.Day);
            minPeriodDite.ItemsSource = new List<int> { 1, 2, 3, 4, 5 };
            maxPeriodDite.ItemsSource = new List<int> { 10, 15, 20 };
            minPeriodOre.ItemsSource = new List<int> { 12, 24 };
            maxPeriodOre.ItemsSource = new List<int> { 36, 48 };
        }
    }
}