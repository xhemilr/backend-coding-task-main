using Azure.Messaging.EventGrid;
using Claims.CosmosDbEventGrid.Entities;
using Claims.CosmosDbEventGrid.Interfaces.Repository;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Text.Json;
using System;

namespace Claims.CosmosDbEventGrid.Functions
{
    public class CoverAuditFunction
    {
        private readonly ILogger<CoverAuditFunction> _logger;
        private readonly ICoverAuditRepository _coverAuditRepository;

        public CoverAuditFunction(ILogger<CoverAuditFunction> logger, ICoverAuditRepository coverAuditRepository)
        {
            _logger = logger;
            _coverAuditRepository = coverAuditRepository;
        }

        [FunctionName("CoverAudit")]
        public async Task Run([EventGridTrigger] EventGridEvent eventGridEvent)
        {
            try
            {
                var cover = JsonSerializer.Deserialize<CoverAudit>(eventGridEvent.Data);
                if (cover.CoverId == null || cover.HttpRequestType == null)
                    throw new ArgumentException("Invalid Data.");
                cover.Created = DateTime.Now;
                await _coverAuditRepository.AddAsync(cover);
                _logger.LogInformation("Audit record created successfully.");
            }
            catch(Exception ex)
            {
                _logger.LogError($"Exception thrown while creating audit record. Error: {ex.Message}");
            }
        }
    }
}
