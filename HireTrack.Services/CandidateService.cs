using HireTrack.Core.DTOs;
using HireTrack.Core.Interfaces;
using HireTrack.Core.Models;

namespace HireTrack.Services
{
    public class CandidateService
    {
        private readonly ICandidateRepository _repo;

        public CandidateService(ICandidateRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<CandidateResponse>> GetAllAsync(string? search, CandidateStatus? status)
        {
            var candidates = await _repo.GetAllAsync(search, status);
            return candidates.Select(MapToResponse).ToList();
        }

        public async Task<CandidateResponse?> GetByIdAsync(int id)
        {
            var candidate = await _repo.GetByIdAsync(id);
            return candidate == null ? null : MapToResponse(candidate);
        }

        public async Task<(bool success, string message, CandidateResponse? data)> CreateAsync(CreateCandidateRequest req)
        {
            var existing = await _repo.GetByEmailAsync(req.Email);
            if (existing != null)
                return (false, "A candidate with this email already exists.", null);

            var candidate = new Candidate
            {
                FullName = req.FullName,
                Email = req.Email,
                Phone = req.Phone,
                Position = req.Position,
                ResumeUrl = req.ResumeUrl ?? string.Empty
            };

            await _repo.AddAsync(candidate);
            return (true, "Candidate added.", MapToResponse(candidate));
        }

        public async Task<(bool success, string message)> UpdateStatusAsync(int id, CandidateStatus newStatus)
        {
            var candidate = await _repo.GetByIdAsync(id);
            if (candidate == null)
                return (false, "Candidate not found.");

            candidate.Status = newStatus;
            await _repo.UpdateAsync(candidate);
            return (true, $"Status updated to {newStatus}.");
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var candidate = await _repo.GetByIdAsync(id);
            if (candidate == null) return false;

            await _repo.DeleteAsync(candidate);
            return true;
        }

        private static CandidateResponse MapToResponse(Candidate c) => new()
        {
            Id = c.Id,
            FullName = c.FullName,
            Email = c.Email,
            Phone = c.Phone,
            Position = c.Position,
            Status = c.Status.ToString(),
            AppliedAt = c.AppliedAt,
            InterviewCount = c.Interviews?.Count ?? 0
        };
    }
}
