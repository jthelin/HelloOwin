using CmdLine;
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
}