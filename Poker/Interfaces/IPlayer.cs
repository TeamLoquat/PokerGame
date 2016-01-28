namespace Poker.Interfaces
{
    using System.Windows.Forms;

    /// <summary>
    /// public representation of an abstract player (what all players in the game should have)
    /// </summary>
    public interface IPlayer
    {
        /// <summary>
        /// Gets of sets Name of the player
        /// </summary>
        string Name
        {
            get;
            set;
        }

        /// <summary>
        /// panel for the form which displays player info
        /// </summary>
        Panel Panel { get; set; }

        /// <summary>
        /// number of chips in possesion
        /// </summary>
        int Chips { get; set; }

        /// <summary>
        /// power of hand
        /// </summary>
        double Power { get; set; }

        /// <summary>
        /// type of hand
        /// </summary>
        double Type { get; set; }

        /// <summary>
        /// hols info about if a player has folded or not
        /// </summary>
        bool HasFolded { get; set; }

        /// <summary>
        /// call ammount
        /// </summary>
        int Call { get; set; }

        /// <summary>
        /// raise ammount
        /// </summary>
        int Raise { get; set; }

        /// <summary>
        /// if player is in turn
        /// </summary>
        bool Turn { get; set; }

        /// <summary>
        /// if player is a turn after a fold
        /// </summary>
        bool FoldedTurn { get; set; }

        /// <summary>
        /// representation of chips on the form
        /// </summary>
        TextBox ChipsTextBox { get; set; }

        /// <summary>
        /// representation of status on the form
        /// </summary>
        Label StatusLabel { get; set; }
    }
}