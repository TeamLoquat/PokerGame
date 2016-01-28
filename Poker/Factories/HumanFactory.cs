namespace Poker.Factories
{
    using System.Windows.Forms;

    using Poker.Interfaces;
    using Poker.Models;

    /// <summary>
    /// Factory that can raise instances of IHuman
    /// </summary>
    public class HumanFactory : IHumanFactory
    {
        /// <summary>
        /// all Humans start with the same number of chips
        /// </summary>
        private const int InitialChips = 10000;

        /// <summary>
        /// Creates an instance of Human represented as IHuman
        /// </summary>
        /// <returns></returns>
        public IHuman Create()
        {
            var human = new Human(
                new Panel(),
                InitialChips,
                -1,
                false,
                false,
                false,
                "Player",
                new TextBox(),
                new Label());

            return human;
        }
    }
}