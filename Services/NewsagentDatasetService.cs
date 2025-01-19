using AssessmentAPI.Contracts;
using AssessmentAPI.Data;
using AssessmentAPI.Models;
using System.Net.Http;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AssessmentAPI.Services
{
    public class NewsagentDatasetService : INewsagentDatasetService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public NewsagentDatasetService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<ZineCoNewsagent>> GetZineCoDatasetAsync()
        {
            if (!ChainUrls.Urls.TryGetValue(string.Empty, out var url))
            {
                throw new ArgumentException($"Error retrieving ZineCo dataset!");
            }


            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Following response code received while trying to retrive ZineCo dataset: {response.StatusCode}");
            }


            var chainAgents = await response.Content.ReadFromJsonAsync<List<ZineCoNewsagent>>();
            if (chainAgents == null)
            {
                throw new InvalidOperationException("No data returned from the chain API.");
            }

            return chainAgents;
        }

        public async Task<List<Newsagent>> GetChainDatasetAsync(string chainId)
        {
            if (!ChainUrls.Urls.TryGetValue(chainId, out var url))
            {
                throw new ArgumentException($"Invalid chain ID: {chainId}");
            }

            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to fetch chain agents: {response.StatusCode}");
            }

            var chainAgents = await response.Content.ReadFromJsonAsync<List<Newsagent>>();
            return chainAgents ?? new List<Newsagent>();
        }
    }
}

