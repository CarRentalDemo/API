using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using CarRentalAPI.Data;
using System.Web.OData;

namespace CarRentalAPI.WebAPI.Controllers.OData
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using CarRentalAPI.Data;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Booking>("Bookings");
    builder.EntitySet<CarType>("CarTypes"); 
    builder.EntitySet<Client>("Clients"); 
    builder.EntitySet<Rent>("Rents"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class BookingsController : ODataController
    {
        private CarRentalDatabase db = new CarRentalDatabase();

        // GET: odata/Bookings
        [EnableQuery]
        public IQueryable<Booking> GetBookings()
        {
            return db.Bookings;
        }

        // GET: odata/Bookings(5)
        [EnableQuery]
        public SingleResult<Booking> GetBooking([FromODataUri] int key)
        {
            return SingleResult.Create(db.Bookings.Where(booking => booking.Id == key));
        }

        // PUT: odata/Bookings(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<Booking> patch)
        {
            //Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Booking booking = await db.Bookings.FindAsync(key);
            if (booking == null)
            {
                return NotFound();
            }

            patch.Put(booking);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(booking);
        }

        // POST: odata/Bookings
        public async Task<IHttpActionResult> Post(Booking booking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Bookings.Add(booking);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (BookingExists(booking.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(booking);
        }

        // PATCH: odata/Bookings(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Booking> patch)
        {
            //Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Booking booking = await db.Bookings.FindAsync(key);
            if (booking == null)
            {
                return NotFound();
            }

            patch.Patch(booking);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(booking);
        }

        // DELETE: odata/Bookings(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            Booking booking = await db.Bookings.FindAsync(key);
            if (booking == null)
            {
                return NotFound();
            }

            db.Bookings.Remove(booking);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Bookings(5)/CarType
        [EnableQuery]
        public SingleResult<CarType> GetCarType([FromODataUri] int key)
        {
            return SingleResult.Create(db.Bookings.Where(m => m.Id == key).Select(m => m.CarType));
        }

        // GET: odata/Bookings(5)/Client
        [EnableQuery]
        public SingleResult<Client> GetClient([FromODataUri] int key)
        {
            return SingleResult.Create(db.Bookings.Where(m => m.Id == key).Select(m => m.Client));
        }

        // GET: odata/Bookings(5)/Rents
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
