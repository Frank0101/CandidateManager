using CandidateManager.Core.DAL;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace CandidateManager.Infra.DAL
{
    public class CrudRepository<T, TK> : ICrudRepository<T, TK> where T : class
    {
        private readonly DbContext _context;

        public CrudRepository(DbContext context)
        {
            _context = context;
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public T GetById(TK id)
        {
            return _context.Set<T>().Find(id);
        }

        public T AddNew(T model)
        {
            throw new System.NotImplementedException();
        }

        public T Update(T model)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(TK id)
        {
            throw new System.NotImplementedException();
        }
    }
}
