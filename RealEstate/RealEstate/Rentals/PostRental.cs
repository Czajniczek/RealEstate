using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstate.Rentals
{
    public class PostRental
    {
        public string Description { get; set; }
        public int NumberOfRooms { get; set; }
        public decimal Price { get; set; }
        //public List<string> Address = new List<string>();
        public string Address { get; set; }
    }
}
