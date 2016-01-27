namespace Poker.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    public interface IDatabase
    {
        IList<IPlayer> Players { get; set; }

        IDictionary<string, Button> Buttons { get; set; }

        void Initialize();

        void DisplayPlayerChips();
    }
}
