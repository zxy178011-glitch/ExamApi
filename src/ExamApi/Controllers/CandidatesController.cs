using ExamApi.Models;
using ExamApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExamApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CandidatesController : ControllerBase
{
    private readonly ICandidateService _candidateService;

    public CandidatesController(ICandidateService candidateService)
    {
        _candidateService = candidateService;
    }

    /// <summary>
    /// 生成随机考生列表并重新排列
    /// </summary>
    /// <param name="count">考生数量（最少 20）</param>
    [HttpGet("generate")]
    [ProducesResponseType(typeof(CandidateListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<CandidateListResponse> Generate([FromQuery] int count = 25)
    {
        var original = _candidateService.GenerateCandidates(count);
        var reordered = _candidateService.Reorder(original);

        return Ok(new CandidateListResponse
        {
            Original = original,
            Reordered = reordered
        });
    }

    /// <summary>
    /// 对提交的考生列表进行重新排列
    /// </summary>
    [HttpPost("reorder")]
    [ProducesResponseType(typeof(List<Candidate>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<List<Candidate>> Reorder([FromBody] List<Candidate> candidates)
    {
        if (candidates == null || candidates.Count == 0)
            return BadRequest(new { message = "考生列表不能为空。" });

        var reordered = _candidateService.Reorder(candidates);
        return Ok(reordered);
    }
}
