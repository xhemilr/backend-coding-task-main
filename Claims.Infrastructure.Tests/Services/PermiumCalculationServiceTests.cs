using Claims.Core.Enums;
using Claims.Infrastructure.Services;
using NUnit.Framework;

namespace Claims.Infrastructure.Tests.Services
{
    public class PermiumCalculationServiceTests
    {
        [TestCase("01-01-2023", "01-11-2022", CoverType.Yacht, 0)]              // Should return 0 if start date is bigger than end data.
        [TestCase("01-01-2023", "01-11-2023", CoverType.Yacht, 13750)]          // For 10 days calculation (1250 * 1.1 * 10) = 13750
        [TestCase("01-01-2023", "01-11-2023", CoverType.PassengerShip, 15000)]  // For 10 days calculation (1250 * 1.2 * 10) = 15000
        [TestCase("01-01-2023", "01-11-2023", CoverType.Tanker, 18750)]         // For 10 days calculation (1250 * 1.5 * 10) = 18750
        [TestCase("01-01-2023", "01-11-2023", CoverType.BulkCarrier, 16250)]    // For 10 days calculation (1250 * 1.3 * 10) = 16250
        [TestCase("01-01-2023", "02-10-2023", CoverType.Yacht, 53125)]          // For 40 days calculation (1250 * 1.1 * 30 = 41250) + ((1250 - (1250 * 0.05)) * 10 = 11875) = 53125
        [TestCase("01-01-2023", "02-10-2023", CoverType.PassengerShip, 57250)]  // For 40 days calculation (1250 * 1.2 * 30 = 45000) + ((1250 - (1250 * 0.02)) * 10 = 12250) = 57250
        [TestCase("01-01-2023", "02-10-2023", CoverType.Tanker, 68500)]         // For 40 days calculation (1250 * 1.5 * 30 = 56250) + ((1250 - (1250 * 0.02)) * 10 = 12250) = 68500
        [TestCase("01-01-2023", "02-10-2023", CoverType.BulkCarrier, 61000)]    // For 40 days calculation (1250 * 1.3 * 30 = 48750) + ((1250 - (1250 * 0.02)) * 10 = 12250) = 61000
        [TestCase("01-01-2023", "11-27-2023", CoverType.Yacht, 391875)]         // For 330 days calculation (1250 * 1.1 * 30 = 41250) + ((1250 - (1250 * 0.05)) * 150 = 178125) + ((1250 - (1250 * 0.08)) * 150 = 172500) = 391875
        [TestCase("01-01-2023", "11-27-2023", CoverType.PassengerShip, 410625)] // For 330 days calculation (1250 * 1.2 * 30 = 45000) + ((1250 - (1250 * 0.02)) * 150 = 183750) + ((1250 - (1250 * 0.03)) * 150 = 181875) = 410625
        [TestCase("01-01-2023", "11-27-2023", CoverType.Tanker, 421875)]        // For 330 days calculation (1250 * 1.5 * 30 = 56250) + ((1250 - (1250 * 0.02)) * 150 = 183750) + ((1250 - (1250 * 0.03)) * 150 = 181875) = 421875
        [TestCase("01-01-2023", "11-27-2023", CoverType.BulkCarrier, 414375)]   // For 330 days calculation (1250 * 1.3 * 30 = 48750) + ((1250 - (1250 * 0.02)) * 150 = 183750) + ((1250 - (1250 * 0.03)) * 150 = 181875) = 414375
        public void PermiumCalculation(string startDate, string endDate, CoverType coverType, decimal result)
        {
            var premiumCalculationService = new PermiumCalculationService();

            var totalCost = premiumCalculationService.CalculatePremium(DateOnly.Parse(startDate), DateOnly.Parse(endDate), coverType);

            Assert.That(totalCost, Is.EqualTo(result));
        }
    }
}
