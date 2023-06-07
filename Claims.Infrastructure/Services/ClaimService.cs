using AutoMapper;
using Claims.Application.Exceptions;
using Claims.Application.Requests.Claim;
using Claims.Application.Responses;
using Claims.Application.Services;
using Claims.Core.Entities;
using Claims.Core.Repository;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Claims.Infrastructure.Services
{
    public class ClaimService : IClaimService
    {
        private readonly ILogger<ClaimService> _logger;
        private readonly IClaimRepository _claimRepository;
        private readonly ICoverRepository _coverRepository;
        private readonly IMapper _mapper;
        private readonly HttpClient _auditHttpClient;
        private readonly IDateTimeService _dateTimeService;

        public ClaimService(
            ILogger<ClaimService> logger, 
            IClaimRepository claimRepository,
            ICoverRepository coverRepository,
            IMapper mapper, 
            IHttpClientFactory httpClientFactory, 
            IDateTimeService dateTimeService)
        {
            _logger = logger;
            _claimRepository = claimRepository;
            _coverRepository = coverRepository;
            _mapper = mapper;
            _auditHttpClient = httpClientFactory.CreateClient(nameof(Claim));
            _dateTimeService = dateTimeService;
        }

        public async Task<ClaimResponse> CreateAsync(CreateClaimRequest request)
        {
            var cover = await _coverRepository.GetItemAsync(request.CoverId);
            
            if(cover == null)
                throw new EntityNotFoundException($"Could not find cover with id: {request.CoverId}");

            var requestDate = DateOnly.Parse(request.Created.Date.ToShortDateString());

            if (requestDate < cover.StartDate || requestDate > cover.EndDate)
                throw new InvalidDateException($"Created date must be within {cover.StartDate} and {cover.EndDate}");
            
            var claimRequest = _mapper.Map<Claim>(request);

            var result = await _claimRepository.AddItemAsync(claimRequest);

            if(result.Id != null)
            {
                new Task(async () => await TriggerClaimAuditFunction(result.Id, "POST")).Start();
            }

            return _mapper.Map<ClaimResponse>(result);
        }

        public async Task DeleteAsync(string id)
        {
            if(await GetByIdAsync(id) == null)
                throw new EntityNotFoundException($"Could not find claim with id: {id}.");
            
            await _claimRepository.DeleteItemAsync(id);
            
            new Task(async () => await TriggerClaimAuditFunction(id, "DELETE")).Start();
        }

        public async Task<IEnumerable<ClaimResponse>> GetAllAsync()
        {
            var response = await _claimRepository.GetAllAsync();
            return _mapper.Map<List<ClaimResponse>>(response);
        }

        public async Task<ClaimResponse> GetByIdAsync(string id)
        {
            var response = await _claimRepository.GetItemAsync(id);
            return _mapper.Map<ClaimResponse>(response);
        }

        private async Task TriggerClaimAuditFunction(string id, string requestType)
        {
            var content = new
            {
                id = string.Empty,
                subject = string.Empty,
                data = new
                {
                    ClaimId = id,
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
            catch(Exception ex)
            {
                _logger.LogError($"Exception throw while adding audit entry. Error: {ex}");
            }
            
        }
    }
}
