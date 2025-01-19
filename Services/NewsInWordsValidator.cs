using AssessmentAPI.Contracts;
using AssessmentAPI.Models;

namespace AssessmentAPI.Services
{
    public class NewsInWordsValidator : IChainValidator
    {
        private readonly INewsagentDatasetService _datasetService;

        public NewsInWordsValidator(INewsagentDatasetService datasetService)
        {
            _datasetService = datasetService;
        }

        public string ChainId => "NIW";
        public async Task<bool> ValidateAsync(ZineCoNewsagent agent)
        {
            var chainAgents = await _datasetService.GetChainDatasetAsync(ChainId);
            var chainAgent = chainAgents.FirstOrDefault(c =>
            ReverseWords(agent.Name).Equals(c.Name, StringComparison.OrdinalIgnoreCase));
            
            if (chainAgent == null)
            {
                return false;
            }
            return ReverseWords(agent.Name).Equals(chainAgent.Name, StringComparison.OrdinalIgnoreCase);

        }

        private static string ReverseWords(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            return string.Join(" ", input.Split(' ').Reverse());
        }
    }
}
