using Owin;
using Hello.Owin.Interfaces;

namespace Hello.Owin.Server
{
    public class HelloOwinServer
    {
        internal static bool UseJson;

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
                UseJsonRequest = HelloOwinMessagingConfig.DefaultUseJson,
                UseJsonReply = HelloOwinMessagingConfig.DefaultUseJson,
                SendReplyTimestamp = true,
                DefaultName = "World"
            };
            app.Use<HelloMessageProcessor>(options);
        }
    }
}
