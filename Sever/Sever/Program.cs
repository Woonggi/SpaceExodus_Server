using System;
using System.Threading;

namespace Sever
{
    class Program
    {
        private static bool isRunning = false;
        static void Main(string[] args)
        {
            Console.Title = "Game Server";
            isRunning = true;

            Thread mainThread = new Thread(new ThreadStart(MainThread));
            mainThread.Start();

            Server.Start(10, 3100);
        }
        private static void MainThread()
        {
            Console.WriteLine($"Main thread started. Running at {Constants.TICKS_PER_SEC} ticks per second.");
            DateTime nextLoop = DateTime.Now;
            while (isRunning == true)
            {
                while (nextLoop < DateTime.Now)
                {
                    GameLogic.Update();
                    nextLoop = nextLoop.AddMilliseconds(Constants.MS_PER_TICK);
                    // To reduce cpu usage.
                    if (nextLoop > DateTime.Now)
                    {
                        Thread.Sleep(nextLoop - DateTime.Now);
                    }
                }
            }
        }
    }
}
