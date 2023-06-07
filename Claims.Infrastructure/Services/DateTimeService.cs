using Claims.Application.Services;

namespace Claims.Infrastructure.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime GetUtcNow() => DateTime.UtcNow;
    }
}
