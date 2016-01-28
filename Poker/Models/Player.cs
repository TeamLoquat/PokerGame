namespace Poker.Models
{
    using System.Windows.Forms;

    using Interfaces;
    /// <summary>
    /// represents a player in the current context
    /// </summary>
    public abstract class Player : IPlayer
    {
        /// <summary>
        /// raises an instance of type Player
        /// </summary>
        /// <param name="panel">panel for displaying on the form</param>
        /// <param name="chips">number of chips in possesion</param>
        /// <param name="type">type of hand</param>
        /// <param name="turn">if player is in turn</param>
        /// <param name="foldedTurn">if player is in turn after a fold</param>
        /// <param name="hasFolded">if player has folded</param>
        /// <param name="name">name of player</param>
        /// <param name="chipsTextBox">chips in a textbox for form</param>
        /// <param name="label">label for form</param>
        protected Player(
            Panel panel, 
            int chips,
            double type,
            bool turn,
            bool foldedTurn,
            bool hasFolded,
            string name,
            TextBox chipsTextBox,
            Label label)
        {
            this.Panel = panel;
            this.Chips = chips;
            this.Type = type;
            this.Turn = turn;
            this.FoldedTurn = foldedTurn;
            this.HasFolded = hasFolded;
            this.Name = name;
            this.ChipsTextBox = chipsTextBox;
            this.StatusLabel = label;
            this.StatusLabel.Text = string.Empty;
        }

        /// <summary>
        /// gets or sets the name of the player
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// gets or sets the panel used by the form
        /// </summary>
        public Panel Panel { get; set; }

        /// <summary>
        /// gets or sets the chips each player has
        /// </summary>
        public int Chips { get; set; }

        /// <summary>
        /// gets or sets the power of the players hand
        /// </summary>
        public double Power { get; set; }

        /// <summary>
        /// gets or sets the type of each players hand
        /// </summary>
        public double Type { get; set; }

        /// <summary>
        /// gets or sets the hasFolded bool of each player 
        /// </summary>
        public bool HasFolded { get; set; }

        /// <summary>
        /// gets or sets the call ammount of each player
        /// </summary>
        public int Call { get; set; }

        /// <summary>
        /// gets or sets the raise ammount of each player
        /// </summary>
        public int Raise { get; set; }

        /// <summary>
        /// gets or sets the inTurn var of each player
        /// </summary>
        public bool Turn { get; set; }

        /// <summary>
        /// gets or sets the value if player is in turn after a fold
        /// </summary>
        public bool FoldedTurn { get; set; }

        /// <summary>
        /// gets or sets the textox used to represent each players chips
        /// </summary>
        public TextBox ChipsTextBox { get; set; }

        /// <summary>
        /// gets or sets the status label of each player
        /// </summary>
        public Label StatusLabel { get; set; }
    }
}