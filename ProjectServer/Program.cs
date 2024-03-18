using System;
using System.Threading;

namespace ProjectServer
{
    class Program
    {
        public static SnapshotManager snapshotManager;
        private static bool isRunning = false;
        static void Main(string[] args)
        {
            Console.Title = "Networking Project Server";
            isRunning = true;
            Thread mainThread = new Thread(new ThreadStart(MainThread));
            mainThread.Start();
            Server.Start(3, 19855);
            Console.WriteLine("Hello, World!");
            GlobalState initialState = new GlobalState(150, 150, 1, 1, 3, 3);
            snapshotManager = new SnapshotManager();
            // Take a snapshot of the initial state
            Snapshot initialSnapshot = snapshotManager.TakeSnapshot(snapshotManager.snapshotId, initialState);

        }

        private static void MainThread()
        {
            Console.WriteLine($"Main thread started. Running at {Constants.TICKS_PER_SEC} ticks per second.");
            DateTime _nextLoop = DateTime.Now;
            while (isRunning)
            {
                while(_nextLoop < DateTime.Now)
                {
                    GameLogic.Update();
                    _nextLoop = _nextLoop.AddMilliseconds(Constants.MS_PER_TICK);
                    if (_nextLoop > DateTime.Now) 
                    {
                        Thread.Sleep(_nextLoop - DateTime.Now);
                    }
                }
            }
        }
    }
}

