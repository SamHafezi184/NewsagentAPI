using AssessmentAPI.Contracts;
using AssessmentAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace AssessmentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsagentController : ControllerBase
    {
        private readonly INewsagentMatcher _newsagentMatcher;
        private readonly INewsagentDatasetService _datasetService;

        public NewsagentController(INewsagentMatcher newsagentMatcher, INewsagentDatasetService datasetService)
        {
            _newsagentMatcher = newsagentMatcher;
            _datasetService = datasetService;
        }

        [HttpPost("validate")]
        public async Task<IActionResult> ValidateSingle([FromBody] ZineCoNewsagent agent)
        {
            var result = await _newsagentMatcher.ValidateNewsagentAsync(agent);
            return Ok(result);
        }

        [HttpGet("validate/{chainId}")]
        public async Task<IActionResult> ValidateAll(string chainId)
        {
            var results = await _newsagentMatcher.ValidateAllNewsagentsAsync(chainId);
            return Ok(results);
        }

        [HttpGet("validate/by-name/{name}")]
        public async Task<IActionResult> ValidateByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Name cannot be empty.");
            }

            var result = await _newsagentMatcher.ValidateNewsagentByNameAsync(name);
            return result.IsValid
                ? Ok(new { message = "Validation succeeded.", result })
                : BadRequest(new { message = "Validation failed.", result });
        }
    }
}
