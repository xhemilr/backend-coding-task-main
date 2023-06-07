// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Azure.Messaging.EventGrid;
using Claims.CosmosDbEventGrid.Interfaces.Repository;
using System.Threading.Tasks;
using Claims.CosmosDbEventGrid.Entities;
using System.Text.Json;
using System;

namespace Claims.CosmosDbEventGrid.Functions
{
    public class ClaimAduitFunction
    {
        private readonly ILogger<ClaimAduitFunction> _logger;
        private readonly IClaimAuditRepository _claimAuditRepository;
        
        public ClaimAduitFunction(ILogger<ClaimAduitFunction> logger, IClaimAuditRepository claimAuditRepository)
        {
            _logger = logger;
            _claimAuditRepository = claimAuditRepository;
        }

        [FunctionName("ClaimAudit")]
        public async Task Run([EventGridTrigger] EventGridEvent eventGridEvent)
        {
            try
            {
                var claim = JsonSerializer.Deserialize<ClaimAudit>(eventGridEvent.Data);
                if (claim.ClaimId == null || claim.HttpRequestType == null)
                    throw new ArgumentException("Invalid Data.");
                claim.Created = DateTime.UtcNow;
                await _claimAuditRepository.AddAsync(claim);
                _logger.LogInformation("Audit record created successfully.");
            }
            catch(Exception ex)
            {
                _logger.LogError($"Exception thrown while creating audit record. Error: {ex.Message}");
            }
        }
    }
}
