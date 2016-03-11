using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CandidateManager.DAL.Entities
{
    [Table("Candidates")]
    public class CandidateEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string Surname { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(50)]
        public string Phone { get; set; }
        public string Notes { get; set; }
    }
}
