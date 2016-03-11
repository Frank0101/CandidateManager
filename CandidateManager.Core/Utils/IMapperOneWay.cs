namespace CandidateManager.Core.Utils
{
    public interface IMapperOneWay<T1, T2>
    {
        T2 Map(T1 model);
    }
}
