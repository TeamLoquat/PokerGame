﻿namespace Poker.Models
{
    using System.Windows.Forms;

    using Interfaces;

    public abstract class Player : IPlayer
    {
        protected Player(Panel panel, int chips, double type, bool turn, bool foldedTurn, bool hasFolded, string name, TextBox chipsTextBox, Label label)
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

        public string Name { get; set; }

        public Panel Panel { get; set; }

        public int Chips { get; set; }

        public double Power { get; set; }

        public double Type { get; set; }

        public bool HasFolded { get; set; }

        public int Call { get; set; }

        public int Raise { get; set; }

        public bool Turn { get; set; }

        public bool FoldedTurn { get; set; }

        public TextBox ChipsTextBox { get; set; }

        public Label StatusLabel { get; set; }
    }
}