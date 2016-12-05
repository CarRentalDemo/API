using CarRentalAPI.Data;
using CarRentalAPI.WebAPI.Model;
using NUnit.Framework;
using System;
using System.Collections;

namespace CarRentalAPI.WebAPI.Tests.Model
{
    [TestFixture]
    public class FinalPriceModelTest
    {
        [Test]
        [TestCaseSource("TestCases")]
        public decimal CalculateFinalPriceAsyncTest(
            decimal DayMultiplier, decimal DayPrice,
            decimal KilometerMultiplier, decimal KilometerPrice,
            DateTime DateFrom, DateTime DateTo,
            decimal InitialMileage, decimal FinalMileage)
        {
            var model = new FinalPriceModel(
                new CarType { DayMultiplier = DayMultiplier, KilometerMultiplier = KilometerMultiplier },
                new Setting { DayPrice = DayPrice, KilometerPrice = KilometerPrice });

            model.DateFrom = DateFrom;
            model.DateTo = DateTo;

            model.InitialMileage = InitialMileage;
            model.FinalMileage = FinalMileage;
            
            return model.CalculateFinalPriceAsync().Result;
        }

        public static IEnumerable TestCases
        {
            get
            {
                yield return new TestCaseData(1m, 100m, 0m, 20m, new DateTime(2016, 12, 1, 10, 0, 0), new DateTime(2016, 12, 5, 20, 10, 10), 100m, 200m).Returns(600);
                yield return new TestCaseData(1m, 100m, 1m, 20m, new DateTime(2016, 12, 1, 10, 0, 0), new DateTime(2016, 12, 5, 20, 10, 10), 100m, 200m).Returns(2600);
                yield return new TestCaseData(0m, 100m, 0m, 20m, new DateTime(2016, 12, 1, 10, 0, 0), new DateTime(2016, 12, 5, 20, 10, 10), 100m, 200m).Returns(0);
                yield return new TestCaseData(1m, 100m, 1.5m, 20m, new DateTime(2016, 12, 1, 10, 0, 0), new DateTime(2016, 12, 5, 20, 10, 10), 100m, 200m).Returns(3600);
            }
        }
    }
}
