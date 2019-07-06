using System;
using System.Threading.Tasks;
using FluentAssertions;
using Hello.Owin.Client;
using Hello.Owin.Server;
using Microsoft.Owin.Testing;
using Owin;
using Xunit;
using Xunit.Abstractions;

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
                _testOutputHelper.WriteLine("Started Owin server {0}", server);
            }
        }

        [Fact]
        public void ServerHostStartupTest()
        {
            using (TestServer server = TestServer.Create<HelloOwinServer>())
            {
                _testOutputHelper.WriteLine("Started Owin server {0}", server);
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
                clientArgs.Address = server.HttpClient.BaseAddress.AbsoluteUri;

                HelloOwinClient client = new HelloOwinClient(server.HttpClient);

                rc = await client.Run(clientArgs)
                    .WithTimeout(TimeSpan.FromSeconds(10));
            }

            rc.Should().Be(0, "HelloOwinClient.Run rc = {0}", rc);
        }
    }
}
