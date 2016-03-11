using AutoMapper;
using CandidateManager.Core.Utils;

namespace CandidateManager.Infra.Utils
{
    public abstract class MapperOneWay<T1, T2> : IMapperOneWay<T1, T2>
    {
        public T2 Map(T1 model)
        {
            return Mapper.Map<T1, T2>(model);
        }
    }
}
