using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization.Attributes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    class PocoTests
    {
        public PocoTests()
        {
            JsonWriterSettings.Defaults.Indent = true;
        }

        //[BsonIgnoreExtraElements]
        public class Person
        {
            public string FirstName { get; set; }
            public int Age { get; set; }

            public List<string> Adress = new List<string>();
            //[BsonIgnoreIfNull]
            public Contact Contact = new Contact();

            [BsonIgnore]
            public string IgnoreMe { get; set; }
            [BsonElement("New")]
            public string Old { get; set; }
            [BsonElement]
            private string Encapsulated { get; set; }

            //[BsonRepresentation(BsonType.Double)]
            //public decimal NetWorth { get; set; }
            //[BsonDateTimeOptions(Kind = DateTimeKind.Local)]
            //public DateTime BirthTime { get; set; }
            //[BsonId]
            //public int PersonId { get; set; }
            //[BsonDateTimeOptions(DateOnly = true)]
            //public DateTime BirthDate { get; set; }
        }

        public class Contact
        {
            public string Email { get; set; }
            public string Phone { get; set; }
        }

        [Test]
        public void Automatic()
        {
            var person = new Person
            {
                FirstName = "Gabriel",
                Age = 21
            };

            person.Adress.Add("101 Some Road");
            person.Adress.Add("Unit 501");

            person.Contact.Email = "email@email.com";
            person.Contact.Phone = "123-456-789";

            Console.WriteLine(person);
            Console.WriteLine($"To JSON: {person.ToJson()}");

            //Tests.PocoTests + Person
            //To JSON: {
            //    "Adress" : ["101 Some Road", "Unit 501"],
            //    "Contact" : {
            //        "Email" : "email@email.com",
            //        "Phone" : "123-456-789"
            //    },
            //    "FirstName" : "Gabriel",
            //    "Age" : 21,
            //    "New" : null,
            //    "Encapsulated" : null
            //}
        }

        [Test]
        public void SerializationAttributes()
        {
            var person = new Person();
            //person.NetWorth = 10.09m;
            //person.BirthTime = new DateTime(2014, 1, 2, 11, 30, 0);

            Console.WriteLine($"To JSON: {person.ToJson()}");
            Console.WriteLine($"To BSON: {person.ToBson()}");
            Console.WriteLine($"To BSON in HEX: {BitConverter.ToString(person.ToBson())}");

            //To JSON: {
            //    "Adress" : [],
            //    "Contact" : {
            //        "Email" : null,
            //        "Phone" : null
            //    },
            //    "FirstName" : null,
            //    "Age" : 0,
            //    "New" : null,
            //    "Encapsulated" : null
            //}
            //To BSON: System.Byte[]
            //To BSON in HEX: 55 - 00 - 00 - 00 - 04 - 41 - 64 - 72 - 65 - 73 - 73 - 00 - 05 - 00 - 00 - 00 - 00 - 03 - 43 - 6F - 6E-74 - 61 - 63 - 74 - 00 - 13 - 00 - 00 - 00 - 0A - 45 - 6D - 61 - 69 - 6C - 00 - 0A - 50 - 68 - 6F - 6E-65 - 00 - 00 - 0A - 46 - 69 - 72 - 73 - 74 - 4E-61 - 6D - 65 - 00 - 10 - 41 - 67 - 65 - 00 - 00 - 00 - 00 - 00 - 0A - 4E-65 - 77 - 00 - 0A - 45 - 6E-63 - 61 - 70 - 73 - 75 - 6C - 61 - 74 - 65 - 64 - 00 - 00
        }
    }
}