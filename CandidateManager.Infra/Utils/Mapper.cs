using AutoMapper;
using CandidateManager.Core.Utils;

namespace CandidateManager.Infra.Utils
{
    public class Mapper<T1, T2> : MapperOneWay<T1, T2>, IMapper<T1, T2>
    {
        public Mapper()
        {
            Mapper.Configuration.AllowNullCollections = true;
            Mapper.CreateMap<T1, T2>().ReverseMap();
        }

        public T1 Map(T2 model)
        {
            return Mapper.Map<T2, T1>(model);
        }
    }
}
