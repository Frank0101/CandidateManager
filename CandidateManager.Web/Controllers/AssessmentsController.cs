using CandidateManager.Core.DAL.Repositories;
using CandidateManager.Core.Models;
using CandidateManager.Core.Services;
using CandidateManager.Core.Utils;
using CandidateManager.Web.Builders;
using CandidateManager.Web.ViewModels;
using System;
using System.Net.Mime;
using System.Web.Mvc;

namespace CandidateManager.Web.Controllers
{
    public class AssessmentsController : Controller
    {
        private readonly IMapper<SessionModel, SessionViewModel> _mapper;
        private readonly IExercisesRepository _exercisesRepository;
        private readonly ISessionsRepository _sessionsRepository;
        private readonly IAssessmentStatusService _assessmentStatusService;
        private readonly ISessionStartedEmailBuilder _sessionStartedEmailBuilder;
        private readonly ISessionSubmittedEmailBuilder _sessionSubmittedEmailBuilder;
        private readonly IEmailService _emailService;

        public AssessmentsController(IMapper<SessionModel, SessionViewModel> mapper,
            IExercisesRepository exercisesRepository, ISessionsRepository sessionRepository,
            IAssessmentStatusService assessmentStatusService,
            ISessionStartedEmailBuilder sessionStartedEmailBuilder,
            ISessionSubmittedEmailBuilder sessionSubmittedEmailBuilder,
            IEmailService emailService)
        {
            _mapper = mapper;
            _exercisesRepository = exercisesRepository;
            _sessionsRepository = sessionRepository;
            _assessmentStatusService = assessmentStatusService;
            _sessionStartedEmailBuilder = sessionStartedEmailBuilder;
            _sessionSubmittedEmailBuilder = sessionSubmittedEmailBuilder;
            _emailService = emailService;
        }

        [Route("Assessments/{id}")]
        public ActionResult Index(Guid id)
        {
            return EvaluateAssessmentStatus(id, session => View(_mapper.Map(session)));
        }

        public ActionResult Download(Guid id)
        {
            return EvaluateAssessmentStatus(id, session =>
            {
                var exercise = _exercisesRepository.GetById(session.ExerciseId);
                if (session.Status == SessionStatus.Published)
                {
                    StartSession(session);
                }
                return File(exercise.FileData, MediaTypeNames.Application.Octet,
                    exercise.FileName);
            });
        }

        public ActionResult Submit(Guid id)
        {
            return EvaluateAssessmentStatus(id,
                session => View("CantSubmit", _mapper.Map(session)),
                session => View(_mapper.Map(session)));
        }

        [HttpPost]
        public ActionResult Submit(SessionViewModel submittedSession)
        {
            return EvaluateAssessmentStatus(submittedSession.Id,
                session => View("CantSubmit", _mapper.Map(session)),
                session =>
                {
                    SubmitSession(session, _mapper.Map(submittedSession));
                    return RedirectToAction("Index");
                });
        }

        private ActionResult EvaluateAssessmentStatus(Guid id,
            Func<SessionModel, ActionResult> statusValidCallback)
        {
            return EvaluateAssessmentStatus(id, statusValidCallback, statusValidCallback);
        }

        private ActionResult EvaluateAssessmentStatus(Guid id,
            Func<SessionModel, ActionResult> statusAvailableCallback,
            Func<SessionModel, ActionResult> statusStartedCallback)
        {
            var session = _sessionsRepository.GetById(id);
            var assessmentStatus = _assessmentStatusService.GetAssessmentStatus(session);
            switch (assessmentStatus)
            {
                case AssessmentStatus.Unavailable:
                    return HttpNotFound();
                case AssessmentStatus.OutOfRange:
                    return View("OutOfRange", _mapper.Map(session));
                case AssessmentStatus.Available:
                    return statusAvailableCallback(session);
                case AssessmentStatus.Started:
                    return statusStartedCallback(session);
                case AssessmentStatus.Expired:
                    return View("Expired", _mapper.Map(session));
                case AssessmentStatus.Submitted:
                    return View("Submitted", _mapper.Map(session));
            }
            return null;
        }

        private void StartSession(SessionModel session)
        {
            session.Status = SessionStatus.Started;
            session.StartedAt = DateTime.Now;
            _sessionsRepository.Update(session);
            try
            {
                _emailService.SendEmail(_sessionStartedEmailBuilder.Get(_mapper.Map(session)));
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Print(e.Message);
            }
        }

        private void SubmitSession(SessionModel session,
            SessionModel submittedSession)
        {
            session.Status = SessionStatus.Submitted;
            session.SubmittedAt = DateTime.Now;
            session.FileName = submittedSession.FileName;
            session.FileData = submittedSession.FileData;
            _sessionsRepository.Update(session);
            try
            {
                _emailService.SendEmail(_sessionSubmittedEmailBuilder.Get(_mapper.Map(session)));
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Print(e.Message);
            }
        }
    }
}
