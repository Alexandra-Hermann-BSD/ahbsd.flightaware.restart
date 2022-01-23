using System;
using System.Collections.Generic;
using System.Linq;
using ahbsd.network.check;


namespace ahbsd.flightaware.restart
{
    internal static class Program
    {
        private static readonly ICheckHosts checkHosts;

        private static  IReadOnlyList<string> argList;

        static Program()
        {
            checkHosts = new CheckHosts();
            checkHosts.AddHost(CheckArea.External, "8.8.8.8");
        }
        
        static void Main(string[] args)
        {
            SetupArgList(args);
            
            Console.WriteLine("FlightAware Test And Restart");
            Console.WriteLine();
            
            if (StartArguments?.Count > 0) { ReactOnArguments(); }
            
            ICheckConnections checks = new CheckConnections();

            if (checks.IsGatewayReachable)
            {
                Console.WriteLine(checks.LocalConnectionStatus);

                if (checks.IsAnyExternReachable)
                {
                    Console.WriteLine(checks.ExternalConnectionStatus);
                }
                else
                {
                    Console.WriteLine(checks.ExternalConnectionStatus);
                }
            }
            else
            {
                Console.WriteLine(checks.LocalConnectionStatus);
            }
        }

        private static void ReactOnArguments()
        {
            if (StartArguments.Contains("-?") || StartArguments.Contains("--help"))
            {
                Console.WriteLine();
            }
        }

        private static void SetupArgList(string[] args)
        {
            argList = (IReadOnlyList<string>) args?.ToList();
        }
        
        public static IReadOnlyList<string> StartArguments => argList;
    }
}
