using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using NUnit.Framework;
using System;

namespace Tests
{
    public class BsonDocumentTests
    {
        public BsonDocumentTests()
        {
            JsonWriterSettings.Defaults.Indent = true;
        }

        [Test]
        public void EmptyDocument()
        {
            var document = new BsonDocument();

            Console.WriteLine($"To JSON: {document.ToJson()}");
            Console.WriteLine(document);

            //To JSON: { }
            //{ }
        }

        [Test]
        public void AddingElements()
        {
            //var person = new BsonDocument();
            var person = new BsonDocument
            {
                {"age", new BsonInt32(54) },
                {"IsAlive", true }
            };
            person.Add("firstName", new BsonString("Gabriel"));

            Console.WriteLine(person.ToJson());
            Console.WriteLine(person);

            //{
            //    "age" : 54,
            //    "IsAlive" : true,
            //    "firstName" : "Gabriel"
            //}
            //{
            //    "age" : 54,
            //    "IsAlive" : true,
            //    "firstName" : "Gabriel"
            //}
        }

        [Test]
        public void AddingArrays()
        {
            var person = new BsonDocument();
            person.Add("adress", new BsonArray(new[] { "101 Some Road", "Unit 501" }));

            Console.WriteLine(person);

            //{
            //    "adress" : ["101 Some Road", "Unit 501"]
            //}
        }

        [Test]
        public void EmbeddedDocument()
        {
            var person = new BsonDocument
            {
                {
                    "contact", new BsonDocument
                    {
                        {"phone", "123-456-789" },
                        {"email", "email@email.com" }
                    }
                }
            };

            Console.WriteLine(person);

            //{
            //    "contact" : {
            //        "phone" : "123-456-789",
            //        "email" : "email@email.com"
            //    }
            //}
        }

        [Test]
        public void BsonValueConversions()
        {
            var person = new BsonDocument
            {
                {"age", 54 }
            };

            Console.WriteLine(person["age"]);
            Console.WriteLine(person["age"].ToDouble() + 10);
            Console.WriteLine(person["age"].IsInt32);
            Console.WriteLine(person["age"].IsString);

            //54
            //64
            //True
            //False
        }

        [Test]
        public void ToBson()
        {
            var person = new BsonDocument
            {
                {"firstName", "Gabriel" }
            };
            var bson = person.ToBson();

            Console.WriteLine(bson);
            Console.WriteLine(BitConverter.ToString(bson));

            var deserializedPerson = BsonSerializer.Deserialize<BsonDocument>(bson);

            Console.WriteLine(deserializedPerson);

            //System.Byte[]
            //1C - 00 - 00 - 00 - 02 - 66 - 69 - 72 - 73 - 74 - 4E-61 - 6D - 65 - 00 - 08 - 00 - 00 - 00 - 47 - 61 - 62 - 72 - 69 - 65 - 6C - 00 - 00
            //{
            //    "firstName" : "Gabriel"
            //}
        }

        [Test]
        public void ExampleDocument()
        {
            var document = new BsonDocument
            {
                {"id", ObjectId.GenerateNewId() },
                {"orderedAt", new BsonDateTime(new DateTime(2014,01,01)) },// new DateTime(2014, 01, 01) },
                {"customerId", 1234 },
                {"salesAssociateId", 4567 },
                {"items", new BsonArray(new []
                    {
                        new BsonDocument
                        {
                            {"product", "Wood Chair" },
                            {"quantity", 2 },
                            {"price", new BsonDouble(55.99) }
                        },
                        new BsonDocument
                        {
                            {"product", "Wood Table" },
                            {"quantity", 1 },
                            {"price", 450.99 }
                        }
                    })
                },
                {"payment", new BsonDocument
                    {
                        {"totalCost", 54.45 },
                        {"paidAt",  new DateTime(2014, 01, 01)},
                        {"with", "paypal" }
                    }
                }
            };

            Console.WriteLine(document);

            //{
            //    "id" : ObjectId("6103b53ce34ad3097da77708"),
            //    "orderedAt" : ISODate("2013-12-31T23:00:00Z"),
            //    "customerId" : 1234,
            //    "salesAssociateId" : 4567,
            //    "items" : [{
            //            "product" : "Wood Chair",
            //            "quantity" : 2,
            //            "price" : 55.990000000000002
            //        }, {
            //            "product" : "Wood Table",
            //            "quantity" : 1,
            //            "price" : 450.99000000000001
            //        }],
            //    "payment" : {
            //        "totalCost" : 54.450000000000003,
            //        "paidAt" : ISODate("2013-12-31T23:00:00Z"),
            //        "with" : "paypal"
            //    }
            //}
        }
    }
}
