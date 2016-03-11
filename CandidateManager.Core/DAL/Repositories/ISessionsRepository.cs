using CandidateManager.Core.Models;
using System;

namespace CandidateManager.Core.DAL.Repositories
{
    public interface ISessionsRepository : ICrudRepository<SessionModel, Guid>
    {
    }
}
