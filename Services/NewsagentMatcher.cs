using AssessmentAPI.Contracts;
using AssessmentAPI.Data;
using AssessmentAPI.Models;
using ValidationResult = AssessmentAPI.Models.ValidationResult;

namespace AssessmentAPI.Services
{
    public class NewsagentMatcher : INewsagentMatcher
    {
        private readonly ChainValidatorFactory _validatorFactory;
        private readonly INewsagentDatasetService _newsagentDatasetService;


        public NewsagentMatcher(ChainValidatorFactory validatorFactory, INewsagentDatasetService newsagentDatasetService)
        {
            _validatorFactory = validatorFactory;
            _newsagentDatasetService = newsagentDatasetService;
        }

        public async Task<ValidationResult> ValidateNewsagentAsync(ZineCoNewsagent agent)
        {
            var validator = _validatorFactory.GetValidator(agent.ChainId);
            if (validator == null)
            {
                return new ValidationResult(false, $"No validator found for chain {agent.ChainId}");
            }

            bool isValid = await validator.ValidateAsync(agent);

            return new ValidationResult(isValid, isValid ? null : "Validation failed.");
        }

        public async Task<List<ValidationResult>> ValidateAllNewsagentsAsync(string chainId)
        {
            chainId = chainId.ToUpper();
            var zineCoAgents = await _newsagentDatasetService.GetZineCoDatasetAsync();
            var chainAgents = await _newsagentDatasetService.GetChainDatasetAsync(chainId);


            var filteredAgents = zineCoAgents.Where(agent => agent.ChainId == chainId).ToList();
            if (!filteredAgents.Any())
            {
                throw new InvalidOperationException($"No agents found for chain ID: {chainId}");
            }

            var validationResults = new List<ValidationResult>();

            foreach (var agent in filteredAgents)
            {
                var result = await ValidateNewsagentAsync(agent);
                validationResults.Add(result);
            }

            return validationResults;
        }

        public async Task<ValidationResult> ValidateNewsagentByNameAsync(string name)
        {
            var zineCoAgents = await _newsagentDatasetService.GetZineCoDatasetAsync();

            var agent = zineCoAgents.FirstOrDefault(a => a.Name.Equals((name), StringComparison.OrdinalIgnoreCase));

            if (agent == null)
            {
                return new ValidationResult(false, $"No newsagent found with name: {name}");
            }

            var validator = _validatorFactory.GetValidator(agent.ChainId);
            if (validator == null)
            {
                return new ValidationResult(false, $"No validator found for chain: {agent.ChainId}");
            }

            var isValid = await validator.ValidateAsync(agent);
            return new ValidationResult(isValid, isValid ? null : "Validation failed.");
        }

    }
}

