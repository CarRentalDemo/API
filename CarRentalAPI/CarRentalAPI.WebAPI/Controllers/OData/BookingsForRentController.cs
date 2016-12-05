using System.Data;
using System.Linq;
using System.Web.Http;
using CarRentalAPI.Data;
using System.Web.OData;

namespace CarRentalAPI.WebAPI.Controllers.OData
{
    public class BookingsForRentController : ODataController
    {
        private CarRentalDatabase db = new CarRentalDatabase();

        // GET: odata/BookingsForRent
        [EnableQuery]
        public IQueryable<Booking> GetBookingsForRent()
        {
            return db.Bookings.Where(x => !x.Rents.Any());
        }

        // GET: odata/BookingsForRent(5)/CarType
        [EnableQuery]
        public SingleResult<CarType> GetCarType([FromODataUri] int key)
        {
            return SingleResult.Create(db.Bookings.Where(m => m.Id == key).Select(m => m.CarType));
        }

        // GET: odata/BookingsForRent(5)/Client
        [EnableQuery]
        public SingleResult<Client> GetClient([FromODataUri] int key)
        {
            return SingleResult.Create(db.Bookings.Where(m => m.Id == key).Select(m => m.Client));
        }

        // GET: odata/BookingsForRent(5)/Rents
        [EnableQuery]
        public IQueryable<Rent> GetRents([FromODataUri] int key)
        {
            return db.Bookings.Where(m => m.Id == key).SelectMany(m => m.Rents);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BookingExists(int key)
        {
            return db.Bookings.Count(e => e.Id == key) > 0;
        }
    }
}
