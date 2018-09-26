using CandidateManager.Core.Models;
using CandidateManager.Web.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace CandidateManager.Web.ViewModels
{
    public class SessionViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Candidate")]
        public int CandidateId { get; set; }

        [Required]
        [Display(Name = "Exercise")]
        public int ExerciseId { get; set; }

        [Required]
        [Display(Name = "Available From")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? AvailableFrom { get; set; }

        [Required]
        [Display(Name = "Available To")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? AvailableTo { get; set; }

        [Required]
        [Display(Name = "Max. Duration (Hours)")]
        public int? MaxDuration { get; set; }

        [Required]
        [ReadOnly]
        public SessionStatus Status { get; set; }

        [ReadOnly]
        [Display(Name = "Started At")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? StartedAt { get; set; }

        [ReadOnly]
        [Display(Name = "Submitted At")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? SubmittedAt { get; set; }

        [Display(Name = "File Name")]
        public string FileName { get; set; }
        public HttpPostedFileBase File { get; set; }

        //Related ViewModels
        public CandidateViewModel Candidate { get; set; }
        public ExerciseViewModel Exercise { get; set; }

        //Calculated Properties
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? ExpiredAt
        {
            get
            {
                if (StartedAt != null && MaxDuration != null)
                {
                    return StartedAt.Value.AddHours(MaxDuration.Value);
                }
                return null;
            }
        }

        public TimeSpan? RemainingDuration
        {
            get
            {
                if (ExpiredAt != null)
                {
                    return (ExpiredAt.Value - DateTime.Now);
                }
                return null;
            }
        }

        public int? RemainingDurationHours
        {
            get
            {
                if (RemainingDuration != null)
                {
                    return (int)Math.Floor(RemainingDuration.Value.TotalHours);
                }
                return null;
            }
        }

        public int? RemainingDurationMinutes
        {
            get
            {
                if (RemainingDuration != null)
                {
                    return RemainingDuration.Value.Minutes;
                }
                return null;
            }
        }
    }
}
