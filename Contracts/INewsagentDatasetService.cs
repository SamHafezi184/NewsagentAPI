using AssessmentAPI.Models;

namespace AssessmentAPI.Contracts
{
    public interface INewsagentDatasetService
    {
        Task<List<ZineCoNewsagent>> GetZineCoDatasetAsync();
        Task<List<Newsagent>> GetChainDatasetAsync(string chainId);
    }
}
