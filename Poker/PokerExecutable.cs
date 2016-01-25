using System;
using Poker.Core;
using Poker.Interfaces;

namespace Poker
{
    using Poker.Factories;

    static class PokerExecutable
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            IHumanFactory humanFactory = new HumanFactory();
            IBotFactory botFactory = new BotFactory();
            IDatabase database = new Database(botFactory,humanFactory);
            IEngine engine = new Engine(database);
            engine.Run();

           
        }
    }
}
