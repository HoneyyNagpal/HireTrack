namespace HireTrack.Core.Models
{
    public class Candidate
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string ResumeUrl { get; set; } = string.Empty;
        public CandidateStatus Status { get; set; } = CandidateStatus.Applied;
        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public List<Interview> Interviews { get; set; } = new();
    }

    public enum CandidateStatus
    {
        Applied,
        Shortlisted,
        InterviewScheduled,
        Offered,
        Hired,
        Rejected
    }
}
