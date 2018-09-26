namespace CandidateManager.Core.Utils
{
    public interface IMapper<T1, T2> : IMapperOneWay<T1, T2>
    {
        T1 Map(T2 model);
    }
}
