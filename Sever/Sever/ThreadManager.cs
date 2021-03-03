using System;
using System.Collections.Generic;
using System.Text;

namespace Sever
{
    public class ThreadManager
    {
        private static readonly List<Action> mainThread = new List<Action>();
        private static readonly List<Action> copiedMainThread = new List<Action>();
        private static bool actionToExecuteOnMainThread = false;

        // Update is called once per frame
        public static void ExecuteOnMainThread(Action action)
        {
            if (action == null)
            {
                Console.WriteLine("No action to execute on main thread!");
                return;
            }
            lock (mainThread)
            {
                mainThread.Add(action);
                actionToExecuteOnMainThread = true;
            }
        }

        public static void UpdateMain()
        {
            if (actionToExecuteOnMainThread == true)
            {
                copiedMainThread.Clear();
                lock (mainThread)
                {
                    copiedMainThread.AddRange(mainThread);
                    mainThread.Clear();
                    actionToExecuteOnMainThread = false;
                }

                for (int i = 0; i < copiedMainThread.Count; ++i)
                {
                    copiedMainThread[i]();
                }
            }
        }
    }
}
