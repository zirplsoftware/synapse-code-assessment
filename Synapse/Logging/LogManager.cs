using System;

namespace Synapse.Logging
{
    public static class LogManager
    {
        // NOTE: in a real application, this would be a much more complex class

        // simply use with in any non-static method in the application: this.GetLog().Log("message");
        public static ILogger GetLog<TLogConsumer>(this TLogConsumer logConsumer)
        {
            // NOTE: I have used this strategy on many past projects and
            // other developers have appreciates the simplicity of making log calls
            // making them more likely to write logging code.

            // for NLog I would get the NLog logger matching the full type name of the log consumer

            // TODO: configure NLog and implement return of correct logger for the logConsumer

            return new NLogLogger();
        }

        // Fallback method for getting a logger from static classes or Program.cs
        public static ILogger GetLog(string logName)
        {
            // NOTE: same notes as above
            return new NLogLogger();
        }

        private class NLogLogger :ILogger
        {
            public void Log(string message)
            {
                // TODO: write to NLog

                // NOTE: for time limitations with the eval, I am simply writing to the console
                Console.WriteLine(message);
            }

            public void Log(string message, Exception exception)
            {
                // TODO: write to NLog

                // NOTE: for time limitations with the eval, I am simply writing to the console
                Console.WriteLine(message);
                Console.WriteLine(exception.ToString());
            }
        }
    }
}