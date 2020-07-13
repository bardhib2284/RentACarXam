using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentACarAPI.Models
{
    public class DitetEBllokuara
    {
        public int Id { get; set; }
        public DateTime DitaEStartimit { get; set; }
        public bool AllDays { get; set; }
        public string Koha { get; set; }
        public DateTime KohaEStartimit { get; set; }
        public DateTime KohaEMbarimit { get; set; }
        public bool Perserit { get; set; }
    }
}
