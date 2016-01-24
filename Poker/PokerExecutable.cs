using System;
using Poker.Core;
using Poker.Interfaces;

namespace Poker
{
    static class PokerExecutable
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

           IEngine engine= new Engine();
           engine.Run();
          
            /* Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var whatever = new Form1();
            Application.Run(whatever);*/
        }
    }
}
