using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Owin;
using Newtonsoft.Json;

using Hello.Owin.Interfaces;

namespace Hello.Owin.Server
{
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
    public class HelloMessageProcessor : OwinMiddleware
    {
        private readonly OwinMiddleware _next;
        private readonly Options _options;

        public HelloMessageProcessor(OwinMiddleware next, Options options)
            : base(next)
        {
            _next = next;
            _options = options;
        }

        /// <summary>
        /// The main message processor function for this application.
        /// </summary>
        /// <seealso cref="http://owin.org/spec/spec/owin-1.0.0.html"/>
        /// <param name="context">Owin request environment</param>
        /// <returns>Task indicating when the processing of this request is Done.</returns>
        public override async Task Invoke(IOwinContext context)
        {
            Stream requestStream = context.Request.Body;
            Stream responseStream = context.Response.Body;

            string name = await ReadRequest(requestStream);

            Trace.TraceInformation("Got request Name = {0}", name);

            string message = "Hello, " + name + "!";

            Trace.TraceInformation("Sending response Message = {0}", message);

            await SendReply(responseStream, message);
        }

        private async Task<string> ReadRequest(Stream requestStream)
        {
            if (Stream.Null.Equals(requestStream))
            {
                return _options.DefaultName;
            }

            string name;
            using (StreamReader reader = new StreamReader(requestStream))
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
            using (StreamWriter writer = new StreamWriter(responseStream))
            {
                if (_options.UseJsonReply)
                {
                    Trace.TraceInformation("Send back reply as Json message");

                    HelloReply replyData = new HelloReply
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
                        DateTime now = DateTime.Now;
                        string nowStr = now.ToString(DateTimeFormatInfo.InvariantInfo.SortableDateTimePattern);
                        await writer.WriteAsync(nowStr);
                    }

                    await writer.WriteAsync(message);
                }
            }
        }
    }
}
