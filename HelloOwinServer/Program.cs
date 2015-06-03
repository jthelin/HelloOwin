using System;
using System.Reflection;
using CmdLine;
using Microsoft.Owin.Hosting;

namespace Hello.Owin.Server
{
    public class Program
    {
        public static int Main(string[] args)
        {
            try
            {
                return Run();
            }
            catch (CommandLineException exception)
            {
                Console.WriteLine(exception.ArgumentHelp.Message);
                Console.WriteLine(exception.ArgumentHelp.GetHelpText(Console.BufferWidth));
                return -1;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error starting {0}.exe", Assembly.GetEntryAssembly().GetName().Name);
                Console.Error.WriteLine(ex);
                Console.Error.Flush();
                return 1;
            }
        }

        private static int Run()
        {
            HelloOwinServerArguments progArgs = CommandLine.Parse<HelloOwinServerArguments>();

            string address = progArgs.Address;
            bool useJson = progArgs.UseJson;

            Console.WriteLine("Starting Owin server at Address = {0} UseJson = {1}", address, useJson);

            HelloOwinServer.UseJson = useJson;

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
