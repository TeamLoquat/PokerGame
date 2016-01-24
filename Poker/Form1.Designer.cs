namespace Poker
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Forms;

    using Poker.Interfaces;

    public partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent(IList<IPlayer> playerList)
        {
            this.progressBarTimer = new ProgressBar();
            this.textBoxAdd = new TextBox();
            this.textBoxPot = new TextBox();
            this.textBoxSB = new TextBox();
            this.textBoxBB = new TextBox();
            this.textBoxRaise = new TextBox();
            this.label1 = new Label();

            //PLAYERS INITIALIZATION----------------
            Dictionary<int, List<Point>> locationPointsForPlayers = new Dictionary<int, List<Point>>()
                                                      {
                                                          {0, new List<Point>()
                                                                 {
                                                                     new Point(755, 563),
                                                                     new Point(755, 589)
                                                                 } },
                                                          {1, new List<Point>()
                                                                 {
                                                                     new Point(181, 563),
                                                                     new Point(181, 589)
                                                                 } },
                                                          {2, new List<Point>()
                                                                 {
                                                                     new Point(276, 81),
                                                                     new Point(276, 107)
                                                                 } },
                                                          {3, new List<Point>()
                                                                 {
                                                                     new Point(755, 81),
                                                                     new Point(755, 107)
                                                                 } },
                                                          {4, new List<Point>()
                                                                 {
                                                                     new Point(950, 81),
                                                                     new Point(950, 107)
                                                                 } },
                                                          {5, new List<Point>()
                                                                 {
                                                                     new Point(1012, 563),
                                                                     new Point(1012, 589)
                                                                 } },

                                                      };

            //vars needeed for automatic IPlayer initialization, they are used by all players
            var fontForEachPlayer = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            var ChipsTextBoxNameTemplate = "textBox{0}Chips";
            var statusLabelNameTemplate = "{0}Status";
            var ChipsTextBoxTextTemplate = "Chips : 0";
            var SizeTemplate = new System.Drawing.Size(163, 23);
            for (int j = 0; j < playerList.Count; j++)
            {
                playerList[j].ChipsTextBox.Font = fontForEachPlayer;
                playerList[j].ChipsTextBox.Name = string.Format(ChipsTextBoxNameTemplate, playerList[j].Name);
                playerList[j].StatusLabel.Name = string.Format(statusLabelNameTemplate, playerList[j].Name);
                playerList[j].ChipsTextBox.Text = ChipsTextBoxTextTemplate;
                playerList[j].ChipsTextBox.Size = SizeTemplate;
                playerList[j].StatusLabel.Size = SizeTemplate;
                playerList[j].ChipsTextBox.TabIndex = j;
                playerList[j].StatusLabel.TabIndex = j + 1;
                playerList[j].ChipsTextBox.Location = locationPointsForPlayers[j][0];
                playerList[j].StatusLabel.Location = locationPointsForPlayers[j][1];
            }
            //dont know what this is
            this.SuspendLayout();

            //BUTTONS INITIALIZATION----------------
            var locationPointsForButtons = new Dictionary<string, Point>()
                                     {
                                        {"Fold",new Point(335, 670)},
                                        {"Check",new Point(494, 670)},
                                        {"Call",new Point(667, 671)},
                                        {"Raise",new Point(835, 671)},
                                        {"AddChips",new Point(12, 707)},
                                        {"Options",new Point(12, 12)},
                                        {"BB",new Point(12, 254)},
                                        {"SB",new Point(12, 199)},
                                     };
            //vars needeed for automatic Buttons initialization, they are used by all Buttons
            var fontTemplate = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(204)));
            var backColor = true;
            var tabIndex = 0;
            var sizeTemplate = new Size(130, 25);

            foreach (var keyValuePair in this.Buttons)
            {
                keyValuePair.Value.Font = fontTemplate;
                keyValuePair.Value.UseVisualStyleBackColor = backColor;
                keyValuePair.Value.Name = keyValuePair.Key.ToString();
                keyValuePair.Value.Text = keyValuePair.Key.ToString();
                keyValuePair.Value.TabIndex = tabIndex++;
                keyValuePair.Value.Size = sizeTemplate;
                keyValuePair.Value.Location = locationPointsForButtons[keyValuePair.Key.ToString()];
            }
            //if you find a way to add this to the foreach body do it
            this.Buttons["Fold"].Anchor = AnchorStyles.Bottom;
            this.Buttons["Fold"].Click += new System.EventHandler(this.ButtonFoldClick);

            this.Buttons["Check"].Anchor = AnchorStyles.Bottom;
            this.Buttons["Check"].Click += new System.EventHandler(this.ButtonCheckClick);

            this.Buttons["Call"].Anchor = AnchorStyles.Bottom;
            this.Buttons["Call"].Click += new System.EventHandler(this.ButtonCallClick);

            this.Buttons["Raise"].Anchor = AnchorStyles.Bottom;
            this.Buttons["Raise"].Click += new System.EventHandler(this.ButtonRaiseClick);

            this.Buttons["AddChips"].Anchor = ((AnchorStyles.Bottom | AnchorStyles.Left));
            this.Buttons["AddChips"].Click += new System.EventHandler(this.ButtonAddClick);

            this.Buttons["Options"].Click += new System.EventHandler(this.ButtonSBBBOptionClick);

            this.Buttons["BB"].Click += new System.EventHandler(this.ButtonBigBlincdClick);

            this.Buttons["SB"].Click += new System.EventHandler(this.ButtonSmallBlindClick);
            // 
            // progressBarTimer
            // 
            this.progressBarTimer.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.progressBarTimer.BackColor = System.Drawing.SystemColors.Control;
            this.progressBarTimer.Location = new System.Drawing.Point(335, 641);
            this.progressBarTimer.Maximum = 1000;
            this.progressBarTimer.Name = "progressBarTimer";
            this.progressBarTimer.Size = new System.Drawing.Size(667, 23);
            this.progressBarTimer.TabIndex = 5;
            this.progressBarTimer.Value = 1000;


            // 

            // 
            // textBoxAdd
            // 
            this.textBoxAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxAdd.Location = new System.Drawing.Point(93, 710);
            this.textBoxAdd.Name = "textBoxAdd";
            this.textBoxAdd.Size = new System.Drawing.Size(125, 20);
            this.textBoxAdd.TabIndex = 8;

            // 
            // textBoxPot
            // 
            this.textBoxPot.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.textBoxPot.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxPot.Location = new System.Drawing.Point(606, 217);
            this.textBoxPot.Name = "textBoxPot";
            this.textBoxPot.Size = new System.Drawing.Size(125, 23);
            this.textBoxPot.TabIndex = 14;
            this.textBoxPot.Text = "0";

            // 
            // textBoxSB
            // 
            this.textBoxSB.Location = new System.Drawing.Point(12, 228);
            this.textBoxSB.Name = "textBoxSB";
            this.textBoxSB.Size = new System.Drawing.Size(75, 20);
            this.textBoxSB.TabIndex = 17;
            this.textBoxSB.Text = "250";
            // 
            // textBoxBB
            // 
            this.textBoxBB.Location = new System.Drawing.Point(12, 283);
            this.textBoxBB.Name = "textBoxBB";
            this.textBoxBB.Size = new System.Drawing.Size(75, 20);
            this.textBoxBB.TabIndex = 19;
            this.textBoxBB.Text = "500";

            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(654, 193);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "Pot";
            // 
            // textBoxRaise
            // 
            this.textBoxRaise.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.textBoxRaise.Location = new System.Drawing.Point(965, 713);
            this.textBoxRaise.Name = "textBoxRaise";
            this.textBoxRaise.Size = new System.Drawing.Size(108, 20);
            this.textBoxRaise.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Poker.Properties.Resources.poker_table___Copy;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1350, 739);

            foreach (var player in playerList)
            {
                this.Controls.Add(player.StatusLabel);
                this.Controls.Add(player.ChipsTextBox);
            }

            foreach (var keyValuePair in this.Buttons)
            {
                this.Controls.Add(keyValuePair.Value);
            }


            this.Controls.Add(this.textBoxRaise);
            this.Controls.Add(this.textBoxAdd);
            this.Controls.Add(this.textBoxPot);

            this.Controls.Add(this.label1);

            this.Controls.Add(this.textBoxBB);
            this.Controls.Add(this.textBoxSB);

            this.Controls.Add(this.progressBarTimer);

            this.DoubleBuffered = true;
            this.Name = "Form1";
            this.Text = "GLS Texas Poker";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion





    }
}

