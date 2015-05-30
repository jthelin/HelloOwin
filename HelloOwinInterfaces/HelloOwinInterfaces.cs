using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hello.Owin.Interfaces
{
    public static class HelloOwinMessagingConfig
    {
        public static readonly string DefaultAddress = @"http://localhost:12345";
        public static readonly bool UseJson = true;
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
