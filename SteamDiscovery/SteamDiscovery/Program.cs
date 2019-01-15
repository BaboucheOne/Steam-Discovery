using System;
using System.Xml;

namespace SteamDiscovery
{
    public static class Program
    {
        public static Configurations config = new Configurations();
        public static Progressbar progressbar = new Progressbar();

        static void Main(string[] args)
        {
            FriendDiscover discovery = new FriendDiscover();
            discovery.Run();
        }
    }
}
