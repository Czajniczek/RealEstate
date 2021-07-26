using MongoDB.Bson;
using MongoDB.Bson.IO;
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

            Console.WriteLine(document.ToJson());
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

            Console.WriteLine(person);
        }

        [Test]
        public void AddingArrays()
        {
            var person = new BsonDocument();
            person.Add("adress", new BsonArray(new[] { "101 Some Road", "Unit 501" }));

            Console.WriteLine(person);
        }

        [Test]
        public void EmbeddedDocument()
        {
            var person = new BsonDocument
            {
                {"contact", new BsonDocument
                {
                    {"phone", "123-456-789" },
                    {"email","email@email.com" }
                }
                }
            };

            Console.WriteLine(person);
        }

        [Test]
        public void BsonValueConversions()
        {
            var person = new BsonDocument
            {
                {"age", 54 }
            };

            Console.WriteLine(person["age"].ToDouble() + 10);
            Console.WriteLine(person["age"].IsInt32);
            Console.WriteLine(person["age"].IsString);
        }
    }
}
