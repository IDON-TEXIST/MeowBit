﻿using dotBitNS.Server;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotBitNS.UI
{
    class Monitor
    {
        public static bool NameServerOnline { get { return NameServer.Ok; } }
        public static bool NameCoinOnline { get { return NmcClient.Instance.Available; } }

        static Timer timerNmcCheck = null;
        static readonly TimeSpan NmcCheckInterval = TimeSpan.FromSeconds(5.0);
        
        public static void Initialize()
        {
            EventSink.NameServerAvailableChanged += EventSink_NameServerAvailableChanged;
            EventSink.Shutdown += EventSink_Shutdown;
            CheckNmcConnection();
            timerNmcCheck = Timer.DelayCall(NmcCheckInterval, NmcCheckInterval, new TimerCallback(CheckNmcConnection));
        }

        static void EventSink_Shutdown(ShutdownEventArgs e)
        {
            Console.WriteLine("Shutting down.");
        }

        static void CheckNmcConnection()
        {
            var info = NmcClient.Instance.GetInfo();
            if (info != null)
                Debug.WriteLine(string.Format("Success: Wallet version {0}", info.Version));
        }


        static void EventSink_NameServerAvailableChanged(NmcClient source, NameServerAvailableChangedEventArgs e)
        {
            Console.WriteLine("Namecoin client is now {0}.", e.Available ? "online" : "offline");
        }
    }
}