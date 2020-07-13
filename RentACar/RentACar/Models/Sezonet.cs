using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentACarAPI.Models
{
    public class Sezonet
    {
        public int id { get; set; }
        public string Emri { get; set; }
        public DateTime DataEFillimit { get; set; }
        public DateTime DataEMbarimit { get; set; }
        public int MinRentalPeriod { get; set; }
        public int MaxRentalPeriod { get; set; }
        public int MinRentalHours { get; set; }
        public int MaxRentalHours { get; set; }
    }
}
