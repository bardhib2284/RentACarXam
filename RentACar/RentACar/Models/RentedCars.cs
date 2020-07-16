using Newtonsoft.Json;
using RentACarAPI.Models;
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
        public int RentId { get; set; }
        public decimal CmimiDitor { get; set; }
        public int DitetELeshimit { get; set; }
        public decimal PerqindjaEVoneses { get; set; }
        public bool IsTermin { get; set; }
        public bool IsFinished { get; set; }
        public string CarName { get; set; }
        public string ClientName { get; set; }
        public int CmimiID { get; set; }

        [JsonIgnore]
        public decimal CmimiTotal { get; set; }
        [JsonIgnore]
        public Cmimet Cmimi { get; set; }
    }
}
