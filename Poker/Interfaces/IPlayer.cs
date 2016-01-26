namespace Poker.Interfaces
{
    using System.Windows.Forms;

    public interface IPlayer
    {
        string Name { get; set; }

        Panel Panel { get; set; }

        int Chips { get; set; }

        double Power { get; set; }

        double Type { get; set; }

        bool HasFolded { get; set; }

        int Call { get; set; }

        int Raise { get; set; }

        bool Turn { get; set; }

        bool FoldedTurn { get; set; }

        TextBox ChipsTextBox { get; set; }

        Label StatusLabel { get; set; }
    }
}