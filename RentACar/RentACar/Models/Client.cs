using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace RentACar.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Nationality { get; set; }
        public string Signature { get; set; }
        public string Gender { get; set; }
        public DateTime ID_ExpiryDate { get; set; }
        public DateTime ID_StartDate { get; set; }
        public string ID_GivenBy { get; set; }
        public int ID_PersonalNumber { get; set; }
        public int ID_CardNumber { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string WorkPlace { get; set; }
        public int RentId { get; set; }
        public byte[] ClientImageFront { get; set; }
        public byte[] ClientImageBack { get; set; }
        public bool IsUnwanted { get; set; }
        [JsonIgnore]
        private MemoryStream ms => new MemoryStream(ClientImageFront);
        [JsonIgnore]
        private MemoryStream msBack => new MemoryStream(ClientImageBack);
        [JsonIgnore]
        private MemoryStream msLicense => new MemoryStream(ClientPatentShofer);

        public byte[] ClientPatentShofer { get; set; }
        public int PhoneNumber { get; set; }
        [JsonIgnore]
        public ImageSource ClientFrontImage
        {
            get
            {

                if (ClientImageFront != null)
                {
                    var image = ImageSource.FromStream(() => ms);
                    return image;
                }
                else
                {
                    return ImageSource.FromFile("Passengers.png");
                }
                
            }
        }
        [JsonIgnore]
        public ImageSource ClientFrontBack
        {
            get
            {

                if (ClientImageFront != null)
                {
                    var image = ImageSource.FromStream(() => msBack);
                    return image;
                }
                else
                {
                    return ImageSource.FromFile("Passengers.png");
                }

            }
        }
        [JsonIgnore]
        public ImageSource ClientCarLicense
        {
            get
            {

                if (ClientImageFront != null)
                {
                    var image = ImageSource.FromStream(() => msLicense);
                    return image;
                }
                else
                {
                    return ImageSource.FromFile("Passengers.png");
                }

            }
        }

    }
}
