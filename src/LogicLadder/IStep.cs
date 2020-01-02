namespace LogicLadder
{
    using System.Threading.Tasks;

    public interface IStep<T>
    {
        bool ContinueOnError { get; }
        Task<T> RunStep(T context);
    }
}