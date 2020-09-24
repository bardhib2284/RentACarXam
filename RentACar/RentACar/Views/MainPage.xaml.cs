using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RentACar.Models;

namespace RentACar.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : MasterDetailPage
    {
        Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();
        public MainPage()
        {
            InitializeComponent();

            MasterBehavior = MasterBehavior.Popover;
            MenuPages.Add((int)MenuItemType.Browse, (NavigationPage)Detail);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Detail.Unfocused += Detail_Unfocused;
        }

        private void Detail_Unfocused(object sender, FocusEventArgs e)
        {
            foreach (var child in MenuPageForExpande.Content.FindByName<StackLayout>("Holder").Children)
            {
                if (child is Expander expand)
                {
                    if (expand.IsExpanded)
                    {
                        expand.State = ExpanderState.Collapsed;
                    }
                }
            }
        }

        public async Task NavigateFromMenu(int id)
        {
            
        }
    }
}