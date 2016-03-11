using System;

namespace CandidateManager.Core.Models
{
    public class SessionModel
    {
        public Guid Id { get; set; }
        public int CandidateId { get; set; }
        public int ExerciseId { get; set; }
        public DateTime AvailableFrom { get; set; }
        public DateTime AvailableTo { get; set; }
        public int MaxDuration { get; set; }
        public SessionStatus Status { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public string FileName { get; set; }
        public byte[] FileData { get; set; }
    }
}
