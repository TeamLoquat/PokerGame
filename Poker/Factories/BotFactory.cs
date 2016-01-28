namespace Poker.Factories
{
    using System.Windows.Forms;

    using Poker.Interfaces;
    using Poker.Models;

    /// <summary>
    /// Bot factories can raise objects of type Bot
    /// </summary>
    public class BotFactory : IBotFactory
    {
        /// <summary>
        /// all bots start with the same number of chips
        /// </summary>
        private const int InitialChips = 10000;

        /// <summary>
        /// creates an instane of type IBot
        /// </summary>
        /// <param name="number">bot id</param>
        /// <returns></returns>
        public IBot Create(int number)
        {
            var bot = new Bot(
                new Panel(),
                InitialChips,
                -1,
                false,
                false,
                false,
                string.Format("Bot {0}", number),
                new TextBox(),
                new Label());

            return bot;
        }
    }
}