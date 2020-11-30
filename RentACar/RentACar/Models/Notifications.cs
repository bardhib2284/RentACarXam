using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Models
{
    public class Notifications
    {
        public int ID { get; set; }
        public int RentID { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime DateReceived { get; set; }
        public DateTime DateRead { get; set; }
        public bool IsRead { get; set; }
        public bool HasInteraction { get; set; }
        public bool IsImportant { get; set; }
    }
}