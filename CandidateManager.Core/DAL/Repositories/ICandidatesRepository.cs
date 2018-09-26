using CandidateManager.Core.Models;

namespace CandidateManager.Core.DAL.Repositories
{
    public interface ICandidatesRepository : ICrudRepository<CandidateModel, int>
    {
    }
}
