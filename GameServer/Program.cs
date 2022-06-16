using System;

namespace GameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Game Server";
            Server.Start(50, 7777);

            Console.ReadKey();
        }
    }
}
