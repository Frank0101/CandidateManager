using AutoMapper;
using CandidateManager.Core.Models;
using CandidateManager.Infra.Utils;
using System.Globalization;
using System.Web.Mvc;

namespace CandidateManager.Web.Utils
{
    public class ExerciseListItemMapper : MapperOneWay<ExerciseModel, SelectListItem>
    {
        public ExerciseListItemMapper()
        {
            Mapper.Configuration.AllowNullCollections = true;
            Mapper.CreateMap<ExerciseModel, SelectListItem>()
                .ForMember(dest => dest.Text, opts => opts.MapFrom(src =>
                    src.Name))
                .ForMember(dest => dest.Value, opts => opts.MapFrom(src =>
                    src.Id.ToString(CultureInfo.InvariantCulture)));
        }
    }
}
