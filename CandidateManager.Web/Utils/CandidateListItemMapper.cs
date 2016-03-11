using AutoMapper;
using CandidateManager.Core.Models;
using CandidateManager.Infra.Utils;
using System.Globalization;
using System.Web.Mvc;

namespace CandidateManager.Web.Utils
{
    public class CandidateListItemMapper : MapperOneWay<CandidateModel, SelectListItem>
    {
        public CandidateListItemMapper()
        {
            Mapper.Configuration.AllowNullCollections = true;
            Mapper.CreateMap<CandidateModel, SelectListItem>()
                .ForMember(dest => dest.Text, opts => opts.MapFrom(src =>
                    string.Format("{0} {1}", src.Name, src.Surname)))
                .ForMember(dest => dest.Value, opts => opts.MapFrom(src =>
                    src.Id.ToString(CultureInfo.InvariantCulture)));
        }
    }
}
