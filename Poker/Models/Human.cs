namespace Poker.Models
{
    using System.Windows.Forms;

    using Poker.Interfaces;
    /// <summary>
    /// represents a human
    /// </summary>
    public class Human : Player, IHuman
    {
        /// <summary>
        /// Initializes an instance of Human
        /// </summary>
        public Human(
            Panel panel,
            int chips,
            double type,
            bool turn,
            bool foldedTurn,
            bool hasFolded,
            string name,
            TextBox chipsTextBox,
            Label label)
            : base(panel, chips, type, turn, foldedTurn, hasFolded, name, chipsTextBox, label)
        {

        }
    }
}