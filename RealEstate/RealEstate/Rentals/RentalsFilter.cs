﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstate.Rentals
{
    public class RentalsFilter
    {
        public decimal? PriceLimit { get; set; }
        public int? MinimumRooms { get; set; }
    }
}
