using CandidateManager.Core.DAL.Repositories;
using CandidateManager.Core.Models;
using CandidateManager.Core.Utils;
using CandidateManager.DAL.Entities;
using System;
using System.Data.Entity;

namespace CandidateManager.DAL.Repositories
{
    public class SessionsRepository : CrudRepository<SessionModel, Guid, SessionEntity>, ISessionsRepository
    {
        public SessionsRepository(IMapper<SessionModel, SessionEntity> mapper, DbContext context)
            : base(mapper, context)
        {
        }

        public new SessionModel Update(SessionModel model)
        {
            var id = GetKeyValue(model);
            var modelEntity = _mapper.Map(model);
            var entity = _context.Set<SessionEntity>().Find(id);
            if (modelEntity.FileName == null || modelEntity.FileData == null)
            {
                modelEntity.FileName = entity.FileName;
                modelEntity.FileData = entity.FileData;
            }
            _context.Entry(entity).CurrentValues.SetValues(modelEntity);
            _context.SaveChanges();
            return _mapper.Map(entity);
        }

        protected override Guid GetKeyValue(SessionModel model)
        {
            return model.Id;
        }
    }
}
