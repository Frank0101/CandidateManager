using System.Data.Entity;

namespace CandidateManager.DAL.Entities
{
    public class CandidateManagerContext : DbContext
    {
        public DbSet<CandidateEntity> Candidates { get; set; }
        public DbSet<ExerciseEntity> Exercises { get; set; }
        public DbSet<SessionEntity> Sessions { get; set; }

        public CandidateManagerContext()
            : base("CandidateManagerConString")
        {
        }
    }
}
