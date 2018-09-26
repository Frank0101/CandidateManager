using CandidateManager.Core.DAL.Repositories;
using CandidateManager.Core.Models;
using CandidateManager.Core.Utils;
using CandidateManager.DAL.Entities;
using System.Data.Entity;

namespace CandidateManager.DAL.Repositories
{
    public class CandidatesRepository : CrudRepository<CandidateModel, int, CandidateEntity>, ICandidatesRepository
    {
        public CandidatesRepository(IMapper<CandidateModel, CandidateEntity> mapper, DbContext context)
            : base(mapper, context)
        {
        }

        protected override int GetKeyValue(CandidateModel model)
        {
            return model.Id;
        }
    }
}
