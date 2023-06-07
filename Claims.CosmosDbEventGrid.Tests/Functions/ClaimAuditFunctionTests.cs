using Azure.Messaging.EventGrid;
using Claims.CosmosDbEventGrid.Entities;
using Claims.CosmosDbEventGrid.Functions;
using Claims.CosmosDbEventGrid.Interfaces.Repository;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace Claims.CosmosDbEventGrid.Tests.Functions
{
    internal class ClaimAuditFunctionTests
    {
        [TestCase("1", "POST")]
        [TestCase("2", "DELETE")]
        public async Task Should_Call_ClaimRepository(string id, string method)
        {
            var instance = CreateInstance(out var logger, out var claimAuditRepository);

            var data = new { ClaimId = id, HttpRequestType = method };

            var eventGrid = new EventGridEvent(string.Empty, string.Empty, string.Empty, data);

            await instance.Run(eventGrid);

            await claimAuditRepository.Received(1).AddAsync(Arg.Is<ClaimAudit>(x => x.ClaimId == id && x.HttpRequestType == method));
        }

        private ClaimAduitFunction CreateInstance(out ILogger<ClaimAduitFunction> logger, out IClaimAuditRepository claimAuditRepository)
        {
            logger = Substitute.For<ILogger<ClaimAduitFunction>>();
            claimAuditRepository = Substitute.For<IClaimAuditRepository>();

            claimAuditRepository.AddAsync(Arg.Any<ClaimAudit>()).Returns(new ClaimAudit());

            return new ClaimAduitFunction(logger, claimAuditRepository);
        }
    }
}
