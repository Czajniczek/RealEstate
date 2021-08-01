using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstate.Rentals
{
    // Informacje od użytkownika podczas dokonywania korekty ceny
    public class AdjustPrice
    {
        public decimal NewPrice { get; set; }

        public string Reason { get; set; }
    }
}
