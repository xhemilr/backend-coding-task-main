using Claims.Application.Services;
using Claims.Core.Enums;

namespace Claims.Infrastructure.Services
{
    public class PermiumCalculationService : IPremiumCalculationService
    {
        public decimal CalculatePremium(DateOnly startDate, DateOnly endDate, CoverType coverType)
        {
            if(startDate > endDate)
                return 0;

            decimal totalPremium;
            int baseRate = 1250;

            var period = endDate.DayNumber - startDate.DayNumber;

            totalPremium = CalculateFirstPeriod(baseRate, period, coverType);

            if ((period - 30) > 0)
            {
                totalPremium += CalculateSecondPeriod(baseRate, period - 30, coverType);
            }

            if ((period - 180) > 0)
            {
                totalPremium += CalculateThirdPeriod(baseRate, period - 180, coverType);
            }

            return totalPremium;
        }

        private decimal CalculateFirstPeriod(decimal baseRate, int period, CoverType coverType)
        {
            decimal finalRate;
            switch (coverType)
            {
                case CoverType.Yacht:
                    finalRate = baseRate * 1.1m;
                    break;
                case CoverType.PassengerShip:
                    finalRate = baseRate * 1.2m;
                    break;
                case CoverType.Tanker:
                    finalRate = baseRate * 1.5m;
                    break;
                default:
                    finalRate = baseRate * 1.3m;
                    break;
            }

            if (period <= 30)
            {
                return finalRate * period;
            }

            return finalRate * 30;
        }

        private decimal CalculateSecondPeriod(decimal baseRate, int period, CoverType coverType)
        {
            decimal finalRate;
            switch (coverType)
            {
                case CoverType.Yacht:
                    finalRate = baseRate - (baseRate * 0.05m);
                    break;
                default:
                    finalRate = baseRate - (baseRate * 0.02m);
                    break;
            }

            if (period <= 150)
            {
                return finalRate * period;
            }
            return finalRate * 150;
        }

        private decimal CalculateThirdPeriod(decimal baseRate, int period, CoverType coverType)
        {
            decimal finalRate;
            switch (coverType)
            {
                case CoverType.Yacht:
                    finalRate = baseRate - (baseRate * 0.08m);
                    break;
                default:
                    finalRate = baseRate - (baseRate * 0.03m);
                    break;
            }
            return finalRate * period;
        }

    }
}
