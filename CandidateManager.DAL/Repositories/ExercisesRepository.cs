using CandidateManager.Core.DAL.Repositories;
using CandidateManager.Core.Models;
using CandidateManager.Core.Utils;
using CandidateManager.DAL.Entities;
using System.Data.Entity;

namespace CandidateManager.DAL.Repositories
{
    public class ExercisesRepository : CrudRepository<ExerciseModel, int, ExerciseEntity>, IExercisesRepository
    {
        public ExercisesRepository(IMapper<ExerciseModel, ExerciseEntity> mapper, DbContext context)
            : base(mapper, context)
        {
        }

        public new ExerciseModel Update(ExerciseModel model)
        {
            var id = GetKeyValue(model);
            var modelEntity = _mapper.Map(model);
            var entity = _context.Set<ExerciseEntity>().Find(id);
            if (modelEntity.FileName == null || modelEntity.FileData == null)
            {
                modelEntity.FileName = entity.FileName;
                modelEntity.FileData = entity.FileData;
            }
            _context.Entry(entity).CurrentValues.SetValues(modelEntity);
            _context.SaveChanges();
            return _mapper.Map(entity);
        }

        protected override int GetKeyValue(ExerciseModel model)
        {
            return model.Id;
        }
    }
}
