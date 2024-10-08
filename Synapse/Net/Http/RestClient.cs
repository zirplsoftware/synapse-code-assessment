﻿using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Synapse.Net.Http
{
    // NOTE: in a real application, this would be a much more robust set of classes
    // but this suffices for the assignment
    public class RestClient : IDisposable
    {
        // NOTE: I use Cancellation tokens for HTTP calls.
        // It's a good practice to do so
        public async Task<T> GetAndParseResponseAsJsonAsync<T>(string uri, CancellationToken? cancellationToken = null)
        {
            // BUG: try/catch should be around entire method
            // BUG: check for non-null responseContent before deserializing. return default(T) if null
            
            // TODO: see microsoft's documentation on HttpClient instantiation
            // this approach can result in port exhaustion
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(uri, cancellationToken ?? CancellationToken.None);

                try
                {
                    response.EnsureSuccessStatusCode();
                }
                catch (Exception ex)
                {
                    throw new RestClientRequestException($"Unexpected status code from GET request to {uri}", ex);
                }

                var responseContent = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<T>(responseContent);
            }
        }

        public async Task<T> PostAsJsonAndParseResponseAsync<T>(string uri, T requestData, CancellationToken? cancellationToken = null)
        {
            // BUG: try/catch should be around entire method
            // BUG: check for non-null responseContent before deserializing. return default(T) if null

            // NOTE: same note as above about HttpClient instantiation
            using (var httpClient = new HttpClient())
            {
                var requestContentBody = requestData != null
                    ? JsonConvert.SerializeObject(requestData)
                    : null;
                var httpContent = new StringContent(requestContentBody, System.Text.Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(uri, httpContent, cancellationToken ?? CancellationToken.None);
                
                try
                {
                    response.EnsureSuccessStatusCode();
                }
                catch (Exception ex)
                {
                    throw new RestClientRequestException($"Unexpected status code from POST request to {uri}", ex);
                }

                var responseContent = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<T>(responseContent);
            }
        }

        public void Dispose()
        {
            // NOTE: in a real application, this class would likely be
            // architected differently and this would be needed.
        }
    }
}
