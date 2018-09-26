using CandidateManager.Core.DAL.Repositories;
using CandidateManager.Core.Models;
using CandidateManager.Core.Services;
using CandidateManager.Core.Utils;
using CandidateManager.DAL.Entities;
using CandidateManager.DAL.Repositories;
using CandidateManager.Infra.Services;
using CandidateManager.Infra.Utils;
using CandidateManager.Web.Builders;
using CandidateManager.Web.Utils;
using CandidateManager.Web.ViewModels;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System;
using System.Configuration;
using System.Web.Mvc;

namespace CandidateManager.Web.Infrastructure
{
    public class CastleInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For(typeof(IMapper<,>)).ImplementedBy(typeof(Mapper<,>)),
                Component.For<IMapper<ExerciseModel, ExerciseViewModel>>().ImplementedBy<ExerciseViewModelMapper>(),
                Component.For<IMapper<SessionModel, SessionViewModel>>().ImplementedBy<SessionViewModelMapper>(),

                Component.For<IMapperOneWay<CandidateModel, SelectListItem>>().ImplementedBy<CandidateListItemMapper>(),
                Component.For<IMapperOneWay<ExerciseModel, SelectListItem>>().ImplementedBy<ExerciseListItemMapper>(),

                Component.For<ICandidatesRepository>().ImplementedBy<CandidatesRepository>().DependsOn(new
                {
                    context = new CandidateManagerContext()
                }),
                Component.For<IExercisesRepository>().ImplementedBy<ExercisesRepository>().DependsOn(new
                {
                    context = new CandidateManagerContext()
                }),
                Component.For<ISessionsRepository>().ImplementedBy<SessionsRepository>().DependsOn(new
                {
                    context = new CandidateManagerContext()
                }),

                Component.For<IAssessmentStatusService>().ImplementedBy<AssessmentStatusService>(),

                Component.For<ISessionPublishedEmailBuilder>().ImplementedBy<SessionPublishedEmailBuilder>().DependsOn(new
                {
                    emailFrom = ConfigurationManager.AppSettings["emailServiceSender"],
                    emailSubject = ConfigurationManager.AppSettings["sessionPublishedEmailSubject"],
                    emailBody = ConfigurationManager.AppSettings["sessionPublishedEmailBody"]
                }),
                Component.For<ISessionStartedEmailBuilder>().ImplementedBy<SessionStartedEmailBuilder>().DependsOn(new
                {
                    emailFrom = ConfigurationManager.AppSettings["emailServiceSender"],
                    emailTo = ConfigurationManager.AppSettings["emailServiceRecipient"],
                    emailSubject = ConfigurationManager.AppSettings["sessionStartedEmailSubject"],
                    emailBody = ConfigurationManager.AppSettings["sessionStartedEmailBody"]
                }),
                Component.For<ISessionSubmittedEmailBuilder>().ImplementedBy<SessionSubmittedEmailBuilder>().DependsOn(new
                {
                    emailFrom = ConfigurationManager.AppSettings["emailServiceSender"],
                    emailTo = ConfigurationManager.AppSettings["emailServiceRecipient"],
                    emailSubject = ConfigurationManager.AppSettings["sessionSubmittedEmailSubject"],
                    emailBody = ConfigurationManager.AppSettings["sessionSubmittedEmailBody"]
                }),
                Component.For<IEmailService>().ImplementedBy<Infra.Services.EmailService>().DependsOn(new
                {
                    host = ConfigurationManager.AppSettings["emailServiceHost"],
                    port = Convert.ToInt32(ConfigurationManager.AppSettings["emailServicePort"])
                }));

            //Register all the MVC controllers
            //in the current executing assembly
            container.Register(Classes.FromThisAssembly().BasedOn<Controller>().LifestylePerWebRequest());
        }
    }
}
