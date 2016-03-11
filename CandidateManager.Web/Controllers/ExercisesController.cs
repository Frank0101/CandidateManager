using CandidateManager.Core.DAL.Repositories;
using CandidateManager.Core.Models;
using CandidateManager.Core.Utils;
using CandidateManager.Web.ViewModels;
using System.Linq;
using System.Net.Mime;
using System.Web.Mvc;

namespace CandidateManager.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ExercisesController : Controller
    {
        private readonly IMapper<ExerciseModel, ExerciseViewModel> _mapper;
        private readonly IExercisesRepository _exercisesRepository;

        public ExercisesController(IMapper<ExerciseModel, ExerciseViewModel> mapper,
            IExercisesRepository exercisesRepository)
        {
            _mapper = mapper;
            _exercisesRepository = exercisesRepository;
        }

        public ActionResult List()
        {
            return View(_exercisesRepository.GetAll().Select(_mapper.Map).ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ExerciseViewModel exercise)
        {
            try
            {
                _exercisesRepository.AddNew(_mapper.Map(exercise));
                return RedirectToAction("List");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Detail(int id)
        {
            return View(_mapper.Map(_exercisesRepository.GetById(id)));
        }

        [HttpPost]
        public ActionResult Detail(ExerciseViewModel exercise)
        {
            try
            {
                _exercisesRepository.Update(_mapper.Map(exercise));
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
                _exercisesRepository.Delete(id);
                return RedirectToAction("List");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Download(int id)
        {
            var exercise = _exercisesRepository.GetById(id);
            return File(exercise.FileData, MediaTypeNames.Application.Octet,
                exercise.FileName);
        }
    }
}
