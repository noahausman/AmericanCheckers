using System.Text;
using System.Timers;
using System.Drawing;
using GameLogic;
using GameUI;

namespace GameControl
{
    public class Controller
    {
        private Game m_Game;
        private FormGame m_FormGame;

        public Controller(Game i_Game, FormGame i_FormGame)
        {
            m_Game = i_Game;
            m_FormGame = i_FormGame;
            setListeners();
            m_FormGame.InitializeButtonBoard();
        }

        public void Run()
        {
            runSingleGame();
        }

        private void runSingleGame()
        {
            m_Game.InitializeGame();
            m_FormGame.InitializeButtonBackgroundImage();
            m_FormGame.SetLabelText(m_Game.Player1.r_Name, m_Game.Player1.GetTotalScore());
            m_FormGame.SetLabelText(m_Game.Player2.r_Name, m_Game.Player2.GetTotalScore());
            m_FormGame.SwitchBoldLabel(m_Game.CurrentPlayer.r_Name);
            if(m_Game.Round == 0)
            {
                m_FormGame.ShowDialog();
            }
        }

        private void formGame_MoveWasMade(Point i_PrevPoint, Point i_NextPoint)
        {
            bool jump, anotherMove;
            Position prevPos, nextPov;

            prevPos = convertPointToPosition(i_PrevPoint);
            nextPov = convertPointToPosition(i_NextPoint);
            if (m_Game.IsValidMove(prevPos, nextPov, out jump))
            {
                m_Game.MakeSingleMove(prevPos, nextPov, out anotherMove, jump);
                if (!anotherMove)
                {
                    switchPlayers();
                    if (!m_Game.HasValidMoves())
                    {
                        endSingleGame();
                    }
                    else if (m_Game.CurrentPlayer.r_Computer)
                    {
                        m_Game.MakeComputerMove();
                        switchPlayers();
                        if (!m_Game.HasValidMoves())
                        {
                            endSingleGame();
                        }
                    }
                }
            }
            else
            {
                m_FormGame.ShowInvalidMoveMessage();
            }
        }

        private void endSingleGame()
        {
            int player1Score, player2Score;
            StringBuilder message = new StringBuilder();

            m_Game.UpdatePlayersScore();
            m_Game.Round++;
            player1Score = m_Game.Player1.GetTotalScore();
            player2Score = m_Game.Player2.GetTotalScore();
            if (player1Score > player2Score)
            {
                message = message.AppendFormat("{0} Won!", m_Game.Player1.r_Name);
            }
            else if (player1Score < player2Score)
            {
                message = message.AppendFormat("{0} Won!", m_Game.Player2.r_Name);
            }
            else
            {
                message = message.Append("Tie!");
            }

            message.AppendLine();
            message.Append("Another Round?");
            m_FormGame.ShowEndGameMessage(message.ToString());
        }

        private void switchPlayers()
        {
            m_Game.SwitchCurrentPlayer();
            m_FormGame.SwitchBoldLabel(m_Game.CurrentPlayer.r_Name);
        }

        private string button_StateChanged(Point i_Point)
        {
            string shape;
            Position position = convertPointToPosition(i_Point);
            Checker checker;

            if ((checker = m_Game.Board.GetCheckerByPosition(position)) == null)
            {
                shape = string.Empty;
            }
            else
            {
                shape = checker.ToString();
            }

            return shape;
        }

        private Position convertPointToPosition(Point i_Point)
        {
            return new Position(i_Point.X, i_Point.Y);
        }

        private Point convertPositionToPoint(Position i_Position)
        {
            return new Point(i_Position.Col, i_Position.Row);
        }

        private void game_CheckerMoved(Position i_PrevPos, Position i_NextPos)
        {
            Point prevPoint = convertPositionToPoint(i_PrevPos), nextPoint = convertPositionToPoint(i_NextPos);

            m_FormGame.SwitchButtonBackgroundImage(prevPoint, nextPoint);
        }

        private void game_CheckerDeleted(Position i_Pos)
        {
            Point point = convertPositionToPoint(i_Pos);

            m_FormGame.DeleteButtonBackgroundImage(point);
        }
        
        private bool formGame_FirstStepOfMoveWasMade(Point i_Point)
        {
            Position position = convertPointToPosition(i_Point);

            return m_Game.Board.GetCheckerByPosition(position) != null;
        }

        private void game_CheckerBecameKing(Position i_Position)
        {
            Point point = convertPositionToPoint(i_Position);

            m_FormGame.ChangeButtonBackgroundImage(point);
        }

        private void formGame_ButtonRestartClicked()
        {
            this.Run();
        }

        private void setListeners()
        {
            m_FormGame.ButtonStateChanged += button_StateChanged;
            m_FormGame.MoveWasMade += formGame_MoveWasMade;
            m_FormGame.FirstStepOfMoveWasMade += formGame_FirstStepOfMoveWasMade;
            m_FormGame.ButtonRestartGameClicked += formGame_ButtonRestartClicked;
            m_Game.Board.CheckerMoved += game_CheckerMoved;
            m_Game.Board.CheckerDeleted += game_CheckerDeleted;
            m_Game.Board.CheckerBecameKing += game_CheckerBecameKing;
        }
    }
}