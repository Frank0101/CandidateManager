using AutoMapper;
using CandidateManager.Core.DAL.Repositories;
using CandidateManager.Core.Models;
using CandidateManager.Core.Utils;
using CandidateManager.Infra.Utils;
using CandidateManager.Web.ViewModels;
using System.IO;
using System.Web;

namespace CandidateManager.Web.Utils
{
    public class SessionViewModelMapper : Mapper<SessionModel, SessionViewModel>
    {
        private readonly IMapper<CandidateModel, CandidateViewModel> _candidateMapper;
        private readonly IMapper<ExerciseModel, ExerciseViewModel> _exerciseMapper;
        private readonly ICandidatesRepository _candidatesRepository;
        private readonly IExercisesRepository _exercisesRepository;

        public SessionViewModelMapper(
            IMapper<CandidateModel, CandidateViewModel> candidateMapper,
            IMapper<ExerciseModel, ExerciseViewModel> exerciseMapper,
            ICandidatesRepository candidatesRepository,
            IExercisesRepository exercisesRepository)
        {
            _candidateMapper = candidateMapper;
            _exerciseMapper = exerciseMapper;
            _candidatesRepository = candidatesRepository;
            _exercisesRepository = exercisesRepository;

            Mapper.Configuration.AllowNullCollections = true;
            Mapper.CreateMap<SessionModel, SessionViewModel>()
                .ForMember(dest => dest.Candidate, opts => opts.MapFrom(src =>
                    _candidateMapper.Map(_candidatesRepository.GetById(src.CandidateId))))
                .ForMember(dest => dest.Exercise, opts => opts.MapFrom(src =>
                    _exerciseMapper.Map(_exercisesRepository.GetById(src.ExerciseId))));
            Mapper.CreateMap<SessionViewModel, SessionModel>()
                .ForMember(dest => dest.FileName, opts => opts.MapFrom(src =>
                    ExtractFileName(src.File)))
                .ForMember(dest => dest.FileData, opts => opts.MapFrom(src =>
                    ExtractFileData(src.File)));
        }

        private static string ExtractFileName(HttpPostedFileBase file)
        {
            return file != null ? file.FileName : null;
        }

        private static byte[] ExtractFileData(HttpPostedFileBase file)
        {
            if (file == null) return null;
            using (var memoryStream = new MemoryStream())
            {
                file.InputStream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
