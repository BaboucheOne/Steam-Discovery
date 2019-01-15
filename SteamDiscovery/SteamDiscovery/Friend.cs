using System;
using System.IO;
using System.Net;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace SteamDiscovery
{
    [Serializable()]
    public struct FriendInfo
    {
        [XmlElement("steamid")]
        public string steamid;
        [XmlElement("relationship")]
        public string relationship;
        [XmlElement("friend_since")]
        public int friend_since;
    }

    [Serializable()]
    [XmlRoot("friendslist")]
    public class SteamFriend
    {
        public static Dictionary<string, SteamFriend> global_friends = new Dictionary<string, SteamFriend>();
        public readonly string steam_id64 = "";
        
        [XmlArray("friends")]
        [XmlArrayItem("friend", typeof(FriendInfo))]
        public FriendInfo[] friendslist = null;
        public List<string> Friends = new List<string>();
        public List<string> Nodes = new List<string>();

        public int Layer = 0;

        private float x = 0;
        private float y = 0;
        public SteamFriend() { }
        public SteamFriend(string steam64)
        {
            if (string.IsNullOrEmpty(steam64)) return;

            steam_id64 = steam64;

            if (!global_friends.ContainsKey(steam64))
            {
                global_friends[steam64] = this;
            }
        }

        public SteamFriend(string steam64, float x, float y)
        {
            if (string.IsNullOrEmpty(steam64)) return;

            steam_id64 = steam64;
            this.x = x;
            this.y = y;

            if (!global_friends.ContainsKey(steam64))
            {
                global_friends[steam64] = this;
            }
        }

        public float X() { return x; }
        public float Y() { return y; }

        public bool IsParentOf(string steamID)
        {
            if (Nodes == null || Nodes.Count == 0) return false;
            if (Nodes.Contains(steamID)) return true;
            return false;
        }

        public bool IsChildOf(SteamFriend friend, string steamID)
        {
            return friend.IsParentOf(steamID);
        }

        public static bool IsDiscover(string steamID)
        {
            return SteamFriend.global_friends.ContainsKey(steamID);
        }

        public void ProcessFriend()
        {
            if (friendslist == null || friendslist.Length == 0) return;

            int iteration = 0;
            foreach (FriendInfo friend_info in friendslist)
            {
                SteamFriend new_friend = new SteamFriend();
                if (IsDiscover(friend_info.steamid))
                {
                    new_friend = global_friends[friend_info.steamid];
                }
                else
                {
                    if (Program.config.config_elements.maxLayer < Layer + 1) continue;

                    new_friend = new SteamFriend(friend_info.steamid)
                    {
                        Layer = Layer + 1
                    };

                    float radius = 400.0f - Layer * 5;
                    float angle = (iteration * (MathF.PI * 2)) / friendslist.Length;
                    new_friend.SetPosition(x + MathF.Cos(angle) * radius,
                    y + MathF.Sin(angle) * radius);
                }

                iteration++;

                if (!IsParentOf(friend_info.steamid))
                {
                    if (!IsChildOf(new_friend, friend_info.steamid))
                    {
                        Nodes.Add(friend_info.steamid);
                    }
                }

                if (!string.IsNullOrEmpty(friend_info.steamid))
                {
                    AddFriend(friend_info.steamid);
                }
            }

            global_friends[steam_id64].Friends = Friends;
            friendslist = null;
        }

        public void SetPosition(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        private void AddFriend(string id)
        {
            if (Friends.Contains(id)) return;
            Friends.Add(id);
        }

        public string RequestXml()
        {
            if (Friends.Count != 0) return string.Empty;

            string html = string.Empty;
            string url = @"http://api.steampowered.com/ISteamUser/GetFriendList/v0001/?key=" +
            Program.config.config_elements.apiKey + "&steamid=" + steam_id64 + "&format=xml&relationship=friend";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }

            return html;
        }

        public void ParseXml(SteamFriend friend)
        {
            string html = friend.RequestXml();

            if (html == string.Empty)
            {
                // Console.WriteLine("Unable to Parse XML...");
                return;
            }

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SteamFriend));
            SteamFriend receive = null;

            using (TextReader reader = new StringReader(html))
            {
                receive = (SteamFriend)xmlSerializer.Deserialize(reader);
            }

            SteamFriend.global_friends[friend.steam_id64].friendslist = receive.friendslist;
        }
    }
}
