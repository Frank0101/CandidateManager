using System.ComponentModel.DataAnnotations;
using System.Web;

namespace CandidateManager.Web.ViewModels
{
    public class ExerciseViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        [Display(Name = "File Name")]
        public string FileName { get; set; }
        public HttpPostedFileBase File { get; set; }
    }
}
