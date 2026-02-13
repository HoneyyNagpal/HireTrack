using HireTrack.Core.DTOs;
using HireTrack.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HireTrack.API.Controllers
{
    [ApiController]
    [Route("api/interviews")]
    [Authorize]
    public class InterviewsController : ControllerBase
    {
        private readonly InterviewService _service;

        public InterviewsController(InterviewService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> Schedule([FromBody] ScheduleInterviewRequest req)
        {
            var (success, message, data) = await _service.ScheduleAsync(req);
            if (!success) return BadRequest(new { message });
            return StatusCode(201, data);
        }

        [HttpGet("candidate/{candidateId}")]
        public async Task<IActionResult> GetByCandidate(int candidateId)
        {
            var result = await _service.GetByCandidateAsync(candidateId);
            return Ok(result);
        }

        [HttpPost("{id}/feedback")]
        [Authorize(Roles = "Admin,HR,Interviewer")]
        public async Task<IActionResult> SubmitFeedback(int id, [FromBody] SubmitFeedbackRequest req)
        {
            var (success, message) = await _service.SubmitFeedbackAsync(id, req);
            if (!success) return BadRequest(new { message });
            return Ok(new { message });
        }

        [HttpGet("today")]
        public async Task<IActionResult> GetToday()
        {
            var result = await _service.GetTodayAsync();
            return Ok(result);
        }
    }
}
