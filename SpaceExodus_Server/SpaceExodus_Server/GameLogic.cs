using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceExodus_Server
{
    class GameLogic
    {
        public static void Update()
        {
            foreach (Client client in Server.clients.Values)
            {
                if (client.player != null)
                {
                    client.player.Update();
                }
            }
            //Console.WriteLine(Server.frame++);
            Server.frame++;
            ThreadManager.UpdateMain();
        }
    }
}
