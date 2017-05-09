using System;
using Hello.Owin.Client;
using Hello.Owin.Server;
using Microsoft.Owin.Testing;
using Owin;
using Xunit;

namespace Hello.Owin.Tests
{
    public class HolloOwinTests
    {
        [Fact]
        public void ServerStartup()
        {
            using (TestServer server = TestServer.Create(app =>
            {
                app.UseErrorPage(); // See Microsoft.Owin.Diagnostics
                app.Use<HelloMessageProcessor>();
            }))
            {
                Console.WriteLine("Started Owin server {0}", server);
            }
        }
    }
}
