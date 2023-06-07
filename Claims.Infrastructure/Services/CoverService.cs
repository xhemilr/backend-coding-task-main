using AutoMapper;
using Claims.Application.Exceptions;
using Claims.Application.Requests.Cover;
using Claims.Application.Responses;
using Claims.Application.Services;
using Claims.Core.Entities;
using Claims.Core.Enums;
using Claims.Core.Repository;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Claims.Infrastructure.Services
{
    public class CoverService : ICoverService
    {
        private readonly ILogger<CoverService> _logger;
        private readonly ICoverRepository _coverRepository;
        private readonly IPremiumCalculationService _premiumCalculationService;
        private readonly IMapper _mapper;
        private readonly HttpClient _auditHttpClient;
        private readonly IDateTimeService _dateTimeService;

        public CoverService(
            ILogger<CoverService> logger,
            ICoverRepository coverRepository,
            IPremiumCalculationService premiumCalculationService,
            IMapper mapper,
            IHttpClientFactory httpClientFactory,
            IDateTimeService dateTimeService)
        {
            _logger = logger;
            _coverRepository = coverRepository;
            _premiumCalculationService = premiumCalculationService;
            _mapper = mapper;
            _dateTimeService = dateTimeService;
            _auditHttpClient = httpClientFactory.CreateClient(nameof(Cover));
        }

        public async Task<CoverResponse> CreateAsync(CreateCoverRequest request)
        {
            if(request.StartDate < DateOnly.FromDateTime(_dateTimeService.GetUtcNow()))
                throw new InvalidDateException($"Cover cannot be in the past!");

            if ((request.EndDate.DayNumber - request.StartDate.DayNumber) > 365)
                throw new InvalidDateException($"Cover cannot exced 1 year!");

            var coverRequest = _mapper.Map<Cover>(request);

            var result = await _coverRepository.AddItemAsync(coverRequest);

            if (result.Id != null)
                new Task(async () => await TriggerCoverAuditFunction(result.Id, "POST")).Start();
            
            return _mapper.Map<CoverResponse>(result);
        }

        public async Task DeleteAsync(string id)
        {
            if (await GetByIdAsync(id) == null) 
                throw new EntityNotFoundException($"Could not find cover with id: {id}.");
            
            await _coverRepository.DeleteItemAsync(id);

            new Task(async () => await TriggerCoverAuditFunction(id, "DELETE")).Start();
        }

        public async Task<IEnumerable<CoverResponse>> GetAllAsync()
        {
            var response = await _coverRepository.GetAllAsync();
            return _mapper.Map<List<CoverResponse>>(response);
        }

        public async Task<CoverResponse> GetByIdAsync(string id)
        {
            var response = await _coverRepository.GetItemAsync(id);
            return _mapper.Map<CoverResponse>(response);
        }

        public Task<decimal> ComputePremiumAsync(DateOnly startDate, DateOnly endDate, CoverType coverType)
        {
            var result = _premiumCalculationService.CalculatePremium(startDate, endDate, coverType);
            return Task.FromResult(result);
        }

        private async Task TriggerCoverAuditFunction(string id, string requestType)
        {
            var content = new
            {
                id = string.Empty,
                subject = string.Empty,
                data = new
                {
                    CoverId = id,
                    HttpRequestType = requestType
                },
                eventType = string.Empty,
                dataVersion = "1",
                metadataVersion = "1",
                eventTime = _dateTimeService.GetUtcNow(),
            };

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Content = new StringContent(JsonSerializer.Serialize(content)),
            };
            try
            {
                var result = await _auditHttpClient.SendAsync(request);
                if (result.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Audit entry added successfully.");
                }
                else
                {
                    _logger.LogError($"Failed to add audit entry. Status code: {result.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown while adding audit entry. Error: {ex}");
            }

        }

    }
}
