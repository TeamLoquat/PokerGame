namespace Poker.Interfaces
{
    using System.Collections.Generic;
    using System.Windows.Forms;

    /// <summary>
    /// Holds all the players and buttons the game relies on
    /// </summary>
    public interface IDatabase
    {
        /// <summary>
        /// Holds all IPlayers in the game
        /// </summary>
        IList<IPlayer> Players
        {
            get;
            set;
        }

        /// <summary>
        /// Hold all buttons that the game relies on
        /// </summary>
        IDictionary<string, Button> Buttons
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes the DB
        /// </summary>
        void Initialize();

        /// <summary>
        /// displays chips of all the players in the database
        /// </summary>
        void DisplayPlayerChips();
    }
}
