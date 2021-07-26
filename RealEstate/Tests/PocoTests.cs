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

        //[BsonIgnoreExtraElements] - Ignoruje dane, które są w dokumencie, ale nie ma ich w modelu (nie rzuca wyjątkiem)
        public class Person
        {
            public string FirstName { get; set; }
            public int Age { get; set; }

            public List<string> Adress = new List<string>();
            //[BsonIgnoreIfNull] - Zignoruje właściwość i jej nie wyświetli, jeżeli jej wartość to null
            public Contact Contact = new Contact();

            [BsonIgnore]
            public string IgnoreMe { get; set; }
            [BsonElement("New")]
            public string Old { get; set; }
            [BsonElement]
            private string Encapsulated { get; set; }

            //[BsonRepresentation(BsonType.Double)] - Jawne określenie typu właściwości
            //public decimal NetWorth { get; set; }
            //[BsonDateTimeOptions(Kind = DateTimeKind.Local)] - Jawne ustawienie lokalnej strefy czasowej
            //public DateTime BirthTime { get; set; }
            //[BsonId] - Jawne określenie, która właściwość to identyfikator (Id)
            //public int PersonId { get; set; }
            //[BsonDateTimeOptions(DateOnly = true)] - Uwzględnienie tylko daty, bez przesunięcia czasowego
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

            Console.WriteLine(person.ToJson());
        }

        [Test]
        public void SerializationAttributes()
        {
            var person = new Person();

            Console.WriteLine(person.ToJson());
        }
    }
}