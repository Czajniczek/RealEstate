using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
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

        public IActionResult Index(RentalsFilter filters)
        {
            //var cursor = FilterRentals(filters);
            //cursor.ToList();
            //var results = cursor
            //    .SetSortOrder(SortBy<Rental>.Ascending(r => r.Price));

            var rentals = FilterRentals(filters)
                .SetSortOrder(SortBy<Rental>.Ascending(r => r.Price));
            var model = new RentalsList
            {
                Rentals = rentals,
                Filters = filters
            };

            return View(model);
        }

        private MongoCursor<Rental> FilterRentals(RentalsFilter filters)
        {
            if (!filters.PriceLimit.HasValue)
            {
                return Context.Rentals.FindAll();
            }

            var query = Query<Rental>.LTE(r => r.Price, filters.PriceLimit);

            return Context.Rentals.Find(query);
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

        private Rental GetRental(string id)
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

        //[HttpPost]
        //public IActionResult AdjustPrice(string id, AdjustPrice adjustPrice)
        //{
        //    var rental = GetRental(id);
        //    var adjustment = new PriceAdjustment(adjustPrice, rental.Price);
        //    var modificationUpdate = Update<Rental>
        //        .Push(r => r.Adjustments, adjustment)
        //        .Set(r => r.Price, adjustment.NewPrice);

        //    Context.Rentals.Update(Query.EQ("_id", new ObjectId(id)), modificationUpdate);

        //    return RedirectToAction("Index");
        //}

        public IActionResult Delete(string id)
        {
            Context.Rentals.Remove(Query.EQ("_id", new ObjectId(id)));

            return RedirectToAction("Index");
        }
    }
}
