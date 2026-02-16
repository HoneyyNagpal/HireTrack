using HireTrack.Core.Models;

namespace HireTrack.Core.Interfaces
{
    public interface IInterviewRepository
    {
        Task<List<Interview>> GetByCandidateAsync(int candidateId);
        Task<Interview?> GetByIdAsync(int id);
        Task<List<Interview>> GetScheduledTodayAsync();
        Task AddAsync(Interview interview);
        Task UpdateAsync(Interview interview);
    }
}
