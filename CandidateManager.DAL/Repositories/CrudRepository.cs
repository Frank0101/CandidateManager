using CandidateManager.Core.DAL.Repositories;
using CandidateManager.Core.Utils;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace CandidateManager.DAL.Repositories
{
    public abstract class CrudRepository<T, TK, TE> : ICrudRepository<T, TK>
        where T : class
        where TE : class
    {
        protected readonly IMapper<T, TE> _mapper;
        protected readonly DbContext _context;

        protected CrudRepository(IMapper<T, TE> mapper, DbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<TE>().Select(_mapper.Map).ToList();
        }

        public T GetById(TK id)
        {
            return _mapper.Map(_context.Set<TE>().Find(id));
        }

        public T AddNew(T model)
        {
            var modelEntity = _mapper.Map(model);
            var entity = _context.Set<TE>().Create();
            _context.Set<TE>().Add(entity);
            _context.Entry(entity).CurrentValues.SetValues(modelEntity);
            _context.SaveChanges();
            return _mapper.Map(entity);
        }

        public T Update(T model)
        {
            var id = GetKeyValue(model);
            var modelEntity = _mapper.Map(model);
            var entity = _context.Set<TE>().Find(id);
            _context.Entry(entity).CurrentValues.SetValues(modelEntity);
            _context.SaveChanges();
            return _mapper.Map(entity);
        }

        public void Delete(TK id)
        {
            var entity = _context.Set<TE>().Find(id);
            _context.Set<TE>().Remove(entity);
            _context.SaveChanges();
        }

        protected abstract TK GetKeyValue(T model);
    }
}
