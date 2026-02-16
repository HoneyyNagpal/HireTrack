using HireTrack.Core.DTOs;
using HireTrack.Core.Interfaces;
using HireTrack.Core.Models;

namespace HireTrack.Services
{
    public class InterviewService
    {
        private readonly IInterviewRepository _interviewRepo;
        private readonly ICandidateRepository _candidateRepo;

        public InterviewService(IInterviewRepository interviewRepo, ICandidateRepository candidateRepo)
        {
            _interviewRepo = interviewRepo;
            _candidateRepo = candidateRepo;
        }

        public async Task<(bool success, string message, InterviewResponse? data)> ScheduleAsync(ScheduleInterviewRequest req)
        {
            var candidate = await _candidateRepo.GetByIdAsync(req.CandidateId);
            if (candidate == null)
                return (false, "Candidate not found.", null);

            var interview = new Interview
            {
                CandidateId = req.CandidateId,
                InterviewerName = req.InterviewerName,
                ScheduledAt = req.ScheduledAt,
                Type = req.Type
            };

            await _interviewRepo.AddAsync(interview);

            // auto-update candidate status when interview is scheduled
            if (candidate.Status == CandidateStatus.Shortlisted || candidate.Status == CandidateStatus.Applied)
            {
                candidate.Status = CandidateStatus.InterviewScheduled;
                await _candidateRepo.UpdateAsync(candidate);
            }

            return (true, "Interview scheduled.", MapToResponse(interview, candidate.FullName));
        }

        public async Task<List<InterviewResponse>> GetByCandidateAsync(int candidateId)
        {
            var interviews = await _interviewRepo.GetByCandidateAsync(candidateId);
            return interviews.Select(i => MapToResponse(i, i.Candidate?.FullName ?? "")).ToList();
        }

        public async Task<(bool success, string message)> SubmitFeedbackAsync(int interviewId, SubmitFeedbackRequest req)
        {
            var interview = await _interviewRepo.GetByIdAsync(interviewId);
            if (interview == null)
                return (false, "Interview not found.");

            if (req.Rating < 1 || req.Rating > 5)
                return (false, "Rating must be between 1 and 5.");

            interview.Feedback = req.Feedback;
            interview.Rating = req.Rating;
            interview.Status = InterviewStatus.Completed;
            interview.CompletedAt = DateTime.UtcNow;

            await _interviewRepo.UpdateAsync(interview);
            return (true, "Feedback submitted.");
        }

        public async Task<List<InterviewResponse>> GetTodayAsync()
        {
            var interviews = await _interviewRepo.GetScheduledTodayAsync();
            return interviews.Select(i => MapToResponse(i, i.Candidate?.FullName ?? "")).ToList();
        }

        private static InterviewResponse MapToResponse(Interview i, string candidateName) => new()
        {
            Id = i.Id,
            CandidateId = i.CandidateId,
            CandidateName = candidateName,
            InterviewerName = i.InterviewerName,
            ScheduledAt = i.ScheduledAt,
            Type = i.Type.ToString(),
            Status = i.Status.ToString(),
            Feedback = i.Feedback,
            Rating = i.Rating
        };
    }
}
