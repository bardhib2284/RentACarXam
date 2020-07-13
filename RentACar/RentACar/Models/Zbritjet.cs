using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentACarAPI.Models
{
    public class Zbritjet
    {
        public int Id { get; set; }
        public string Emri { get; set; }
        public string CuponCode { get; set; }
        public DiscountTypes DiscountType { get; set; }
        public bool Aktive { get; set; }
        public bool VisibleForDirectBooker { get; set; }




        public enum DiscountTypes { Perqindje = 0, Cmimore = 1}
    }
}
