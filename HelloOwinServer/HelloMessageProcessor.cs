using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Hello.Owin.Interfaces;
using Newtonsoft.Json;

namespace Hello.Owin.Server
{
    using AppFunc = Func<IDictionary<string, object>, Task>;
    using Options = HelloMessageProcessorOptions;

    public class HelloMessageProcessorOptions
    {
        public bool UseJsonRequest { get; set; }
        public bool UseJsonReply { get; set; }
        public bool SendReplyTimestamp { get; set; }
        public string DefaultName { get; set; }
    }

    /// <summary>
    /// Owin processor component for the Hello application.
    /// </summary>
    public class HelloMessageProcessor
    {
        private readonly AppFunc _next;
        private readonly Options _options;

        public HelloMessageProcessor(AppFunc next, Options options)
        {
            _next = next;
            _options = options;
        }
 
        /// <summary>
        /// The main message processor function for this application.
        /// </summary>
        /// <seealso cref="http://owin.org/spec/spec/owin-1.0.0.html"/>
        /// <param name="environment">Owin request environment</param>
        /// <returns>Task indicating when the processing of this request is Done.</returns>
        public async Task Invoke(IDictionary<string, object> environment)
        {
            var request = environment["owin.RequestBody"] as Stream;
            string name = await ReadRequest(request);

            Trace.TraceInformation("Got request Name = {0}", name);

            string message = "Hello, " + name + "!";

            Trace.TraceInformation("Sending response Message = {0}", message);

            var response = environment["owin.ResponseBody"] as Stream;
            await SendReply(response, message);
        }

        private async Task<string> ReadRequest(Stream requestStream)
        {
            if (Stream.Null.Equals(requestStream))
            {
                return _options.DefaultName;
            }

            string name;
            using (var reader = new StreamReader(requestStream))
            {
                string requestBody = await reader.ReadToEndAsync();

                if (string.IsNullOrEmpty(requestBody))
                {
                    Trace.TraceInformation("No body - use default name");
                    name = _options.DefaultName;
                }
                else if (_options.UseJsonRequest)
                {
                    Trace.TraceInformation("Decode Json message from request body");
                    HelloRequest data = JsonConvert.DeserializeObject<HelloRequest>(requestBody);
                    name = data.Name;
                }
                else
                {
                    Trace.TraceInformation("Body text is the name data");
                    name = requestBody;
                }
            }
            return name;
        }

        private async Task SendReply(Stream responseStream, string message)
        {
            using (var writer = new StreamWriter(responseStream))
            {
                if (_options.UseJsonReply)
                {
                    Trace.TraceInformation("Send back reply as Json message");
                    var replyData = new HelloReply
                    {
                        Message = message
                    };
                    if (_options.SendReplyTimestamp)
                    {
                        replyData.Timestamp = DateTime.UtcNow;
                    }
                    string data = JsonConvert.SerializeObject(replyData);

                    await writer.WriteAsync(data);
                }
                else
                {
                    Trace.TraceInformation("Send back reply as plain text");
                    if (_options.SendReplyTimestamp)
                    {
                        await writer.WriteAsync(DateTime.Now.ToLongTimeString());
                    }
                    await writer.WriteAsync(message);
                }
            }
        }
    }
}
