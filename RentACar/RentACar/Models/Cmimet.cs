using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentACarAPI.Models
{
    public class Cmimet
    {
        public int ID { get; set; }
        public string Dega { get; set; }
        public int SeasonID { get; set; }
        public int VehicleClass { get; set; }
        public int TransmissionType { get; set; }
        public decimal CmimiPerOre { get; set; }
        public decimal CmimiPerDite { get; set; }
        public decimal CmimiPerJave { get; set; }
        public bool KaZbritjeNeBazeTeDiteve { get; set; }
        public bool KaZbritjeNeBazeTeOreve { get; set; }
    }
}
