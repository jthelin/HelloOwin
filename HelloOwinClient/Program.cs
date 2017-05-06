﻿using System;
using System.Reflection;
using CmdLine;

namespace Hello.Owin.Client
{
    public class Program
    {
        public static int Main(string[] args)
        {
            try
            {
                HelloOwinClientArguments progArgs = CommandLine.Parse<HelloOwinClientArguments>();

                HelloOwinClient client = new HelloOwinClient();
                
                return client.Run(progArgs);
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
    }
}
