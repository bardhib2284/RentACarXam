﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Models
{
    public class AuthenticationData
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public int RentID { get; set; }
    }
}