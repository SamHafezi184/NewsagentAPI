using AssessmentAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace AssessmentAPI.Contracts
{
    public interface INewsagentMatcher
    {
        Task<ValidationResult> ValidateNewsagentAsync(ZineCoNewsagent agent);

        Task<List<ValidationResult>> ValidateAllNewsagentsAsync(string chainId);

        Task<ValidationResult> ValidateNewsagentByNameAsync(string name);
    }

}
