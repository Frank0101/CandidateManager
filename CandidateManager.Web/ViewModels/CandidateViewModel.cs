using System.ComponentModel.DataAnnotations;

namespace CandidateManager.Web.ViewModels
{
    public class CandidateViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(\+\d+)?[ ]?\d+$", ErrorMessage = "Phone number is not valid")]
        public string Phone { get; set; }

        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }

        //Calculated Properties
        public string FullName
        {
            get { return string.Format("{0} {1}", Name, Surname); }
        }
    }
}
