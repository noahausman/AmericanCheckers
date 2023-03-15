using System;
using System.Windows.Forms;

namespace GameUI
{
    public partial class FormSettings : Form
    {
        public FormSettings()
        {
            InitializeComponent();
        }

        public string Player1Name
        {
            get { return this.textBoxPlayer1Name.Text; }
        }

        public string Player2Name
        {
            get { return this.textBoxPlayer2Name.Text; }
        }

        public bool TwoPlayers
        {
            get { return this.checkBoxPlayer2.Checked; }
        }

        public int BoardSize
        {
            get
            {
                int boardSize;

                if (this.radioButtonSize6.Checked)
                {
                    boardSize = 6;
                }
                else if (this.radioButtonSize8.Checked)
                {
                    boardSize = 8;
                }
                else
                {
                    boardSize = 10;
                }

                return boardSize;
            }
        }

        private void buttonDone_Clicked(object sender, EventArgs e)
        {
            if (this.Player1Name.Contains(" ") || this.Player1Name.Length > 10)
            {
                MessageBox.Show("Invalid name of player 1, enter a name with a maximum of 10 characters and no spaces");
            }
            else if (this.TwoPlayers && (this.Player2Name.Contains(" ") || this.Player2Name.Length > 10))
            {
                MessageBox.Show("Invalid name of player 2, enter a name with a maximum of 10 characters and no spaces");
            }
            else if (this.Player1Name.Equals(this.Player2Name))
            {
                MessageBox.Show("Players can't have same names");
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void checkBoxPlayer2_StateChanged(object sender, EventArgs e)
        {
            this.textBoxPlayer2Name.Enabled = !this.textBoxPlayer2Name.Enabled;
        }
    }
}