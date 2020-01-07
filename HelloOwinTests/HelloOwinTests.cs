using System;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Owin.Testing;
using Owin;
using Xunit;
using Xunit.Abstractions;

using Hello.Owin.Client;
using Hello.Owin.Server;

namespace Hello.Owin.Tests
{
    public class HelloOwinTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public HelloOwinTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void ServerStartupTest()
        {
            using (TestServer server = TestServer.Create(app =>
            {
                app.UseErrorPage(); // See Microsoft.Owin.Diagnostics
                app.Use<HelloMessageProcessor>();
            }))
            {
                _testOutputHelper.WriteLine("Started Owin server {0}-{1}", server, server.GetHashCode());
            }
        }

        [Fact]
        public void ServerHostStartupTest()
        {
            using (TestServer server = TestServer.Create<HelloOwinServer>())
            {
                _testOutputHelper.WriteLine("Started Owin server {0}-{1}", server, server.GetHashCode());
            }
        }

        [Fact]
        public async Task ClientServerTest()
        {
            var serverArgs = new HelloMessageProcessorOptions();

            var clientArgs = new HelloOwinClientArguments
            {
                UseJson = serverArgs.UseJsonReply,
                Name = "Owin Tester"
            };

            int rc;
            using (TestServer server = TestServer.Create(app =>
            {
                app.Use<HelloMessageProcessor>(serverArgs);
            }))
            {
                _testOutputHelper.WriteLine("Started Owin server {0}-{1}", server, server.GetHashCode());

                clientArgs.Address = server.HttpClient.BaseAddress.AbsoluteUri;

                _testOutputHelper.WriteLine("Creating Owin client for address {0}", clientArgs.Address);

                HelloOwinClient client = new HelloOwinClient(server.HttpClient);

                TextWriter traceWriter = Console.Out;
                
                rc = await client.Run(clientArgs, traceWriter)
                    .WithTimeout(TimeSpan.FromSeconds(10));

                _testOutputHelper.WriteLine($"Run finished with rc={rc}");
            }

            rc.Should().Be(0, $"HelloOwinClient.Run rc = {rc}");
        }
    }
}
