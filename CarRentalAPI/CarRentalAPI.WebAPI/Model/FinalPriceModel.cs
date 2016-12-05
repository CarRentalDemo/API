using CarRentalAPI.Data;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace CarRentalAPI.WebAPI.Model
{
    public class FinalPriceModel
    {
        private CarType _carType;
        private Setting _settings;

        public int CarTypeId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public decimal InitialMileage { get; set; }
        public decimal FinalMileage { get; set; }

        public FinalPriceModel()
        {
        }

        public FinalPriceModel(CarType carType, Setting settings)
        {
            _carType = carType;
            _settings = settings;
        }

        public async Task<decimal> CalculateFinalPriceAsync()
        {
            if (_carType == null || _settings == null)
            {
                using (var db = new CarRentalDatabase())
                {
                    _carType = _carType ?? await db.CarTypes.FirstOrDefaultAsync(x => x.Id == CarTypeId);
                    _settings = _settings ?? await db.Settings.FirstOrDefaultAsync();
                }
            }

            var days = (int)Math.Ceiling(DateTo.Subtract(DateFrom).TotalDays) + 1;
            var mileage = FinalMileage - InitialMileage;

            return _carType.DayMultiplier * days * _settings.DayPrice
                + _carType.KilometerMultiplier * mileage * _settings.KilometerPrice;
        }
    }
}