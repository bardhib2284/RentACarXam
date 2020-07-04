using Newtonsoft.Json;
using RentACar.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace RentACar.Models
{
    public class Car : ViewModelBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public DateTime ProductionYear { get; set; }
        public int NumberOfSeats { get; set; }
        public string Fuel { get; set; }
        public string Color { get; set; }
        public TransmissionTypes Transmission { get; set; }
        public int Km { get; set; }
        public DateTime DateOfRegister { get; set; }
        public DateTime DateOfStartCheck { get; set; }
        public DateTime NextDateOfCheck { get; set; }
        public StatusTypes Statusi { get; set; }
        public int RentId { get; set; }
        public byte[] CarImage_1 { get; set; }
        public byte[] CarImage_2 { get; set; }
        public string CarImageUrl { get; set; }
        public enum TransmissionTypes { Automatic = 0, Manual = 1 }
        public enum StatusTypes { Aktiv = 0, Zene = 1, Pa_Regjistruar = 2, NeServis = 3 }

        //EXTRAS
        private MemoryStream ms => new MemoryStream(CarImage_1);
        [JsonIgnore]
        private ImageSource _CarImageSource;
        [JsonIgnore]
        public ImageSource CarImageSource
        {
            get
            {
                if(CarImageSourceSaved != null)
                {
                    return CarImageSourceSaved;
                }
                if(string.IsNullOrEmpty(CarImageUrl))
                {
                    if (CarImage_1 != null)
                    {
                        var image = ImageSource.FromStream(() => ms);
                        CarImageSourceSaved = image;
                        return image;
                    }
                    else
                    {
                        return ImageSource.FromFile("Audi.png");
                    }
                }
                else
                {
                    return ImageSource.FromUri(new Uri(CarImageUrl));
                }
                
            }
            set { SetProperty(ref _CarImageSource, value); }
        }
        [JsonIgnore]
        public ImageSource CarImageSourceSaved;
        [JsonIgnore]
        public bool ImageChoosen { get; set; }
    }
}
