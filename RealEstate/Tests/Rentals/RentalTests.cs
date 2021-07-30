using MongoDB.Bson;
using MongoDB.Bson.IO;
using NUnit.Framework;
using RealEstate.Rentals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Rentals
{
    [TestFixture]
    class RentalTests : AssertionHelper
    {
        public RentalTests()
        {
            JsonWriterSettings.Defaults.Indent = true;
        }

        [Test]
        public void ToDocument_RentalWithPrice_PriceRepresentedAsDouble()
        {
            var rental = new Rental();
            rental.Price = 1;

            var document = rental.ToBsonDocument();

            Expect(document["Price"].BsonType, Is.EqualTo(BsonType.Double));

            Console.WriteLine(document);

            //{
            //    "Address" : [],
            //    "Adjustments" : [],
            //    "_id" : null,
            //    "Description" : null,
            //    "NumberOfRooms" : 0,
            //    "Price" : 1.0
            //}
        }

        [Test]
        public void ToDocument_RentalWithAnId_IdIsRepresentedAsAnObjectId()
        {
            var rental = new Rental();
            rental.Id = ObjectId.GenerateNewId().ToString();

            var document = rental.ToBsonDocument();

            Expect(document["_id"].BsonType, Is.EqualTo(BsonType.ObjectId));

            Console.WriteLine(document);

            //{
            //    "Address" : [],
            //    "Adjustments" : [],
            //    "_id" : ObjectId("6103bceb511bf789b947230e"),
            //    "Description" : null,
            //    "NumberOfRooms" : 0,
            //    "Price" : 0.0
            //}
        }
    }
}
