using CmdLine;
using JetBrains.Annotations;

using Hello.Owin.Interfaces;

namespace Hello.Owin.Server
{
    [PublicAPI]
    [CommandLineArguments(Program = "HelloOwinServer", Title = "HelloOwinServer", Description = "Hello Owin server example")]
    public class HelloOwinServerArguments
    {
        [CommandLineParameter(Command = "?", Default = false, Description = "Show Help", Name = "Help", IsHelp = true)]
        public bool Help { get; set; }

        [CommandLineParameter(Command = "json", Required = false,
            Default = HelloOwinMessagingConfig.DefaultUseJson,
            Description = "Use Json messaging.")]
        public bool UseJson { get; set; }

        [CommandLineParameter(Name = "address", ParameterIndex = 1, Required = false,
            Default = HelloOwinMessagingConfig.DefaultAddress,
            Description = "HTTP address this server should listen on.")]
        public string Address { get; set; }

        public HelloOwinServerArguments()
        {
            Address = HelloOwinMessagingConfig.DefaultAddress;
            UseJson = HelloOwinMessagingConfig.DefaultUseJson;
        }
    }
}
