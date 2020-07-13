using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;

namespace RentACarAPI.Models
{
    public class TarifatShtese
    {
        public int Id { get; set; }
        public int Emri { get; set; }
        public ChargeTypes ChargeType { get; set; }
        public SelectionTypes SelectionType { get; set; }
        public enum ChargeTypes { Sasi=0,Ditore=1,Javore=2 }
        public decimal Vlera { get; set; }
        public bool TarifaESigurimit { get; set; }
        public bool EZgjedhurByDefault { get; set; }
        public enum SelectionTypes { Nje = 0,Disa = 1}
    }
}
