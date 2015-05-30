using System;

namespace Hello.Owin.Interfaces
{
    public static class HelloOwinMessagingConfig
    {
        public const string DefaultAddress = @"http://localhost:12345";
        public const bool DefaultUseJson = true;
    }

    [Serializable]
    public class HelloRequest
    {
        public string Name { get; set; }
    }

    [Serializable]
    public class HelloReply
    {
        public DateTime Timestamp { get; set; }
        public string Message { get; set; }
    }
}
