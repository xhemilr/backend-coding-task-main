using Claims.Core.Enums;

namespace Claims.Application.Services
{
    public interface IPremiumCalculationService
    {
        decimal CalculatePremium(DateOnly startDate, DateOnly endDate, CoverType coverType);
    }
}
