using System;

namespace SteamDiscovery
{
    public class Progressbar
    {
        private string processing_text = "Loading";

        public Progressbar() { }

        public void NewLoading()
        {
            Console.CursorLeft = processing_text.Length + 13;
            Console.WriteLine("\n");
        }

        public void SetText(string text)
        {
            processing_text = text;
        }

        public void Update(float per)
        {
            Console.CursorLeft = 0;
            Console.Write(processing_text);

            Console.CursorLeft = processing_text.Length + 1;
            Console.Write("[");
            Console.CursorLeft = processing_text.Length + 12;
            Console.Write("]");

            int position = processing_text.Length + 2;
            for (int i = 0; i < 10; i++)
            {
                Console.CursorLeft = position;

                if (i < (int)per / 10)
                {
                    Console.Write("█");
                } else
                {
                    Console.Write(" ");
                }

                position++;
            }

            Console.CursorLeft = processing_text.Length + 13;
            Console.Write(" - {0}%   ", per);
        }
    }
}
