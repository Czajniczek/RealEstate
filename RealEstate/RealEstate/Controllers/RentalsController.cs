using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using RealEstate.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstate.Rentals
{
    public class RentalsController : Controller
    {
        public readonly RealEstateContext Context = new RealEstateContext();

        public IActionResult Index()
        {
            var rentals = Context.Rentals.FindAll();

            return View(rentals);
        }

        public IActionResult Post()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Post(PostRental postRental)
        {
            var rental = new Rental(postRental);
            Context.Rentals.Insert(rental);

            return RedirectToAction("Index");
        }

        public IActionResult AdjustPrice(string id)
        {
            var rental = GetRental(id);

            return View(rental);
        }

        public Rental GetRental(string id)
        {
            var rental = Context.Rentals.FindOneById(new ObjectId(id));

            return rental;
        }

        [HttpPost]
        public IActionResult AdjustPrice(string id, AdjustPrice adjustPrice)
        {
            var rental = GetRental(id);
            rental.AdjustPrice(adjustPrice);

            Context.Rentals.Save(rental);

            return RedirectToAction("Index");
        }
    }
}
