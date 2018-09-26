using CandidateManager.Core.DAL.Repositories;
using CandidateManager.Core.Models;
using CandidateManager.Core.Services;
using CandidateManager.Core.Utils;
using CandidateManager.Web.Builders;
using CandidateManager.Web.ViewModels;
using System;
using System.Linq;
using System.Net.Mime;
using System.Web.Mvc;

namespace CandidateManager.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SessionsController : Controller
    {
        private readonly IMapper<SessionModel, SessionViewModel> _mapper;
        private readonly IMapperOneWay<CandidateModel, SelectListItem> _candidateListItemMapper;
        private readonly IMapperOneWay<ExerciseModel, SelectListItem> _exerciseListItemMapper;
        private readonly ICandidatesRepository _candidatesRepository;
        private readonly IExercisesRepository _exercisesRepository;
        private readonly ISessionsRepository _sessionsRepository;
        private readonly ISessionPublishedEmailBuilder _sessionPublishedEmailBuilder;
        private readonly IEmailService _emailService;

        public SessionsController(IMapper<SessionModel, SessionViewModel> mapper,
            IMapperOneWay<CandidateModel, SelectListItem> candidateListItemMapper,
            IMapperOneWay<ExerciseModel, SelectListItem> exerciseListItemMapper,
            ICandidatesRepository candidatesRepository,
            IExercisesRepository exercisesRepository,
            ISessionsRepository sessionsRepository,
            ISessionPublishedEmailBuilder sessionPublishedEmailBuilder,
            IEmailService emailService)
        {
            _mapper = mapper;
            _candidateListItemMapper = candidateListItemMapper;
            _exerciseListItemMapper = exerciseListItemMapper;
            _candidatesRepository = candidatesRepository;
            _exercisesRepository = exercisesRepository;
            _sessionsRepository = sessionsRepository;
            _sessionPublishedEmailBuilder = sessionPublishedEmailBuilder;
            _emailService = emailService;
        }

        public ActionResult List()
        {
            return View(_sessionsRepository.GetAll().Select(_mapper.Map).ToList());
        }

        public ActionResult Create()
        {
            return View(new SessionFormViewModel
            {
                Candidates = _candidatesRepository.GetAll().Select(_candidateListItemMapper.Map),
                Exercises = _exercisesRepository.GetAll().Select(_exerciseListItemMapper.Map)
            });
        }

        public ActionResult CreateByCandidate(int id)
        {
            return View("Create", new SessionFormViewModel
            {
                Candidates = _candidatesRepository.GetAll().Select(_candidateListItemMapper.Map),
                Exercises = _exercisesRepository.GetAll().Select(_exerciseListItemMapper.Map),
                Session = new SessionViewModel
                {
                    CandidateId = id
                }
            });
        }

        [HttpPost]
        public ActionResult Create(SessionViewModel session)
        {
            try
            {
                _sessionsRepository.AddNew(_mapper.Map(session));
                return RedirectToAction("List");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Detail(Guid id)
        {
            return View(new SessionFormViewModel
            {
                Candidates = _candidatesRepository.GetAll().Select(_candidateListItemMapper.Map),
                Exercises = _exercisesRepository.GetAll().Select(_exerciseListItemMapper.Map),
                Session = _mapper.Map(_sessionsRepository.GetById(id))
            });
        }

        [HttpPost]
        public ActionResult Detail(SessionViewModel session)
        {
            try
            {
                _sessionsRepository.Update(_mapper.Map(session));
                return RedirectToAction("List");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(Guid id)
        {
            try
            {
                _sessionsRepository.Delete(id);
                return RedirectToAction("List");
            }
            catch
            {
                return RedirectToAction("List");
            }
        }

        public ActionResult Publish(Guid id)
        {
            var session = _sessionsRepository.GetById(id);
            if (session.Status == SessionStatus.Created)
            {
                PublishSession(session);
            }
            return RedirectToAction("List");
        }

        public ActionResult Download(Guid id)
        {
            var exercise = _sessionsRepository.GetById(id);
            return File(exercise.FileData, MediaTypeNames.Application.Octet,
                exercise.FileName);
        }

        private void PublishSession(SessionModel session)
        {
            session.Status = SessionStatus.Published;
            _sessionsRepository.Update(session);
            try
            {
                _emailService.SendEmail(_sessionPublishedEmailBuilder.Get(_mapper.Map(session)));
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Print(e.Message);
            }
        }
    }
}
