using CandidateManager.Core.DAL.Repositories;
using CandidateManager.Core.Models;
using CandidateManager.Core.Utils;
using CandidateManager.Web.ViewModels;
using System.Linq;
using System.Web.Mvc;

namespace CandidateManager.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CandidatesController : Controller
    {
        private readonly IMapper<CandidateModel, CandidateViewModel> _mapper;
        private readonly ICandidatesRepository _candidatesRepository;

        public CandidatesController(IMapper<CandidateModel, CandidateViewModel> mapper,
            ICandidatesRepository candidatesRepository)
        {
            _mapper = mapper;
            _candidatesRepository = candidatesRepository;
        }

        public ActionResult List()
        {
            return View(_candidatesRepository.GetAll().Select(_mapper.Map).ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CandidateViewModel candidate)
        {
            try
            {
                _candidatesRepository.AddNew(_mapper.Map(candidate));
                return RedirectToAction("List");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Detail(int id)
        {
            return View(_mapper.Map(_candidatesRepository.GetById(id)));
        }

        [HttpPost]
        public ActionResult Detail(CandidateViewModel candidate)
        {
            try
            {
                _candidatesRepository.Update(_mapper.Map(candidate));
                return RedirectToAction("List");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                _candidatesRepository.Delete(id);
                return RedirectToAction("List");
            }
            catch
            {
                return RedirectToAction("List");
            }
        }
    }
}
