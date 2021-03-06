﻿namespace Poker
{
    using System;
    using Core;
    using Interfaces;
    using Factories;

    public static class PokerExecutable
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            IHumanFactory humanFactory = new HumanFactory();
            IBotFactory botFactory = new BotFactory();
            IDatabase database = new Database(botFactory, humanFactory);
            IEngine engine = new Engine(database);
            engine.Run();
        }
    }
}
