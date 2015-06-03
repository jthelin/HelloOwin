using CmdLine;
using Hello.Owin.Interfaces;

namespace Hello.Owin.Server
{
    [CommandLineArguments(Program = "HelloOwinServer", Title = "HelloOwinServer", Description = "Hello Owin server example")]
    internal class HelloOwinServerArguments
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
    }
}