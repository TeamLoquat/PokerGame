namespace Poker.Factories
{
    using System.Windows.Forms;

    using Poker.Interfaces;
    using Poker.Models;

    public class HumanFactory : IHumanFactory
    {
        public IHuman Create()
        {
            var human = new Human(
                new Panel(),
                10000,
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