namespace Poker
{
    using System.Collections.Generic;

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
            this.buttonFold = new System.Windows.Forms.Button();
            this.buttonCheck = new System.Windows.Forms.Button();
            this.buttonCall = new System.Windows.Forms.Button();
            this.buttonRaise = new System.Windows.Forms.Button();
            this.buttonAddChips = new System.Windows.Forms.Button();

            this.progressBarTimer = new System.Windows.Forms.ProgressBar();

            this.textBoxAdd = new System.Windows.Forms.TextBox();

            this.textBoxPot = new System.Windows.Forms.TextBox();

            this.bOptions = new System.Windows.Forms.Button();

            this.textBoxSB = new System.Windows.Forms.TextBox();
            this.buttonSB = new System.Windows.Forms.Button();

            this.textBoxBB = new System.Windows.Forms.TextBox();
            this.buttonBB = new System.Windows.Forms.Button();

            for (int j = 0; j < playerList.Count; j++)
            {
                playerList[j].StatusLabel = new System.Windows.Forms.Label();
                playerList[j].ChipsTextBox = new System.Windows.Forms.TextBox();

                switch (j)
                {
                    case 0:
                        playerList[j].ChipsTextBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
                        playerList[j].ChipsTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
                        playerList[j].ChipsTextBox.Location = new System.Drawing.Point(755, 563);
                        playerList[j].ChipsTextBox.Name = "textBoxPlayerChips";
                        playerList[j].ChipsTextBox.Size = new System.Drawing.Size(163, 23);
                        playerList[j].ChipsTextBox.TabIndex = 6;
                        playerList[j].ChipsTextBox.Text = "Chips : 0";

                        playerList[j].StatusLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
                        playerList[j].StatusLabel.Location = new System.Drawing.Point(755, 589);
                        playerList[j].StatusLabel.Name = "playerStatusTextLabel";
                        playerList[j].StatusLabel.Size = new System.Drawing.Size(163, 32);
                        playerList[j].StatusLabel.TabIndex = 30;
                        break;
                    case 1:
                        playerList[j].ChipsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
                        playerList[j].ChipsTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
                        playerList[j].ChipsTextBox.Location = new System.Drawing.Point(181, 563);
                        playerList[j].ChipsTextBox.Name = "textBoxBot1Chips";
                        playerList[j].ChipsTextBox.Size = new System.Drawing.Size(142, 23);
                        playerList[j].ChipsTextBox.TabIndex = 13;
                        playerList[j].ChipsTextBox.Text = "Chips : 0";

                        playerList[j].StatusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
                        playerList[j].StatusLabel.Location = new System.Drawing.Point(181, 589);
                        playerList[j].StatusLabel.Name = "bot1Status";
                        playerList[j].StatusLabel.Size = new System.Drawing.Size(142, 32);
                        playerList[j].StatusLabel.TabIndex = 29;
                        break;
                    case 2:
                        playerList[j].ChipsTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
                        playerList[j].ChipsTextBox.Location = new System.Drawing.Point(276, 81);
                        playerList[j].ChipsTextBox.Name = "textBoxBot2Chips";
                        playerList[j].ChipsTextBox.Size = new System.Drawing.Size(133, 23);
                        playerList[j].ChipsTextBox.TabIndex = 12;
                        playerList[j].ChipsTextBox.Text = "Chips : 0";

                        playerList[j].StatusLabel.Location = new System.Drawing.Point(276, 107);
                        playerList[j].StatusLabel.Name = "bot2Status";
                        playerList[j].StatusLabel.Size = new System.Drawing.Size(133, 32);
                        playerList[j].StatusLabel.TabIndex = 31;
                        break;
                    case 3:
                        playerList[j].ChipsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
                        playerList[j].ChipsTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
                        playerList[j].ChipsTextBox.Location = new System.Drawing.Point(755, 81);
                        playerList[j].ChipsTextBox.Name = "textBoxBot3Chips";
                        playerList[j].ChipsTextBox.Size = new System.Drawing.Size(125, 23);
                        playerList[j].ChipsTextBox.TabIndex = 11;
                        playerList[j].ChipsTextBox.Text = "Chips : 0";

                        playerList[j].StatusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
                        playerList[j].StatusLabel.Location = new System.Drawing.Point(755, 107);
                        playerList[j].StatusLabel.Name = "bot3Status";
                        playerList[j].StatusLabel.Size = new System.Drawing.Size(125, 32);
                        playerList[j].StatusLabel.TabIndex = 28;
                        break;
                    case 4:
                        playerList[j].ChipsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
                        playerList[j].ChipsTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
                        playerList[j].ChipsTextBox.Location = new System.Drawing.Point(970, 81);
                        playerList[j].ChipsTextBox.Name = "textBoxBot4Chips";
                        playerList[j].ChipsTextBox.Size = new System.Drawing.Size(123, 23);
                        playerList[j].ChipsTextBox.TabIndex = 10;
                        playerList[j].ChipsTextBox.Text = "Chips : 0";

                        playerList[j].StatusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
                        playerList[j].StatusLabel.Location = new System.Drawing.Point(970, 107);
                        playerList[j].StatusLabel.Name = "bot4Status";
                        playerList[j].StatusLabel.Size = new System.Drawing.Size(123, 32);
                        playerList[j].StatusLabel.TabIndex = 27;
                        break;
                    case 5:
                        playerList[j].ChipsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
                        playerList[j].ChipsTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
                        playerList[j].ChipsTextBox.Location = new System.Drawing.Point(1012, 563);
                        playerList[j].ChipsTextBox.Name = "textBoxBot5Chips";
                        playerList[j].ChipsTextBox.Size = new System.Drawing.Size(152, 23);
                        playerList[j].ChipsTextBox.TabIndex = 9;
                        playerList[j].ChipsTextBox.Text = "Chips : 0";

                        playerList[j].StatusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
                        playerList[j].StatusLabel.Location = new System.Drawing.Point(1012, 589);
                        playerList[j].StatusLabel.Name = "bot5Status";
                        playerList[j].StatusLabel.Size = new System.Drawing.Size(152, 32);
                        playerList[j].StatusLabel.TabIndex = 26;
                        break;
                    default:
                        break;


                }
            }

            this.label1 = new System.Windows.Forms.Label();
            this.textBoxRaise = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // buttonFold
            // 
            this.buttonFold.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonFold.Font = new System.Drawing.Font("Microsoft Sans Serif", 17F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonFold.Location = new System.Drawing.Point(335, 670);
            this.buttonFold.Name = "buttonFold";
            this.buttonFold.Size = new System.Drawing.Size(130, 62);
            this.buttonFold.TabIndex = 0;
            this.buttonFold.Text = "Fold";
            this.buttonFold.UseVisualStyleBackColor = true;
            this.buttonFold.Click += new System.EventHandler(this.ButtonFoldClick);
            // 
            // buttonCheck
            // 
            this.buttonCheck.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonCheck.Location = new System.Drawing.Point(494, 670);
            this.buttonCheck.Name = "buttonCheck";
            this.buttonCheck.Size = new System.Drawing.Size(134, 62);
            this.buttonCheck.TabIndex = 2;
            this.buttonCheck.Text = "Check";
            this.buttonCheck.UseVisualStyleBackColor = true;
            this.buttonCheck.Click += new System.EventHandler(this.ButtonCheckClick);
            // 
            // buttonCall
            // 
            this.buttonCall.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonCall.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonCall.Location = new System.Drawing.Point(667, 671);
            this.buttonCall.Name = "buttonCall";
            this.buttonCall.Size = new System.Drawing.Size(126, 62);
            this.buttonCall.TabIndex = 3;
            this.buttonCall.Text = "Call";
            this.buttonCall.UseVisualStyleBackColor = true;
            this.buttonCall.Click += new System.EventHandler(this.ButtonCallClick);
            // 
            // buttonRaise
            // 
            this.buttonRaise.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonRaise.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonRaise.Location = new System.Drawing.Point(835, 671);
            this.buttonRaise.Name = "buttonRaise";
            this.buttonRaise.Size = new System.Drawing.Size(124, 62);
            this.buttonRaise.TabIndex = 4;
            this.buttonRaise.Text = "Raise";
            this.buttonRaise.UseVisualStyleBackColor = true;
            this.buttonRaise.Click += new System.EventHandler(this.ButtonRaiseClick);
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
            // buttonAddChips
            // 
            this.buttonAddChips.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonAddChips.Location = new System.Drawing.Point(12, 707);
            this.buttonAddChips.Name = "buttonAddChips";
            this.buttonAddChips.Size = new System.Drawing.Size(75, 25);
            this.buttonAddChips.TabIndex = 7;
            this.buttonAddChips.Text = "AddChips";
            this.buttonAddChips.UseVisualStyleBackColor = true;
            this.buttonAddChips.Click += new System.EventHandler(this.ButtonAddClick);
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
            // bOptions
            // 
            this.bOptions.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bOptions.Location = new System.Drawing.Point(12, 12);
            this.bOptions.Name = "bOptions";
            this.bOptions.Size = new System.Drawing.Size(75, 36);
            this.bOptions.TabIndex = 15;
            this.bOptions.Text = "BB/SB";
            this.bOptions.UseVisualStyleBackColor = true;
            this.bOptions.Click += new System.EventHandler(this.ButtonSBBBOptionClick);
            // 
            // bu
            // 
            this.buttonBB.Location = new System.Drawing.Point(12, 254);
            this.buttonBB.Name = "buttonBB";
            this.buttonBB.Size = new System.Drawing.Size(75, 23);
            this.buttonBB.TabIndex = 16;
            this.buttonBB.Text = "Big Blind";
            this.buttonBB.UseVisualStyleBackColor = true;
            this.buttonBB.Click += new System.EventHandler(this.ButtonBigBlincdClick);
            // 
            // textBoxSB
            // 
            this.textBoxSB.Location = new System.Drawing.Point(12, 228);
            this.textBoxSB.Name = "textBoxSB";
            this.textBoxSB.Size = new System.Drawing.Size(75, 20);
            this.textBoxSB.TabIndex = 17;
            this.textBoxSB.Text = "250";
            // 
            // buttonSB
            // 
            this.buttonSB.Location = new System.Drawing.Point(12, 199);
            this.buttonSB.Name = "buttonSB";
            this.buttonSB.Size = new System.Drawing.Size(75, 23);
            this.buttonSB.TabIndex = 18;
            this.buttonSB.Text = "Small Blind";
            this.buttonSB.UseVisualStyleBackColor = true;
            this.buttonSB.Click += new System.EventHandler(this.ButtonSmallBlindClick);
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
            

            this.Controls.Add(this.textBoxRaise);
            this.Controls.Add(this.textBoxAdd);
            this.Controls.Add(this.textBoxPot);

            this.Controls.Add(this.label1);

            this.Controls.Add(this.textBoxBB);
            this.Controls.Add(this.buttonBB);

            this.Controls.Add(this.textBoxSB);
            this.Controls.Add(this.buttonSB);

            this.Controls.Add(this.bOptions);

            this.Controls.Add(this.buttonAddChips);
            this.Controls.Add(this.progressBarTimer);

            this.Controls.Add(this.buttonRaise);
            this.Controls.Add(this.buttonCall);
            this.Controls.Add(this.buttonCheck);
            this.Controls.Add(this.buttonFold);

            this.DoubleBuffered = true;
            this.Name = "Form1";
            this.Text = "GLS Texas Poker";
            this.Layout += new System.Windows.Forms.LayoutEventHandler(this.Layout_Change);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonAddChips;
        private System.Windows.Forms.Button buttonFold;
        private System.Windows.Forms.Button buttonCheck;
        private System.Windows.Forms.Button buttonCall;
        private System.Windows.Forms.Button buttonRaise;

        private System.Windows.Forms.ProgressBar progressBarTimer;

        private System.Windows.Forms.TextBox textBoxRaise;
        private System.Windows.Forms.TextBox textBoxAdd;
        public System.Windows.Forms.TextBox textBoxPot;

        private System.Windows.Forms.Button bOptions;

        private System.Windows.Forms.TextBox textBoxBB;
        private System.Windows.Forms.Button buttonBB;

        private System.Windows.Forms.TextBox textBoxSB;
        private System.Windows.Forms.Button buttonSB;

        private System.Windows.Forms.Label label1;



    }
}

