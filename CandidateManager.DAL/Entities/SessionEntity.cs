using CandidateManager.Core.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CandidateManager.DAL.Entities
{
    [Table("Sessions")]
    public class SessionEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public int CandidateId { get; set; }

        [Required]
        public int ExerciseId { get; set; }

        [Required]
        public DateTime AvailableFrom { get; set; }

        [Required]
        public DateTime AvailableTo { get; set; }

        [Required]
        public int MaxDuration { get; set; }

        [Required]
        public SessionStatus Status { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? SubmittedAt { get; set; }

        [MaxLength(100)]
        public string FileName { get; set; }
        public byte[] FileData { get; set; }

        //Related Entities
        [ForeignKey("CandidateId")]
        public virtual CandidateEntity Candidate { get; set; }

        [ForeignKey("ExerciseId")]
        public virtual ExerciseEntity Exercise { get; set; }
    }
}
