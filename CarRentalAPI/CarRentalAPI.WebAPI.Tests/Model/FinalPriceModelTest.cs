using CarRentalAPI.Data;
using CarRentalAPI.WebAPI.Model;
using NUnit.Framework;
using System;

namespace CarRentalAPI.WebAPI.Tests.Model
{
    [TestFixture]
    public class FinalPriceModelTest
    {
        [Test]
        public void CalculateFinalPriceAsyncTest()
        {
            var model = new FinalPriceModel(
                new CarType { DayMultiplier = 1, KilometerMultiplier = 0 },
                new Setting { DayPrice = 100, KilometerPrice = 5 });

            model.DateFrom = new DateTime(2016, 12, 1, 10, 0, 0);
            model.DateTo = new DateTime(2016, 12, 5, 20, 10, 10);

            model.InitialMileage = 100;
            model.FinalMileage = 200;

            var expected = 600;
            var actual = model.CalculateFinalPriceAsync().Result;

            Assert.AreEqual(expected, actual);
        }
    }
}
