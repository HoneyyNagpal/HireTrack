namespace HireTrack.Core.Models
{
    public class Interview
    {
        public int Id { get; set; }
        public int CandidateId { get; set; }
        public Candidate Candidate { get; set; } = null!;

        public string InterviewerName { get; set; } = string.Empty;
        public DateTime ScheduledAt { get; set; }
        public InterviewType Type { get; set; }
        public InterviewStatus Status { get; set; } = InterviewStatus.Scheduled;

        public string? Feedback { get; set; }
        public int? Rating { get; set; }  // 1-5
        public DateTime? CompletedAt { get; set; }
    }

    public enum InterviewType
    {
        Phone,
        Technical,
        HR,
        Final
    }

    public enum InterviewStatus
    {
        Scheduled,
        Completed,
        Cancelled,
        NoShow
    }
}
