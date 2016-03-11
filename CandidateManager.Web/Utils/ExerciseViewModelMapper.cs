using AutoMapper;
using CandidateManager.Core.Models;
using CandidateManager.Infra.Utils;
using CandidateManager.Web.ViewModels;
using System.IO;
using System.Web;

namespace CandidateManager.Web.Utils
{
    public class ExerciseViewModelMapper : Mapper<ExerciseModel, ExerciseViewModel>
    {
        public ExerciseViewModelMapper()
        {
            Mapper.Configuration.AllowNullCollections = true;
            Mapper.CreateMap<ExerciseModel, ExerciseViewModel>();
            Mapper.CreateMap<ExerciseViewModel, ExerciseModel>()
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
