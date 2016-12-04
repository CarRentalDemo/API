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
    builder.EntitySet<CarType>("CarTypes");
    builder.EntitySet<Booking>("Bookings"); 
    builder.EntitySet<Car>("Cars"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class CarTypesController : ODataController
    {
        private CarRentalDatabase db = new CarRentalDatabase();

        // GET: odata/CarTypes
        [EnableQuery]
        public IQueryable<CarType> GetCarTypes()
        {
            return db.CarTypes;
        }

        // GET: odata/CarTypes(5)
        [EnableQuery]
        public SingleResult<CarType> GetCarType([FromODataUri] byte key)
        {
            return SingleResult.Create(db.CarTypes.Where(carType => carType.Id == key));
        }

        // PUT: odata/CarTypes(5)
        public async Task<IHttpActionResult> Put([FromODataUri] byte key, Delta<CarType> patch)
        {
            //Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CarType carType = await db.CarTypes.FindAsync(key);
            if (carType == null)
            {
                return NotFound();
            }

            patch.Put(carType);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarTypeExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(carType);
        }

        // POST: odata/CarTypes
        public async Task<IHttpActionResult> Post(CarType carType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CarTypes.Add(carType);
            await db.SaveChangesAsync();

            return Created(carType);
        }

        // PATCH: odata/CarTypes(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] byte key, Delta<CarType> patch)
        {
            //Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CarType carType = await db.CarTypes.FindAsync(key);
            if (carType == null)
            {
                return NotFound();
            }

            patch.Patch(carType);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarTypeExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(carType);
        }

        // DELETE: odata/CarTypes(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] byte key)
        {
            CarType carType = await db.CarTypes.FindAsync(key);
            if (carType == null)
            {
                return NotFound();
            }

            db.CarTypes.Remove(carType);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/CarTypes(5)/Bookings
        [EnableQuery]
        public IQueryable<Booking> GetBookings([FromODataUri] byte key)
        {
            return db.CarTypes.Where(m => m.Id == key).SelectMany(m => m.Bookings);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CarTypeExists(byte key)
        {
            return db.CarTypes.Count(e => e.Id == key) > 0;
        }
    }
}
