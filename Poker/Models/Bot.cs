namespace Poker.Models
{
    using System.Windows.Forms;

    using Poker.Interfaces;

    /// <summary>
    /// represents a basic bot
    /// </summary>
    public class Bot : Player, IBot
    {
        /// <summary>
        /// Initializes an instance of Bot
        /// </summary>
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
            :base(panel, chips, type, turn, foldedTurn, hasFolded, name, chipsTextBox, label)
        {

        }
    }
}