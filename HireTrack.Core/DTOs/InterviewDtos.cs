using HireTrack.Core.Models;

namespace HireTrack.Core.DTOs
{
    public class ScheduleInterviewRequest
    {
        public int CandidateId { get; set; }
        public string InterviewerName { get; set; } = string.Empty;
        public DateTime ScheduledAt { get; set; }
        public InterviewType Type { get; set; }
    }

    public class SubmitFeedbackRequest
    {
        public string Feedback { get; set; } = string.Empty;
        public int Rating { get; set; }  // 1-5
    }

    public class InterviewResponse
    {
        public int Id { get; set; }
        public int CandidateId { get; set; }
        public string CandidateName { get; set; } = string.Empty;
        public string InterviewerName { get; set; } = string.Empty;
        public DateTime ScheduledAt { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? Feedback { get; set; }
        public int? Rating { get; set; }
    }
}
