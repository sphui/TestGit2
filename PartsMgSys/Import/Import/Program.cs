using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Import
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (!MongoDBServer.Instance.Start())
            {
                System.Windows.Forms.MessageBox.Show("Database failed to start");
                return;
            }

            Application.Run(new InputForm1());
        }
    }
}
