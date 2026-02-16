using HireTrack.Core.Models;

namespace HireTrack.Core.Interfaces
{
    public interface ICandidateRepository
    {
        Task<List<Candidate>> GetAllAsync(string? search, CandidateStatus? status);
        Task<Candidate?> GetByIdAsync(int id);
        Task<Candidate?> GetByEmailAsync(string email);
        Task AddAsync(Candidate candidate);
        Task UpdateAsync(Candidate candidate);
        Task DeleteAsync(Candidate candidate);
        Task<bool> ExistsAsync(int id);
    }
}
