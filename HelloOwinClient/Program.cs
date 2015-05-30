using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CmdLine;
using Newtonsoft.Json;

using Hello.Owin.Interfaces;

namespace Hello.Owin.Client
{
    [CommandLineArguments(Program = "HelloOwinClient", Title = "HelloOwinClient", Description = "Hello Owin client example")]
    internal class HelloOwinClientArguments
    {
        [CommandLineParameter(Command = "?", Default = false, Description = "Show Help", Name = "Help", IsHelp = true)]
        public bool Help { get; set; }

        [CommandLineParameter(Command = "json", Required = false,
            Default = HelloOwinMessagingConfig.DefaultUseJson,
            Description = "Use Json messaging.")]
        public bool UseJson { get; set; }

        [CommandLineParameter(Name = "name", ParameterIndex = 1, Required = false,
            Default = "Earth",
            Description = "Which name the server should say hello to.")]
        public string Name { get; set; }

        [CommandLineParameter(Name = "address", ParameterIndex = 2, Required = false,
        Default = HelloOwinMessagingConfig.DefaultAddress,
        Description = "HTTP address this client should connect to.")]
        public string Address { get; set; }
    }

    public class Program
    {
        private static bool Verbose = true;

        public static int Main(string[] args)
        {
            try
            {
                return Run();
            }
            catch (CommandLineException exception)
            {
                Console.WriteLine(exception.ArgumentHelp.Message);
                Console.WriteLine(exception.ArgumentHelp.GetHelpText(Console.BufferWidth));
                return -1;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error starting {0}.exe", Assembly.GetEntryAssembly().GetName().Name);
                Console.Error.WriteLine(ex);
                Console.Error.Flush();
                return 1;
            }
        }

        private static int Run()
        {
            HelloOwinClientArguments progArgs = CommandLine.Parse<HelloOwinClientArguments>();

            string address = progArgs.Address;
            bool useJson = progArgs.UseJson;
            string name = progArgs.Name;

            HelloRequest request = new HelloRequest
            {
                Name = name
            };

            HelloReply reply = Send(request, useJson, address).Result; // Blocking call because this is Main thread.

            Console.WriteLine(reply.Message);

            Console.WriteLine();
            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();

            return 0;
        }

        private static async Task<HelloReply> Send(HelloRequest request, bool useJson, string address)
        {
            if (Verbose) Trace.TraceInformation("Creating request data object");
            string requestBody;
            if (useJson)
            {
                requestBody = JsonConvert.SerializeObject(request);
            }
            else
            {
                requestBody = request.Name;
            }

            Trace.TraceInformation("Sending web request to {0}", address);
            WebRequest webRequest = await CreateWebRequest(address, requestBody);

            string replyBody = await GetWebResponse(webRequest);

            if (Verbose) Trace.TraceInformation("Received web response data {0}", replyBody);

            if (Verbose) Trace.TraceInformation("Decoding reply data object");
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

        private static async Task<WebRequest> CreateWebRequest(string uri, string data)
        {
            const string method = "POST"; // Method must be POST to send a request body

            if (Verbose) Trace.TraceInformation("Creating web request for method {0} address {1}", method, uri);

            WebRequest webRequest = WebRequest.Create(uri);
            webRequest.Method = method;
            webRequest.ContentType = "application/x-www-form-urlencoded";

            // Encode request data to UTF-8 byte array
            byte[] byteArray = Encoding.UTF8.GetBytes(data);
            webRequest.ContentLength = byteArray.Length;
            using (Stream dataStream = webRequest.GetRequestStream())
            {
                await dataStream.WriteAsync(byteArray, 0, byteArray.Length);
            }

            return webRequest;
        }

        public static async Task<string> GetWebResponse(WebRequest webRequest)
        {
            // Get the original response.
            HttpWebResponse response = webRequest.GetResponse() as HttpWebResponse;

            Trace.TraceInformation("Server response code = {0} {1}",
                response.StatusCode,
                response.StatusDescription);

            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                return await reader.ReadToEndAsync();
            }
        }

    }
}
