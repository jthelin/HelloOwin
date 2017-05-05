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
        /// <param name="app"> The Owin stack builder. </param>
        public void Configuration(IAppBuilder app)
        {
#if DEBUG
            app.UseErrorPage();
#endif
            var options = new HelloMessageProcessorOptions
            {
                UseJsonRequest = HelloOwinMessagingConfig.DefaultUseJson,
                UseJsonReply = HelloOwinMessagingConfig.DefaultUseJson,
                SendReplyTimestamp = true,
                DefaultName = "World"
            };

            // Wire up the Hello message processor component into the Owin pipeline
            app.Use<HelloMessageProcessor>(options);
        }
    }
}
