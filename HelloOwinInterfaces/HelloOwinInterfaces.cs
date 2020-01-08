using System;
using JetBrains.Annotations;

namespace Hello.Owin.Interfaces
{
    [PublicAPI]
    public static class HelloOwinMessagingConfig
    {
        public const string DefaultAddress = @"http://localhost:12345";
        public const bool DefaultUseJson = true;
    }

    [Serializable]
    [PublicAPI]
    public class HelloRequest
    {
        public string Name { get; set; }
    }

    [Serializable]
    [PublicAPI]
    public class HelloReply
    {
        public DateTime Timestamp { get; set; }
        public string Message { get; set; }
    }
}
