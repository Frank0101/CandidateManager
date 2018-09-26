using CandidateManager.Core.DAL;
using CandidateManager.Core.Models;
using CandidateManager.Infra.Entities;
using System.Collections.Generic;
using System.Linq;

namespace CandidateManager.Infra.DAL
{
    public class CandidatesRepository : ICandidatesRepository
    {
        private readonly CandidatesManagerContext _context;

        public CandidatesRepository(CandidatesManagerContext context)
        {
            _context = context;
        }

        public IEnumerable<CandidateModel> GetAll()
        {
            return _context.Candidates.ToList();
        }

        public CandidateModel GetById(int id)
        {
            return _context.Candidates.Find(id);
        }

        public CandidateModel AddNew(CandidateModel model)
        {
            var candidate = _context.Candidates.Create();
            _context.Candidates.Add(candidate);
            _context.Entry(candidate).CurrentValues.SetValues(model);
            _context.SaveChanges();
            return candidate;
        }

        public CandidateModel Update(CandidateModel model)
        {
            var candidate = _context.Candidates.Find(model.Id);
            _context.Entry(candidate).CurrentValues.SetValues(model);
            _context.SaveChanges();
            return candidate;
        }

        public void Delete(int id)
        {
            var candidate = _context.Candidates.Find(id);
            _context.Candidates.Remove(candidate);
            _context.SaveChanges();
        }
    }
}
