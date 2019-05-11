namespace Vyr.Isolation
{
    public interface IIsolationStrategy
    {
        IIsolation Create();
    }
}
