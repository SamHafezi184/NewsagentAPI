using AssessmentAPI.Models;

namespace AssessmentAPI.Contracts
{
    public interface IChainValidator
    {
        string ChainId { get; }
        Task<bool> ValidateAsync(ZineCoNewsagent agent);
    }
}
