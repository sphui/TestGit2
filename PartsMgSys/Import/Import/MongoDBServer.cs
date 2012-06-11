using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Import
{
    public class MongoDBServer
    {
        private static Lazy<MongoDBServer> instance = new Lazy<MongoDBServer>(() => new MongoDBServer());
        private static string mongodbServerExeName = "mongod.exe";
        private static string mongodbServerExePath = @"E:\Peak\mongodb\bin";
        private static string mongodbDatabasePath = @"C:\data\db";

        static MongoDBServer()
        {
        }

        private MongoDBServer()
        {
        }

        public static MongoDBServer Instance
        {
            get { return instance.Value; }
        }

        public bool Start()
        {
            try
            {
                if (!Process.GetProcesses().Any(x=>x.ProcessName.Contains(mongodbServerExeName)))
                {
                    Process p = new Process();
                    string strmongoexe = Path.Combine(mongodbServerExePath, mongodbServerExeName);
                    p.StartInfo = new ProcessStartInfo(strmongoexe, "-dbpath " + mongodbDatabasePath);
                    p.Start();

                    //create Data folder if it doesn't exist yet
                    //c:\data\db
                    if (!Directory.Exists(@"c:\data\db"))
                        Directory.CreateDirectory(@"c:\data\db");

                    
                    Thread.Sleep(10000); // give it time to start correctly
                    return true;
                }
                else
                {
                    return true;
                }
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}
