using CandidateManager.Core.Models;
using System.Data.Entity;

namespace CandidateManager.Infra.Entities
{
    public class CandidatesManagerContext : DbContext
    {
        public DbSet<CandidateModel> Candidates { get; set; }

        public CandidatesManagerContext(string connectionString)
            : base(connectionString)
        { }
    }
}
