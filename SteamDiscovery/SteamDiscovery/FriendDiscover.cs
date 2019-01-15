using System;
using System.Collections.Generic;
using System.Xml;

namespace SteamDiscovery
{
    public class FriendDiscover
    {
        private SteamFriend player;
        private List<SteamFriend> friends_to_explore;

        public FriendDiscover()
        {
            Program.config.ReadConfigurationFile();

            player = new SteamFriend(Program.config.config_elements.originSteamID64);
            player.ParseXml(player);
            player.ProcessFriend();

            Init();
        }

        private void Init()
        {
            SteamFriend[] friends_array = new SteamFriend[player.Friends.Count];
            for (int i = 0; i < player.Friends.Count; i++)
            {
                friends_array[i] = SteamFriend.global_friends[player.Friends[i]];
            }
            friends_to_explore = new List<SteamFriend>(friends_array);
        }

        public void Run()
        {
            SteamFriend sub_friend = null;

            while (friends_to_explore.Count > 0)
            {
                Program.progressbar.SetText("Friend left " + friends_to_explore.Count);

                for (int i = 0; i < player.Friends.Count; i++)
                {
                    sub_friend = SteamFriend.global_friends[player.Friends[i]];

                    try
                    {
                        sub_friend.ParseXml(sub_friend);
                        sub_friend.ProcessFriend();
                        System.Threading.Thread.Sleep(100);
                    }
                    catch (Exception)
                    {
                        //Console.WriteLine("Steam error 401, retring in 1 seconds..."); //Maybe private profil.
                        System.Threading.Thread.Sleep(100);
                    }

                    float percentage = MathF.Round((i+1.0f) / player.Friends.Count, 2);
                    Program.progressbar.Update(percentage * 100);
                }

                player = friends_to_explore[0];
                friends_to_explore.RemoveAt(0);
            }

            GenerateXML_Output();
        }

        private void GenerateXML_Output()
        {
            using (XmlTextWriter writer = new XmlTextWriter(Program.config.config_elements.xml_destination + @"\processing_output.xml", null))
            {
                writer.Formatting = Formatting.Indented;

                writer.WriteStartDocument();
                writer.WriteStartElement("SteamPlayers");

                writer.WriteStartElement("params");
                writer.WriteElementString("origin_steamid", SteamFriend.global_friends["76561198045587473"].steam_id64);
                writer.WriteElementString("max_layer", (Program.config.config_elements.maxLayer + 1).ToString());
                writer.WriteEndElement();

                Console.WriteLine(SteamFriend.global_friends.Count);
                foreach (var entry in SteamFriend.global_friends)
                {
                    SteamFriend friend = SteamFriend.global_friends[entry.Key];

                    writer.WriteStartElement("friend");
                    writer.WriteElementString("id", entry.Key);
                    writer.WriteStartElement("positions");
                    writer.WriteElementString("x", friend.X().ToString());
                    writer.WriteElementString("y", friend.Y().ToString());
                    writer.WriteEndElement();
                    writer.WriteElementString("layer", friend.Layer.ToString());
                    writer.WriteStartElement("nodes");
                    foreach (string node in friend.Nodes)
                    {
                        writer.WriteElementString("node", node);
                    }
                    writer.WriteEndElement();

                    writer.WriteStartElement("friends");
                    for (int i = 0; i < friend.Friends.Count; i++)
                    {
                        writer.WriteElementString("steam_id", friend.Friends[i]);
                    }
                    writer.WriteEndElement();

                    writer.WriteEndElement();
                }

                writer.WriteEndDocument();
            }
        }
    }
}
