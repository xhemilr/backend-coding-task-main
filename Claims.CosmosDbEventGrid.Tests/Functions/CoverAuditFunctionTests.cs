using Azure.Messaging.EventGrid;
using Claims.CosmosDbEventGrid.Entities;
using Claims.CosmosDbEventGrid.Functions;
using Claims.CosmosDbEventGrid.Interfaces.Repository;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace Claims.CosmosDbEventGrid.Tests.Functions
{
    public class CoverAuditFunctionTests
    {
        [TestCase("1", "POST")]
        [TestCase("2", "DELETE")]
        public async Task Should_Call_CoverRepository(string id, string method)
        {
            var instance = CreateInstance(out var logger, out var coverAuditRepository);

            var data = new { CoverId = id, HttpRequestType = method };

            var eventGrid = new EventGridEvent(string.Empty, string.Empty, string.Empty, data);

            await instance.Run(eventGrid);

            await coverAuditRepository.Received(1).AddAsync(Arg.Is<CoverAudit>(x => x.CoverId == id && x.HttpRequestType == method));
        }

        private CoverAuditFunction CreateInstance(out ILogger<CoverAuditFunction> logger, out ICoverAuditRepository coverAuditRepository)
        {
            logger = Substitute.For<ILogger<CoverAuditFunction>>();
            coverAuditRepository = Substitute.For<ICoverAuditRepository>();

            coverAuditRepository.AddAsync(Arg.Any<CoverAudit>()).Returns(new CoverAudit());

            return new CoverAuditFunction(logger, coverAuditRepository);
        }
    }
}
