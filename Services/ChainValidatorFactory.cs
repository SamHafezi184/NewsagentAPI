using AssessmentAPI.Contracts;

namespace AssessmentAPI.Services
{
    public class ChainValidatorFactory
    {
        private readonly IEnumerable<IChainValidator> _validators;

        public ChainValidatorFactory(IEnumerable<IChainValidator> validators)
        {
            _validators = validators;
        }

        public IChainValidator? GetValidator(string chainId)
        {
            return _validators.FirstOrDefault(v => v.ChainId == chainId);
        }
    }
}
