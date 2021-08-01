using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GridFS;
using MongoDB.Driver.Linq;
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
            var rentals = FilterRentals(filters);
            //.SetSortOrder(SortBy<Rental>.Ascending(r => r.Price));
            var model = new RentalsList
            {
                Rentals = rentals,
                Filters = filters
            };

            return View(model);
        }

        // Demo: Using Find to Add a Price Limit Filter
        //private MongoCursor<Rental> FilterRentals(RentalsFilter filters)
        //{
        //    if (!filters.PriceLimit.HasValue)
        //    {
        //        return Context.Rentals.FindAll();
        //    }

        //    var query = Query<Rental>.LTE(r => r.Price, filters.PriceLimit);

        //    return Context.Rentals.Find(query);
        //}

        private IEnumerable<Rental> FilterRentals(RentalsFilter filters)
        {
            IQueryable<Rental> rentals = Context.Rentals.AsQueryable()
                .OrderBy(r => r.Price);

            if (filters.MinimumRooms.HasValue)
            {
                rentals = rentals
                    .Where(r => r.NumberOfRooms >= filters.MinimumRooms);
            }

            if (filters.PriceLimit.HasValue)
            {
                var query = Query<Rental>.LTE(r => r.Price, filters.PriceLimit);

                rentals = rentals
                    .Where(r => query.Inject());
            }

            return rentals;
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

        // Modyfikacja zamiast zastępowania
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

        public string PriceDistribution()
        {
            return new QueryPriceDistribution()
                .Run(Context.Rentals)
                .ToJson();
        }

        public IActionResult AttachImage(string id)
        {
            var rental = GetRental(id);

            return View(rental);
        }

        [HttpPost]
        public IActionResult AttachImage(string id, IFormFile file) // .NET Core nie obsługuje już "HttpPostedFileBase"
        {
            var rental = GetRental(id);

            if (rental.HasImage()) DeleteImage(rental);

            StoreImage(file, rental);

            return RedirectToAction("Index");
        }

        //[HttpPost]
        //public IActionResult AttachImage(string id, IFormFile file) // .NET Core nie obsługuje już "HttpPostedFileBase"
        //{
        //    var rental = GetRental(id);

        //    if (rental.HasImage()) DeleteImage(rental);

        //    StoreImage(file, id);

        //    return RedirectToAction("Index");
        //}

        private void DeleteImage(Rental rental)
        {
            Context.Database.GridFS.DeleteById(new ObjectId(rental.ImageId));
            rental.ImageId = null;
            Context.Rentals.Save(rental);
        }

        //private void DeleteImage(Rental rental)
        //{
        //    Context.Database.GridFS.DeleteById(new ObjectId(rental.ImageId));

        //    // Użycie modyfikacji zamiast zastępowania
        //    SetRentalImageId(rental.Id, null); // Odłączenie obrazu
        //}

        private void StoreImage(IFormFile file, Rental rental) // .NET Core nie obsługuje już "HttpPostedFileBase"
        {
            var imageId = ObjectId.GenerateNewId();

            rental.ImageId = imageId.ToString();
            Context.Rentals.Save(rental);

            var options = new MongoGridFSCreateOptions
            {
                Id = imageId,
                ContentType = file.ContentType
            };

            Context.Database.GridFS.Upload(file.OpenReadStream(), file.FileName, options); // .NET Core nie obsługuje już file.InputStream - https://forums.asp.net/t/2090370.aspx?Inputstream+and+contentlength+is+missing+in+microsoft+aspnet+http+abstractions
        }

        //private void StoreImage(IFormFile file, string rentalId) // .NET Core nie obsługuje już "HttpPostedFileBase"
        //{
        //    var imageId = ObjectId.GenerateNewId();

        //    // Użycie modyfikacji zamiast zastępowania
        //    SetRentalImageId(rentalId, imageId.ToString()); // Podłączenie obrazu

        //    var options = new MongoGridFSCreateOptions
        //    {
        //        Id = imageId,
        //        ContentType = file.ContentType
        //    };

        //    Context.Database.GridFS.Upload(file.OpenReadStream(), file.FileName, options); // .NET Core nie obsługuje już file.InputStream - https://forums.asp.net/t/2090370.aspx?Inputstream+and+contentlength+is+missing+in+microsoft+aspnet+http+abstractions
        //}

        private void SetRentalImageId(string rentalId, string imageId)
        {
            var rentalById = Query<Rental>.Where(r => r.Id == rentalId);
            var setRentalImageId = Update<Rental>.Set(r => r.ImageId, imageId);

            Context.Rentals.Update(rentalById, setRentalImageId);
        }

        public IActionResult GetImage(string id)
        {
            var image = Context.Database.GridFS.FindOneById(new ObjectId(id));

            if (image == null) return NotFound();

            return File(image.OpenRead(), image.ContentType);
        }
    }
}
