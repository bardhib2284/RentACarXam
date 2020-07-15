using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Models
{
    public class FilterRaports
    {
        public int RentID { get; set; }
        public Client Client { get; set; }
        public Car Car { get; set; }
        public DateTime KohaEFillimit { get; set; }
        public DateTime KohaEMbarimit { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
    }
}