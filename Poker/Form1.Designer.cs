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
        private void InitializeComponent()
        {
            this.progressBarTimer = new ProgressBar();
            this.textBoxAdd = new TextBox();
            this.textBoxPot = new TextBox();
            this.textBoxSB = new TextBox();
            this.textBoxBB = new TextBox();
            this.textBoxRaise = new TextBox();
            this.label1 = new Label();

            //DATABASE INITIALIZATION----------------
            this.Database.Initialize();
            //if you find a way to add this to the initialization of the database -> be my guest
            this.Database.Buttons["Fold"].Anchor = AnchorStyles.Bottom;
            this.Database.Buttons["Fold"].Click += new System.EventHandler(this.ButtonFoldClick);
            this.Database.Buttons["Check"].Anchor = AnchorStyles.Bottom;
            this.Database.Buttons["Check"].Click += new System.EventHandler(this.ButtonCheckClick);
            this.Database.Buttons["Call"].Anchor = AnchorStyles.Bottom;
            this.Database.Buttons["Call"].Click += new System.EventHandler(this.ButtonCallClick);
            this.Database.Buttons["Raise"].Anchor = AnchorStyles.Bottom;
            this.Database.Buttons["Raise"].Click += new System.EventHandler(this.ButtonRaiseClick);
            this.Database.Buttons["AddChips"].Anchor = ((AnchorStyles.Bottom | AnchorStyles.Left));
            this.Database.Buttons["AddChips"].Click += new System.EventHandler(this.ButtonAddClick);
            this.Database.Buttons["Options"].Click += new System.EventHandler(this.ButtonSBBBOptionClick);
            this.Database.Buttons["BB"].Click += new System.EventHandler(this.ButtonBigBlincdClick);
            this.Database.Buttons["SB"].Click += new System.EventHandler(this.ButtonSmallBlindClick);
            //dont know what this is
            this.SuspendLayout();

            
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

            foreach (var player in this.Database.Players)
            {
                this.Controls.Add(player.StatusLabel);
                this.Controls.Add(player.ChipsTextBox);
            }

            foreach (var keyValuePair in this.Database.Buttons)
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

