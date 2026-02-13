using HireTrack.Core.Models;
using HireTrack.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HireTrack.API.Controllers
{
    [ApiController]
    [Route("api/dashboard")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _ctx;

        public DashboardController(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet("metrics")]
        public async Task<IActionResult> GetMetrics()
        {
            var total = await _ctx.Candidates.CountAsync();
            var shortlisted = await _ctx.Candidates.CountAsync(c => c.Status == CandidateStatus.Shortlisted);
            var hired = await _ctx.Candidates.CountAsync(c => c.Status == CandidateStatus.Hired);
            var rejected = await _ctx.Candidates.CountAsync(c => c.Status == CandidateStatus.Rejected);

            var today = DateTime.UtcNow.Date;
            var interviewsToday = await _ctx.Interviews.CountAsync(i =>
                i.ScheduledAt.Date == today && i.Status == InterviewStatus.Scheduled);

            var pendingInterviews = await _ctx.Interviews.CountAsync(i => i.Status == InterviewStatus.Scheduled);

            return Ok(new
            {
                totalCandidates = total,
                shortlisted,
                hired,
                rejected,
                shortlistRatio = total > 0 ? Math.Round((double)shortlisted / total * 100, 1) : 0,
                interviewsScheduledToday = interviewsToday,
                pendingInterviews
            });
        }

        [HttpGet("by-position")]
        public async Task<IActionResult> GetByPosition()
        {
            var data = await _ctx.Candidates
                .GroupBy(c => c.Position)
                .Select(g => new { position = g.Key, count = g.Count() })
                .OrderByDescending(x => x.count)
                .ToListAsync();

            return Ok(data);
        }

        [HttpGet("by-status")]
        public async Task<IActionResult> GetByStatus()
        {
            var data = await _ctx.Candidates
                .GroupBy(c => c.Status)
                .Select(g => new { status = g.Key.ToString(), count = g.Count() })
                .ToListAsync();

            return Ok(data);
        }
    }
}
