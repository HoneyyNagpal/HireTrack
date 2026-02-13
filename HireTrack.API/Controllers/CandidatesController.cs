using HireTrack.Core.DTOs;
using HireTrack.Core.Models;
using HireTrack.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HireTrack.API.Controllers
{
    [ApiController]
    [Route("api/candidates")]
    [Authorize]
    public class CandidatesController : ControllerBase
    {
        private readonly CandidateService _service;

        public CandidatesController(CandidateService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] CandidateStatus? status)
        {
            var result = await _service.GetAllAsync(search, status);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var candidate = await _service.GetByIdAsync(id);
            if (candidate == null) return NotFound(new { message = "Candidate not found" });
            return Ok(candidate);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> Create([FromBody] CreateCandidateRequest req)
        {
            var (success, message, data) = await _service.CreateAsync(req);
            if (!success) return Conflict(new { message });
            return CreatedAtAction(nameof(GetById), new { id = data!.Id }, data);
        }

        [HttpPatch("{id}/status")]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> UpdateStatus(int id, [FromQuery] CandidateStatus status)
        {
            var (success, message) = await _service.UpdateStatusAsync(id, status);
            if (!success) return NotFound(new { message });
            return Ok(new { message });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound(new { message = "Candidate not found" });
            return NoContent();
        }
    }
}
