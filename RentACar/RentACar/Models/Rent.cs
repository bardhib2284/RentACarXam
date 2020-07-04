using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Models
{
    public class Rent
    {
        public int Id { get; set; }
        public string Rent_Name { get; set; }
        public string Location { get; set; }

        public virtual IEnumerable<Car> Cars { get; set; }
        public virtual IEnumerable<Client> Clients { get; set; }
    }
}
