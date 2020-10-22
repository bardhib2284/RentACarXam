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
        public DateTime KohaAktualeEKthimit { get; set; }
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
        public decimal CmimiTotal { get; set; }
        [JsonIgnore]
        public Cmimet Cmimi { get; set; }
        public bool? KaPesuarAksident { get; set; }
        public bool? KaThyerNdonjeRregull { get; set; }
        public int KilometrazhiAktual { get; set; }
        public int SezoniID { get; set; }

        public PaymentTypes PaymentType { get; set; }
        public enum PaymentTypes { CASH, POS };

    }
}
