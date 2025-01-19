using AssessmentAPI.Contracts;
using AssessmentAPI.Models;

namespace AssessmentAPI.Services
{
    public class SuperNewsValidator : IChainValidator
    {
        public string ChainId => "SUP";

        private readonly INewsagentDatasetService _datasetService;

        public SuperNewsValidator(INewsagentDatasetService datasetService)
        {
            _datasetService = datasetService;
        }
        public async Task<bool> ValidateAsync(ZineCoNewsagent agent)
        {
            if (agent == null || string.IsNullOrWhiteSpace(agent.Name) || string.IsNullOrWhiteSpace(agent.Address1))
            {
                return false;
            }

            var chainAgents = await _datasetService.GetChainDatasetAsync(ChainId);
            var chainAgent = chainAgents.FirstOrDefault(c =>
                Normalize(c.Name).Equals(Normalize(agent.Name), StringComparison.OrdinalIgnoreCase) &&
                Normalize(c.Address1).Equals(Normalize(agent.Address1), StringComparison.OrdinalIgnoreCase));

            return chainAgent != null;
        }

        private static string Normalize(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;
            var result = new string(input.Where(c => !char.IsPunctuation(c) && !char.IsWhiteSpace(c))
                .ToArray()).Trim().ToLowerInvariant();
            return result;
        }

    }
}
