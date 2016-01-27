namespace Poker
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    public partial class AddChips : Form
    {
        private int chipsFromTextBoxToBeAdded = 0;

        public AddChips()
        {
            FontFamily fontFamily = new FontFamily("Arial");
            InitializeComponent();
            ControlBox = false;
            RanOutOfChipsLabel.BorderStyle = BorderStyle.FixedSingle;
        }

        public int ChipsFromTextBoxToBeAdded 
        {
            get { return this.chipsFromTextBoxToBeAdded; }
            set { this.chipsFromTextBoxToBeAdded = value; }
        }

        public void buttonAddChips_Click(object sender, EventArgs e)
        {
            int parsedValue;
            if (int.Parse(chipsToBeAddedTextBox.Text) > 100000000)
            {
                MessageBox.Show("The maximium chips you can add is 100000000");
                return;
            }

            if (!int.TryParse(chipsToBeAddedTextBox.Text, out parsedValue))
            {
                MessageBox.Show("This is a number only field");
                return;

            }
            else if (int.TryParse(chipsToBeAddedTextBox.Text, out parsedValue) && int.Parse(chipsToBeAddedTextBox.Text) <= 100000000)
            {
                chipsFromTextBoxToBeAdded = int.Parse(chipsToBeAddedTextBox.Text);
                this.Close();
            }
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            var message = "Are you sure?";
            var title = "Quit";
            var result = MessageBox.Show(
            message, 
            title,
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);
            switch (result)
            {
                case DialogResult.No:
                    break;
                case DialogResult.Yes:
                    Application.Exit();
                    break;
            }
        }
    }
}
