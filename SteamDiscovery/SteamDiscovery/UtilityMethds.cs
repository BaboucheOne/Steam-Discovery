using System;
using System.Collections.Generic;
using System.Text;

namespace SteamDiscovery.Utility
{
    static class UtilityMethds
    {
        public static void HandleConsoleError(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine("Pess any key to exit...");
            Console.ReadKey();

            Environment.Exit(0);
        }
    }
}
