using System;
using System.Drawing;
using System.Windows.Forms;

namespace GameUI
{
    public partial class FormGame : Form
    {
        private Button[,] m_ButtonBoard;
        private bool m_SecondClick = false;
        private int m_ButtonBoardSize;
        private Button m_ButtonPaintedInBlue;
        private Point m_PrevButtonPoint;
        private Point m_NextButtonPoint;

        public event Action<Point, Point> MoveWasMade;

        public event Func<Point, bool> FirstStepOfMoveWasMade;

        public event Func<Point, string> ButtonStateChanged;

        public event Action ButtonRestartGameClicked;

        public FormGame(int i_BoardSize, string i_Player1Name, string i_Player2Name)
        {
            m_ButtonBoardSize = i_BoardSize;
            m_ButtonBoard = new Button[m_ButtonBoardSize, m_ButtonBoardSize];
            InitializeComponent();
            setWindowSize();
            labelPlayer1.Name = i_Player1Name;
            labelPlayer2.Name = i_Player2Name;
        }

        private void setWindowSize()
        {
            this.ClientSize = new System.Drawing.Size(60 + (m_ButtonBoardSize * 50), 120 + (m_ButtonBoardSize * 50));
        }

        public void InitializeButtonBoard()
        {
            Button newButton;

            for (int row = 0; row < m_ButtonBoardSize; row++)
            {
                for (int col = 0; col < m_ButtonBoardSize; col++)
                {
                    newButton = new Button();
                    m_ButtonBoard[row, col] = newButton;
                    newButton.Width = 50;
                    newButton.Height = 50;
                    newButton.Location = new Point((col * 50) + 30, (row * 50) + 90);
                    newButton.FlatStyle = FlatStyle.Flat;
                    if (row % 2 == col % 2)
                    {
                        newButton.Enabled = false;
                        newButton.BackColor = Color.Linen;
                        newButton.FlatAppearance.BorderColor = Color.Linen;
                    }
                    else
                    {
                        newButton.BackColor = Color.Tan;
                        newButton.FlatAppearance.BorderColor = Color.Tan;
                        newButton.Tag = new Point(col, row);
                        newButton.Click += new EventHandler(button_Clicked);
                    }

                    this.Controls.Add(newButton);
                }
            }
        }

        public void InitializeButtonBackgroundImage()
        {
            for (int row = 0; row < m_ButtonBoardSize; row++)
            {
                for (int col = 0; col < m_ButtonBoardSize; col++)
                {
                    if (row % 2 != col % 2)
                    {
                        ChangeButtonBackgroundImage(new Point(col, row));
                    }
                }
            }
        }

        public void SetLabelText(string i_Name, int i_Score)
        {
            if (i_Name.Equals(labelPlayer1.Name))
            {
                labelPlayer1.Text = string.Format("{0} : {1}", i_Name, i_Score);
            }
            else
            {
                labelPlayer2.Text = string.Format("{0} : {1}", i_Name, i_Score);
            }
        }

        private void button_Clicked(object sender, EventArgs e)
        {
            Button senderButton = (Button)sender;
            Point buttonTagAsPoint;

            buttonTagAsPoint = (Point)senderButton.Tag;
            if (!m_SecondClick)
            {
                if (FirstStepOfMoveWasMade != null)
                {
                    if (FirstStepOfMoveWasMade.Invoke(buttonTagAsPoint))
                    {
                        senderButton.BackColor = Color.LightSteelBlue;
                        m_PrevButtonPoint = buttonTagAsPoint;
                        m_ButtonPaintedInBlue = (Button)sender;
                        m_SecondClick = true;
                    }
                }
            }
            else
            {
                m_ButtonPaintedInBlue.BackColor = Color.Tan;
                if (sender != m_ButtonPaintedInBlue)
                {
                    m_NextButtonPoint = buttonTagAsPoint;
                    if (MoveWasMade != null)
                    {
                        MoveWasMade.Invoke(m_PrevButtonPoint, m_NextButtonPoint);
                    }
                }

                m_SecondClick = false;
            }
        }

        public void DeleteButtonBackgroundImage(Point i_Point)
        {
            m_ButtonBoard[i_Point.Y, i_Point.X].BackgroundImage = null;
        }

        public void SwitchButtonBackgroundImage(Point i_PrevPoint, Point i_NextPoint)
        {
            Button buttonInPrevPoint = m_ButtonBoard[i_PrevPoint.Y, i_PrevPoint.X], buttonInNextPoint = m_ButtonBoard[i_NextPoint.Y, i_NextPoint.X];

            buttonInNextPoint.BackgroundImage = buttonInPrevPoint.BackgroundImage;
            buttonInNextPoint.BackgroundImageLayout = ImageLayout.Stretch;
            buttonInPrevPoint.BackgroundImage = null;
            this.Update();
        }

        public void ShowInvalidMoveMessage()
        {
            MessageBox.Show("Invalid Move");
        }

        public void SwitchBoldLabel(string i_Name)
        {
            if (i_Name.Equals(labelPlayer1.Name))
            {
                labelPlayer1.Font = new Font(Font, FontStyle.Bold);
                labelPlayer2.Font = new Font(Font, FontStyle.Regular);
            }
            else
            {
                labelPlayer1.Font = new Font(Font, FontStyle.Regular);
                labelPlayer2.Font = new Font(Font, FontStyle.Bold);
            }
            
            this.Update();
        }

        public void ChangeButtonBackgroundImage(Point i_Point)
        {
            Button button = m_ButtonBoard[i_Point.Y, i_Point.X];
            string shape;

            if (ButtonStateChanged != null)
            {
                shape = ButtonStateChanged.Invoke(i_Point);
                switch (shape)
                {
                    case "X":
                        button.BackgroundImage = Properties.Resources.blackMan;
                        break;
                    case "O":
                        button.BackgroundImage = Properties.Resources.whiteMan;
                        break;
                    case "Z":
                        button.BackgroundImage = Properties.Resources.blackKing;
                        break;
                    case "Q":
                        button.BackgroundImage = Properties.Resources.whiteKing;
                        break;
                    default: 
                        button.BackgroundImage = null;
                        break;
                }

                button.BackgroundImageLayout = ImageLayout.Stretch;
            }

            this.Update();
        }

        public void ShowEndGameMessage(string i_Message)
        {
            DialogResult dialogResult;
            dialogResult = MessageBox.Show(i_Message, "End of Round", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.No)
            {
                this.Close();
            }
            else
            {
                onButtonRestartGameClicked();
            }
        }

        private void onButtonRestartGameClicked()
        {
            if (ButtonRestartGameClicked != null)
            {
                ButtonRestartGameClicked.Invoke();
            }
        }
    }
}