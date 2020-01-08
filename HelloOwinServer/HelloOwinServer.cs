using JetBrains.Annotations;
using Owin;

namespace Hello.Owin.Server
{
    [UsedImplicitly]
    public class HelloOwinServer
    {
        internal static bool UseJson;

        /// <summary>
        /// Wire up the message processing function for this application.
        /// </summary>
        /// <param name="app"> The Owin stack builder. </param>
        [UsedImplicitly]
        public void Configuration(IAppBuilder app)
        {
#if DEBUG
            app.UseErrorPage();
#endif
            var options = new HelloMessageProcessorOptions();

            // Wire up the Hello message processor component into the Owin pipeline
            app.Use<HelloMessageProcessor>(options);
        }
    }
}
