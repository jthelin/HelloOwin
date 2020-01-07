using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations; 
using Newtonsoft.Json;

using Hello.Owin.Interfaces;

namespace Hello.Owin.Client
{
    public class HelloOwinClient
    {
        [PublicAPI]
        public bool Verbose = true;

        private readonly HttpClient _httpClient;

        public HelloOwinClient()
        { }

        public HelloOwinClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<int> Run(HelloOwinClientArguments progArgs, TextWriter traceWriter)
        {
            string address = progArgs.Address;
            bool useJson = progArgs.UseJson;
            string name = progArgs.Name;

            HelloRequest request = new HelloRequest
            {
                Name = name
            };

            HelloReply reply = await Send(request, useJson, address, traceWriter);

            traceWriter.WriteLine(reply.Message);

            return 0;
        }

        private async Task<HelloReply> Send(HelloRequest request, bool useJson, string address, TextWriter traceWriter)
        {
            if (Verbose) traceWriter.WriteLine("Creating request data object");
            string requestBody;
            if (useJson)
            {
                requestBody = JsonConvert.SerializeObject(request);
            }
            else
            {
                requestBody = request.Name;
            }

            traceWriter.WriteLine("Sending web request to {0}", address);
            Uri requestUri = new Uri(address);
            string replyBody;
            HttpClient client = null;
            bool disposeClientAfterUse = _httpClient == null;
            try
            {
                client = _httpClient ?? new HttpClient();

                HttpRequestMessage httpMsg = CreateWebRequest(requestUri, requestBody);

                replyBody = await GetWebResponse(client, httpMsg);
            }
            finally
            {
                if (disposeClientAfterUse)
                {
                    client?.Dispose();
                }
            }

            if (Verbose) traceWriter.WriteLine("Received web response data {0}", replyBody);

            if (Verbose) traceWriter.WriteLine("Decoding reply data object");
            HelloReply reply;
            if (useJson)
            {
                reply = JsonConvert.DeserializeObject<HelloReply>(replyBody);
            }
            else
            {
                reply = new HelloReply { Message = replyBody };
            }
            return reply;
        }

        private static HttpRequestMessage CreateWebRequest(Uri requestUri, string data)
        {
            HttpContent content = new StringContent(data);

            HttpRequestMessage httpMsg = new HttpRequestMessage
            {
                Method = HttpMethod.Post, // Method must be POST to send a request body
                RequestUri = requestUri,
                Content = content
            };

            return httpMsg;
        }

        private static async Task<string> GetWebResponse(HttpClient httpClient, HttpRequestMessage httpMsg)
        {
            HttpResponseMessage response = await httpClient.SendAsync(httpMsg);

            Console.WriteLine("Server response code = {0} {1}",
                (int) response.StatusCode,
                Enum.GetName(typeof(HttpStatusCode), response.StatusCode));

            response.EnsureSuccessStatusCode(); // Throws exception if HTTP error occurred.

            string responseBody = await response.Content.ReadAsStringAsync();

            return responseBody;
        }
    }
}
