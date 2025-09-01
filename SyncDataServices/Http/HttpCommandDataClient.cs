using System.Text;
using System.Text.Json;
using PlatformService.DTOs;

namespace PlatformService.SyncDataServices.Http
{
    public class HttpCommandDataClient : ICommandDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<HttpCommandDataClient> _logger;

        public HttpCommandDataClient(HttpClient httpClient, IConfiguration configuration, ILogger<HttpCommandDataClient> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendPlatformToCommand(PlatformReadDTO platform)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(platform),
                Encoding.UTF8,
                "application/json");

            var commandServiceEndpoint = _configuration["CommandService"];

            try
            {
                var response = await _httpClient.PostAsync($"{commandServiceEndpoint}/api/c/platforms", httpContent);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Sync POST to CommandService was OK!");
                }
                else
                {
                   Console.WriteLine("Sync POST to CommandService was NOT OK! Status Code: {StatusCode}", response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not send synchronously to CommandService {JsonSerializer.Serialize(ex)}");
            }
        }
    }
}