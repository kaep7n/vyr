using System.Diagnostics;
using Xunit;

namespace Homematic.Api.Xml.Tests
{
    public class DebugOnlyFactAttribute : FactAttribute
    {
        public DebugOnlyFactAttribute()
        {
            if (!Debugger.IsAttached)
            {
                this.Skip = "This is only for manual testing purposes and should only run when explicitly debugged";
            }
        }
    }
}
