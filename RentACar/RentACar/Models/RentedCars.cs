using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Models
{
    public class RentedCar
    {
        public int Id { get; set; }
        public DateTime KohaELeshimit { get; set; }
        public DateTime KohaEKthimit { get; set; }
        public int CarId { get; set; }
        public int ClientId { get; set; }
        public decimal CmimiDitor { get; set; }
        public int DitetELeshimit { get; set; }
        public decimal PerqindjaEVoneses { get; set; }
        public bool IsTermin { get; set; }
    }
}
