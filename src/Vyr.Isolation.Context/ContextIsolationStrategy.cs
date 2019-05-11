namespace Vyr.Isolation.Context
{
    public class ContextIsolationStrategy : IIsolationStrategy
    {
        private readonly string directory;

        public ContextIsolationStrategy(string directory)
        {
            this.directory = directory;
        }

        public IIsolation Create()
        {
            return new ContextIsolation(this.directory);
        }
    }
}
