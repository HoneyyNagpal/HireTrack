using HireTrack.Core.Interfaces;
using HireTrack.Core.Models;
using HireTrack.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HireTrack.Infrastructure.Repositories
{
    public class CandidateRepository : ICandidateRepository
    {
        private readonly AppDbContext _ctx;

        public CandidateRepository(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<List<Candidate>> GetAllAsync(string? search, CandidateStatus? status)
        {
            var query = _ctx.Candidates.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(c => c.FullName.Contains(search) || c.Position.Contains(search));

            if (status.HasValue)
                query = query.Where(c => c.Status == status.Value);

            return await query.OrderByDescending(c => c.AppliedAt).ToListAsync();
        }

        public async Task<Candidate?> GetByIdAsync(int id)
            => await _ctx.Candidates.Include(c => c.Interviews).FirstOrDefaultAsync(c => c.Id == id);

        public async Task<Candidate?> GetByEmailAsync(string email)
            => await _ctx.Candidates.FirstOrDefaultAsync(c => c.Email == email);

        public async Task AddAsync(Candidate candidate)
        {
            _ctx.Candidates.Add(candidate);
            await _ctx.SaveChangesAsync();
        }

        public async Task UpdateAsync(Candidate candidate)
        {
            candidate.UpdatedAt = DateTime.UtcNow;
            _ctx.Candidates.Update(candidate);
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(Candidate candidate)
        {
            _ctx.Candidates.Remove(candidate);
            await _ctx.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
            => await _ctx.Candidates.AnyAsync(c => c.Id == id);
    }
}
