using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstate.Rentals
{
    public class Rental
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public List<string> Address = new List<string>();

        public List<PriceAdjustment> Adjustments = new List<PriceAdjustment>();

        public string Description { get; set; }

        public int NumberOfRooms { get; set; }

        [BsonRepresentation(BsonType.Double)]
        public decimal Price { get; set; }

        public string ImageId { get; set; }

        // Deserializacja
        public Rental() { }

        public Rental(PostRental postRental)
        {
            Address = (postRental.Address ?? string.Empty).Split('\n').ToList();
            Description = postRental.Description;
            NumberOfRooms = postRental.NumberOfRooms;
            Price = postRental.Price;
        }

        public void AdjustPrice(AdjustPrice adjustPrice)
        {
            var adjustment = new PriceAdjustment(adjustPrice, Price);

            Adjustments.Add(adjustment);
            Price = adjustPrice.NewPrice;
        }

        public bool HasImage()
        {
            return !String.IsNullOrWhiteSpace(ImageId);
        }
    }
}
