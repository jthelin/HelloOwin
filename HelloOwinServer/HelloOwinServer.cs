using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Hello.Owin.Interfaces;
using Owin;

namespace Hello.Owin.Server
{
    public class HelloOwinServer
    {
        /// <summary>
        /// Wire up the message processing function for this application.
        /// </summary>
        /// <param name="app"></param>
        public void Configuration(IAppBuilder app)
        {
#if DEBUG
            app.UseErrorPage();
#endif
            // Wire up the Hello message processor component into the Owin pipeline
            var options = new HelloMessageProcessorOptions
            {
                UseJsonRequest = HelloOwinMessagingConfig.UseJson,
                UseJsonReply = HelloOwinMessagingConfig.UseJson,
                SendReplyTimestamp = true,
                DefaultName = "World"
            };
            app.Use<HelloMessageProcessor>(options);
        }
    }
}
