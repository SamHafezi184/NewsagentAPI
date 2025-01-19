using AssessmentAPI.Contracts;
using AssessmentAPI.Models;

namespace AssessmentAPI.Services
{
    public class AdventureNewsValidator : IChainValidator
    {
        private static readonly double MaxDistanceMeters = 100;
        private readonly INewsagentDatasetService _datasetService;

        public AdventureNewsValidator(INewsagentDatasetService datasetService)
        {
            _datasetService = datasetService;
        }

        public string ChainId => "ADV";


        public async Task<bool> ValidateAsync(ZineCoNewsagent agent)
        {
            var chainAgents = await _datasetService.GetChainDatasetAsync(ChainId);
            var chainAgent = chainAgents.FirstOrDefault(c =>
            agent.Name.Equals(c.Name, StringComparison.OrdinalIgnoreCase) &&
            HaversineDistance(agent.Latitude, agent.Longitude, c.Latitude, c.Longitude) <= MaxDistanceMeters);
            
            if (chainAgent == null)
            {
                return false;            
            
            }
            bool isValid = agent.Name.Equals(chainAgent.Name, StringComparison.Ordinal) &&
                           HaversineDistance(agent.Latitude, agent.Longitude, chainAgent.Latitude, chainAgent.Longitude) <= MaxDistanceMeters;

            return isValid;
        }

        //The HaversineDistance method is generated using ChatGPT
        private static double HaversineDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371e3; // Earth's radius in meters
            var φ1 = lat1 * Math.PI / 180;
            var φ2 = lat2 * Math.PI / 180;
            var Δφ = (lat2 - lat1) * Math.PI / 180;
            var Δλ = (lon2 - lon1) * Math.PI / 180;

            var a = Math.Sin(Δφ / 2) * Math.Sin(Δφ / 2) +
                    Math.Cos(φ1) * Math.Cos(φ2) *
                    Math.Sin(Δλ / 2) * Math.Sin(Δλ / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c;
        }
    }
}
