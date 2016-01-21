namespace Poker.Models
{
    using System.Threading;
    using System.Windows.Forms;

    using Poker.Interfaces;

    public class Human: Player , IHuman
    {
        public Human(Panel panel, int chips, double type, bool turn, bool foldedTurn, bool hasFolded,string name, System.Windows.Forms.TextBox chipsTextBox, System.Windows.Forms.Label label)
            :base(panel,chips,type,turn,foldedTurn,hasFolded,name, chipsTextBox, label)
        {
            
        }
    }
}