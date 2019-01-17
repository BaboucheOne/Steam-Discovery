using System;

namespace SteamDiscovery
{
    class Program
    {
        public static Configurations config = new Configurations();
        public static Progressbar progressbar = new Progressbar();

        static void Main(string[] args)
        {
            FriendDiscover discovery = new FriendDiscover();
            long timeStart = DateTime.Now.Ticks;
            discovery.Run();
            double timeEnd = (new TimeSpan(DateTime.Now.Ticks - timeStart)).TotalMinutes;

            Console.ReadKey();
            Console.WriteLine("{0} have been found in {1} minutes. Press any key to exit...", SteamFriend.global_friends.Count, timeEnd);
        }
    }
}
