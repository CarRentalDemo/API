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
    builder.EntitySet<Car>("Cars");
    builder.EntitySet<CarType>("CarTypes"); 
    builder.EntitySet<Rent>("Rents"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class CarsController : ODataController
    {
        private CarRentalDatabase db = new CarRentalDatabase();

        // GET: odata/Cars
        [EnableQuery]
        public IQueryable<Car> GetCars()
        {
            return db.Cars;
        }

        // GET: odata/Cars(5)
        [EnableQuery]
        public SingleResult<Car> GetCar([FromODataUri] int key)
        {
            return SingleResult.Create(db.Cars.Where(car => car.Id == key));
        }

        // PUT: odata/Cars(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<Car> patch)
        {
            //Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Car car = await db.Cars.FindAsync(key);
            if (car == null)
            {
                return NotFound();
            }

            patch.Put(car);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(car);
        }

        // POST: odata/Cars
        public async Task<IHttpActionResult> Post(Car car)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Cars.Add(car);
            await db.SaveChangesAsync();

            return Created(car);
        }

        // PATCH: odata/Cars(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Car> patch)
        {
            //Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Car car = await db.Cars.FindAsync(key);
            if (car == null)
            {
                return NotFound();
            }

            patch.Patch(car);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(car);
        }

        // DELETE: odata/Cars(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            Car car = await db.Cars.FindAsync(key);
            if (car == null)
            {
                return NotFound();
            }

            db.Cars.Remove(car);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Cars(5)/CarType
        [EnableQuery]
        public SingleResult<CarType> GetCarType([FromODataUri] int key)
        {
            return SingleResult.Create(db.Cars.Where(m => m.Id == key).Select(m => m.CarType));
        }

        // GET: odata/Cars(5)/Rents
        [EnableQuery]
        public IQueryable<Rent> GetRents([FromODataUri] int key)
        {
            return db.Cars.Where(m => m.Id == key).SelectMany(m => m.Rents);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CarExists(int key)
        {
            return db.Cars.Count(e => e.Id == key) > 0;
        }
    }
}
