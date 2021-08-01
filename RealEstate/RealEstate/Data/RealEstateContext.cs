using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using RealEstate.Controllers;
using RealEstate.Rentals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstate.Data
{
    public class RealEstateContext
    {
        public MongoDatabase Database;

        public RealEstateContext()
        {
            // Połączenie z serwerem zaczyna się od instancji MongoClient
            // Klasa przyjmuje ciąg połączenia (URI zaczynające się od prefiksu "mongodb")
            //var client = new MongoClient("mongodb://localhost");
            MongoClient client = new MongoClient("mongodb://localhost");

            //var server = client.GetServer();
            MongoServer server = client.GetServer();

            Database = server.GetDatabase("RealEstate");
            //Database = server.GetDatabase("Test"); - Baza utworzy się automatycznie po dodaniu do niej jakiegokolwiek elementu
        }

        public MongoCollection<Rental> Rentals
        {
            get
            {
                return Database.GetCollection<Rental>("Rentals");
                //return Database.GetCollection<Rental>("rentals"); - Kolekcja sama się utworzy po dodaniu do niej jakiegokolwiek elementu
            }
        }
    }
}
