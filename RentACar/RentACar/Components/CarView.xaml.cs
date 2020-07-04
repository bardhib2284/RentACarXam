using CarouselView.FormsPlugin.Abstractions;
using FFImageLoading.Forms;
using RentACar.Converters;
using RentACar.Models;
using RentACar.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RentACar.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CarView : ContentView
    {
        public List<int> MyItemsSource = new List<int> {1,2,3,4,5};
        public static readonly BindableProperty CarProperty =
            BindableProperty.Create("Car", typeof(Car), typeof(CarView), default(Car));

        public Car Car
        {
            get { return (Car)GetValue(CarProperty); }
            set { SetValue(CarProperty, value); }
        }

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create("Command", typeof(Command), typeof(DashboardPage), default(Command));

        public ICommand Command
        {
            get { return (Command)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create("CommandParameter", typeof(Object), typeof(DashboardPage), null);

        public Object CommandParameter
        {
            get { return (Object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public event EventHandler Clicked;

        public CarView()
        {
            InitializeComponent();
            CarBrand.SetBinding(Image.SourceProperty, new Binding("Car.Brand", source: this));
            //if(Car.CarImage_1 != null || Car.CarImage_1.Any())
            //{
            //    var stream1 = new MemoryStream(Car.CarImage_1 ?? new byte[] { });
            //    CarImage.Source = ImageSource.FromStream(() => stream1);
            //}
            CarImage.SetBinding(CachedImage.SourceProperty, new Binding("Car.CarImageSource", source: this));
            CarName.SetBinding(Label.TextProperty, new Binding("Car.Name", source: this));
            //carouselForCarImages.SetBinding(CarouselViewControl.ItemsSourceProperty, new Binding("Car.Images", source: this));
            Available.SetBinding(Button.TextProperty, new Binding("Car.Available", BindingMode.Default, new FromBoolToAvailableText(),source:this));
            Available.SetBinding(Button.OpacityProperty, new Binding("Car.Available", BindingMode.Default, new FromBoolToAvailableOpacity(), source: this));
            Available.SetBinding(Button.BackgroundColorProperty, new Binding("Car.Available", BindingMode.Default, new FromBoolToAvailableBackgroundColor(), source: this));
            this.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => {
                    this.Clicked?.Invoke(this, EventArgs.Empty);

                    if (Command != null)
					{
						if(Command.CanExecute(CommandParameter))
						{
							Command.Execute(CommandParameter);
						}
					}
                })
            });
        }

        private void carousel_PositionSelected(object sender, CarouselView.FormsPlugin.Abstractions.PositionSelectedEventArgs e)
        {

        }

        private void carousel_Scrolled(object sender, CarouselView.FormsPlugin.Abstractions.ScrolledEventArgs e)
        {

        }

        private async void button_Clicked(object sender, EventArgs e)
        {
            if (CommandParameter != null)
            {
                await App.instance.DashboardViewModel.CarDetailsAsync(CommandParameter as Car);
            }
        }
    }
}