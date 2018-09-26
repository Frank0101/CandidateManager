using System.Collections.Generic;

namespace CandidateManager.Core.DAL.Repositories
{
    public interface ICrudRepository<T, in TK>
        where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(TK id);
        T AddNew(T model);
        T Update(T model);
        void Delete(TK id);
    }
}
