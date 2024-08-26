using System;

namespace Synapse.Logging
{
    // This interface allows the underlying provider
    // (NLog, Serilog, Elmah, etc.) to be swapped out without
    // changing the code that uses it. 
    //
    // NOTE: in a real application, this would be a more complex interface
    public interface ILogger
    {
        void Log(string message);
        void Log(string message, Exception exception);
    }
}
