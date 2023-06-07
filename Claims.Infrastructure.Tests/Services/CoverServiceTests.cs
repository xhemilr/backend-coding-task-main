using AutoMapper;
using Claims.Application.Exceptions;
using Claims.Application.Requests.Cover;
using Claims.Application.Responses;
using Claims.Application.Services;
using Claims.Core.Entities;
using Claims.Core.Repository;
using Claims.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace Claims.Infrastructure.Tests.Services
{
    public class CoverServiceTests
    {
        [Test]
        public async Task Should_Call_Add_Item()
        {
            var instance = CreateInstance(out var coverRepository, out var _, out var _);

            coverRepository.AddItemAsync(Arg.Any<Cover>()).Returns(new Cover());

            var request = new CreateCoverRequest();

            await instance.CreateAsync(request);

            await coverRepository.Received(1).AddItemAsync(Arg.Any<Cover>());
        }

        [Test]
        public async Task Should_Call_Delete_Item()
        {
            var instance = CreateInstance(out var coverRepository, out var mapper, out var _);

            coverRepository.GetItemAsync(Arg.Any<string>()).Returns(new Cover());

            mapper.Map<CoverResponse>(Arg.Any<Cover>()).Returns(new CoverResponse());

            await instance.DeleteAsync("1");

            await coverRepository.Received(1).DeleteItemAsync(Arg.Any<string>());
        }

        [Test]
        public void Shoul_Throw_Error_If_Delete_Called_On_Wrong_Id()
        {
            var instance = CreateInstance(out var coverRepository, out var _, out var _);

            coverRepository.GetItemAsync(Arg.Any<string>()).Returns(new Cover());

            Assert.ThrowsAsync<EntityNotFoundException>(() => instance.DeleteAsync("1"));
        }

        [Test]
        public async Task Shoul_Call_GetAllItems()
        {
            var instance = CreateInstance(out var coverRepository, out var _, out var _);

            await instance.GetAllAsync();

            await coverRepository.Received(1).GetAllAsync();
        }

        [Test]
        public async Task Shoul_Call_GetItemAsync()
        {
            var instance = CreateInstance(out var coverRepository, out var _, out var _);

            await instance.GetByIdAsync("1");

            await coverRepository.Received(1).GetItemAsync(Arg.Any<string>());
        }

        [Test]
        public async Task Shoul_Call_ComputePremium()
        {
            var instance = CreateInstance(out var _, out var _, out var premiumCalcutaionService);

            await instance.ComputePremiumAsync(DateOnly.Parse("01-01-2023"), DateOnly.Parse("01-01-2023"), Core.Enums.CoverType.Yacht);

            premiumCalcutaionService.CalculatePremium(Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), Arg.Any<Core.Enums.CoverType>());
        }

        private CoverService CreateInstance(out ICoverRepository coverRepository, out IMapper mapper, out IPremiumCalculationService premiumCalculationService)
        {
            var logger = Substitute.For<ILogger<CoverService>>();
            coverRepository = Substitute.For<ICoverRepository>();
            premiumCalculationService = Substitute.For<IPremiumCalculationService>();
            mapper = Substitute.For<IMapper>();
            var httpClientFactory = Substitute.For<IHttpClientFactory>();
            var dateTimeService = Substitute.For<IDateTimeService>();

            httpClientFactory.CreateClient(Arg.Any<string>()).Returns(new HttpClient());
            

            return new CoverService(logger, coverRepository, premiumCalculationService, mapper, httpClientFactory, dateTimeService);
        }
    }
}
