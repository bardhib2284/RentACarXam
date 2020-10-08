using System;
namespace RentACar.Models
{
    public class KontrataObject
    {
        public int clientId { get;set;}
        public int carId { get; set; }
        public int rentedCarId { get; set; }
        public Client client { get; set; }
        public Car car { get; set; }
        public Rent rent { get; set; }
        public RentedCar rentedCar { get; set; }
    }
}
