using HireTrack.Core.Interfaces;
using HireTrack.Core.Models;
using HireTrack.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HireTrack.Infrastructure.Repositories
{
    public class InterviewRepository : IInterviewRepository
    {
        private readonly AppDbContext _ctx;

        public InterviewRepository(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<List<Interview>> GetByCandidateAsync(int candidateId)
            => await _ctx.Interviews
                .Where(i => i.CandidateId == candidateId)
                .OrderByDescending(i => i.ScheduledAt)
                .ToListAsync();

        public async Task<Interview?> GetByIdAsync(int id)
            => await _ctx.Interviews.Include(i => i.Candidate).FirstOrDefaultAsync(i => i.Id == id);

        public async Task<List<Interview>> GetScheduledTodayAsync()
        {
            var today = DateTime.UtcNow.Date;
            return await _ctx.Interviews
                .Include(i => i.Candidate)
                .Where(i => i.ScheduledAt.Date == today && i.Status == InterviewStatus.Scheduled)
                .ToListAsync();
        }

        public async Task AddAsync(Interview interview)
        {
            _ctx.Interviews.Add(interview);
            await _ctx.SaveChangesAsync();
        }

        public async Task UpdateAsync(Interview interview)
        {
            _ctx.Interviews.Update(interview);
            await _ctx.SaveChangesAsync();
        }
    }
}
