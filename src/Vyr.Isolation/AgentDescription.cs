namespace Vyr.Isolation
{
    public class AgentDescription
    {
        public AgentDescription(string assembly, string type)
        {
            this.Assembly = assembly;
            this.Type = type;
        }

        public string Assembly { get; }

        public string Type { get; }
    }
}
