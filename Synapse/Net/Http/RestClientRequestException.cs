using System;

namespace Synapse.Net.Http
{
    // NOTE: in a real application this class would need to carry
    // data about the request and response
    // to be of greater use
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