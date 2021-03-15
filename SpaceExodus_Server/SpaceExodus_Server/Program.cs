using System;
using System.Threading;

namespace SpaceExodus_Server
{
    class Program
    {
        private static bool isRunning = false;
        static void Main(string[] args)
        {
            Console.Title = "Space Exodus Server";
            isRunning = true;

            Thread mainThread = new Thread(new ThreadStart(MainThread));
            mainThread.Start();

            Server.Start(10, 26950);
        }
        private static void MainThread()
        {
            Console.WriteLine($"Main thread started. Running at {Constants.TICS_PER_SEC} ticks per second.");
            DateTime nextLoop = DateTime.Now;

            while (isRunning)
            {
                 while (nextLoop < DateTime.Now)
                {
                    GameLogic.Update();
                    nextLoop = nextLoop.AddMilliseconds(Constants.MS_PER_TICK);
                    if (nextLoop > DateTime.Now)
                    {
                        Thread.Sleep(nextLoop - DateTime.Now);
                    }
                }
            }
        }
    }
}
