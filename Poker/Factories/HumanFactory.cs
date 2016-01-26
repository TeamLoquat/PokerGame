namespace Poker.Factories
{
    using System.Windows.Forms;

    using Poker.Interfaces;
    using Poker.Models;

    public class HumanFactory : IHumanFactory
    {
        private const int InitialChips = 10000;

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