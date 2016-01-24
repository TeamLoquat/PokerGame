namespace Poker.Models
{
    using System.Windows.Forms;

    using Poker.Interfaces;

    public class Bot : Player , IBot
    {
        public Bot(
            Panel panel,
            int chips,
            double type,
            bool turn,
            bool foldedTurn,
            bool hasFolded,
            string name,
            TextBox chipsTextBox,
            Label label)
            :base(panel,chips,type,turn,foldedTurn,hasFolded,name,chipsTextBox, label)
        {

        }
    }
}