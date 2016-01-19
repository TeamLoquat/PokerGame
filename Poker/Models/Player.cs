namespace Poker.Models
{
    using System.Windows.Forms;

    public abstract class Player
    {
        protected Player(Panel panel, int chips, double type, bool turn, bool fTurn, bool hasFolded, string name, System.Windows.Forms.TextBox chipsTextBox, System.Windows.Forms.Label label)
        {
            this.Panel = panel;
            this.Chips = chips;
            this.Type = type;
            this.Turn = turn;
            this.FTurn = fTurn;
            this.HasFolded = hasFolded;
            this.Name = name;
            this.ChipsTextBox = chipsTextBox;
            this.StatusLabel = label;
            this.StatusLabel.Text = "";
        }

        public string Name { get; set; }

        public Panel Panel { get; set; }

        public int Chips { get; set; }

        public double Power { get; set; }

        public double Type { get; set; }

        public bool HasFolded { get; set; }

        public int Call { get; set; }

        public int Raise { get; set; }

        public bool Turn { get; set; }

        public bool FTurn { get; set; }

        public System.Windows.Forms.TextBox ChipsTextBox { get; set; }

        public System.Windows.Forms.Label StatusLabel { get; set; }
    }
}