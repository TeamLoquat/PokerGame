namespace Poker.Factories
{
    using System.Windows.Forms;

    using Poker.Interfaces;
    using Poker.Models;

    public class BotFactory : IBotFactory
    {
        public IBot Create(int number)
        {
            var bot = new Bot(
                new Panel(),
                10000,
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