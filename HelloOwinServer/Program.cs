using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;

using Hello.Owin.Interfaces;

namespace Hello.Owin.Server
{
    class Program
    {
        public static int Main(string[] args)
        {
            try
            {
                return Run(args);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error starting {0}.exe", Assembly.GetEntryAssembly().GetName().Name);
                Console.Error.WriteLine(ex);
                Console.Error.Flush();
                return 1;
            }
        }

        static int Run(string[] args)
        {
            string address = HelloOwinMessagingConfig.DefaultAddress;

            Console.WriteLine("Starting Owin server at address {0}", address);

            using (WebApp.Start<HelloOwinServer>(address))
            {
                Console.WriteLine();
                Console.WriteLine("Press any key to exit.");
                Console.ReadLine();
            }

            return 0;
        }
    }
}
