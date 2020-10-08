using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RentACar.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NavTitleView : ContentView
    {
        public static readonly BindableProperty TitleProperty =
            BindableProperty.Create("Title", typeof(string), typeof(NavTitleView), default(string));

        public static readonly BindableProperty SubTitleProperty =
            BindableProperty.Create("SubTitle", typeof(string), typeof(NavTitleView), default(string));

        public static readonly BindableProperty SearchVisibleProperty =
            BindableProperty.Create("SearchVisible", typeof(bool), typeof(NavTitleView), default(bool));
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public string SubTitle 
        {
            get { return (string)GetValue(SubTitleProperty); }
            set { SetValue(SubTitleProperty, value); }
        }
        public bool SearchVisible
        {
            get { return (bool)GetValue(SearchVisibleProperty); }
            set { SetValue(SearchVisibleProperty, value); }
        }
        public NavTitleView()
        {
            InitializeComponent();
            title.SetBinding(Label.TextProperty, new Binding("Title", source: this));
            subtitle.SetBinding(Label.TextProperty, new Binding("SubTitle", source: this));
        }
    }
}