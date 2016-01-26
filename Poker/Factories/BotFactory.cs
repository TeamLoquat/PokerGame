namespace Poker.Factories
{
    using System.Windows.Forms;

    using Poker.Interfaces;
    using Poker.Models;

    public class BotFactory : IBotFactory
    {
        private const int InitialChips = 10000;

        public IBot Create(int number)
        {
            var bot = new Bot(
                new Panel(),
                InitialChips,
                -1,
                false,
                false,
                false,
                $"Bot {number}",
                new TextBox(),
                new Label());

            return bot;
        }
    }
}