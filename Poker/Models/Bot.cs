namespace Poker.Models
{
    using System.Windows.Forms;

    public class Bot : Player
    {
        public Bot(Panel panel, int chips, double type, bool turn, bool fTurn, bool hasFolded,string name, System.Windows.Forms.TextBox chipsTextBox, System.Windows.Forms.Label label)
            :base(panel,chips,type,turn,fTurn,hasFolded,name,chipsTextBox, label)
        {

        }
    }
}