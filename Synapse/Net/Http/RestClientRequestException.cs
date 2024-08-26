using System;

namespace Synapse.Net.Http
{
    public class RestClientRequestException : Exception
    {
        public RestClientRequestException()
        {
        }

        public RestClientRequestException(string message) : base(message)
        {
        }

        public RestClientRequestException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}