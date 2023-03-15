using System;
using System.Collections.Generic;
using System.Threading;

namespace GameLogic
{
    public class Game
    {
        public Player CurrentPlayer { get; private set; }

        public Board Board { get; private set; }

        public Player Player1 { get; private set; }

        public Player Player2 { get; private set; }

        public int Round { get; set; }

        public Game(string i_Player1Name, string i_Player2Name, int i_BoardSize, bool i_TwoPlayers)
        {
            bool computer = true;

            Board = new Board(i_BoardSize);
            Player1 = new Player(i_Player1Name, eShape.X, !computer);
            if (i_TwoPlayers)
            {
                Player2 = new Player(i_Player2Name, eShape.O, !computer);
            }
            else
            {
                Player2 = new Player(i_Player2Name, eShape.O, computer);
            }
        }

        public void InitializeGame()
        {
            CurrentPlayer = Player1;
            Player1.r_Checkers.Clear();
            Player2.r_Checkers.Clear();
            Board.InitializeCheckerBoard(Player1, Player2);
            updateAllCheckersMoves();
        }

        public void UpdatePlayersScore()
        {
            int checkersLeftToPlayer1, checkersLeftToPlayer2, absScore;

            checkersLeftToPlayer1 = Player1.r_Checkers.Count + (countKingsOfPlayer(Player1.r_Checkers) * 3);
            checkersLeftToPlayer2 = Player2.r_Checkers.Count + (countKingsOfPlayer(Player2.r_Checkers) * 3);
            absScore = Math.Abs(checkersLeftToPlayer1 - checkersLeftToPlayer2);
            if (checkersLeftToPlayer1 > checkersLeftToPlayer2)
            {
                Player1.AddScore(absScore);
                Player2.AddScore(0);
            }
            else if (checkersLeftToPlayer1 < checkersLeftToPlayer2)
            {
                Player2.AddScore(absScore);
                Player1.AddScore(0);
            }
            else
            {
                Player1.AddScore(absScore);
                Player2.AddScore(absScore);
            }
        }

        private static int countKingsOfPlayer(List<Checker> i_Checkers)
        {
            int numOfKings = 0;

            foreach (Checker checker in i_Checkers)
            {
                if (checker.Type == eType.King)
                {
                    numOfKings++;
                }
            }

            return numOfKings;
        }

        public bool HasValidMoves()
        {
            bool hasValidMove = false;

            foreach (Checker checker in CurrentPlayer.r_Checkers)
            {
                if (checker.ValidJumps.Count > 0 || checker.ValidMoves.Count > 0)
                {
                    hasValidMove = true;
                    break;
                }
            }

            return hasValidMove;
        }

        public bool IsValidMove(Position i_CurrentPos, Position i_NextPos, out bool o_IsJump)
        {
            bool isValid = false, flag = false;
            o_IsJump = false;
            Checker currentChecker = Board.GetCheckerByPosition(i_CurrentPos), nextPosChecker = Board.GetCheckerByPosition(i_NextPos);

            if (currentChecker == null || currentChecker.r_Shape != CurrentPlayer.r_Shape || nextPosChecker != null)
            {
                isValid = false;
            }
            else
            {
                foreach (Position jumpPosition in currentChecker.ValidJumps)
                {
                    if (flag)
                    {
                        break;
                    }

                    if (jumpPosition.Equal(i_NextPos))
                    {
                        flag = true;
                        isValid = true;
                        o_IsJump = true;
                    }
                }

                foreach (Checker checker in CurrentPlayer.r_Checkers)
                {
                    if (flag)
                    {
                        break;
                    }

                    if (checker.ValidJumps.Count > 0)
                    {
                        flag = true;
                        isValid = false;
                    }
                }

                foreach (Position movePosition in currentChecker.ValidMoves)
                {
                    if (flag)
                    {
                        break;
                    }

                    if (movePosition.Equal(i_NextPos))
                    {
                        flag = true;
                        isValid = true;
                    }
                }
            }

            return isValid;
        }

        public void MakeSingleMove(Position i_CurrentPos, Position i_NextPos, out bool o_HasAnotherMove, bool i_IsJump)
        {
            Checker currentChecker = Board.GetCheckerByPosition(i_CurrentPos);
            bool isBecomingKing;

            o_HasAnotherMove = true;
            isBecomingKing = isCheckerBecomingKing(currentChecker, i_NextPos);
            if (isBecomingKing)
            {
                currentChecker.Type = eType.King;
            }

            Board.MoveChecker(i_CurrentPos, i_NextPos);
            if (i_IsJump)
            {
                deleteChecker(i_CurrentPos, i_NextPos);
            }

            updateAllCheckersMoves();
            o_HasAnotherMove = i_IsJump && currentChecker.ValidJumps.Count > 0 && !isBecomingKing;
            Board.DoWhenCheckerMoved(i_CurrentPos, i_NextPos);
            if (isBecomingKing)
            {
                Board.DoWhenCheckerBecameKing(i_NextPos);
            }
        }

        public void MakeComputerMove()
        {
            Position prevPos, nextPos;
            bool jump, anotherMove;

            generateMoveForComputer(out prevPos, out nextPos, out jump);
            Thread.Sleep(1000);
            MakeSingleMove(prevPos, nextPos, out anotherMove, jump);
            while (anotherMove)
            {
                prevPos = nextPos;
                nextPos = generateAnotherJumpForComputer(prevPos);
                Thread.Sleep(1000);
                MakeSingleMove(prevPos, nextPos, out anotherMove, jump);
            }
        }

        private void updateAllCheckersMoves()
        {
            foreach (Checker checker1 in Player1.r_Checkers)
            {
                checker1.UpdateValidJumps(Board.CheckerBoard, Board.r_Size);
                checker1.UpdateValidMoves(Board.CheckerBoard, Board.r_Size);
            }

            foreach (Checker checker2 in Player2.r_Checkers)
            {
                checker2.UpdateValidJumps(Board.CheckerBoard, Board.r_Size);
                checker2.UpdateValidMoves(Board.CheckerBoard, Board.r_Size);
            }
        }

        private void deleteChecker(Position i_CurrentPos, Position i_NextPos)
        {
            Position positionOfCheckerToDelete;
            Checker deletedChecker;
            int colOfCheckerToDelete = (i_CurrentPos.Col + i_NextPos.Col) / 2, rowOfCheckerToDelete = (i_CurrentPos.Row + i_NextPos.Row) / 2;

            positionOfCheckerToDelete = new Position(colOfCheckerToDelete, rowOfCheckerToDelete);
            deletedChecker = Board.DeleteChecker(positionOfCheckerToDelete);
            if (Player1.IsCheckerOfPlayer(deletedChecker))
            {
                Player1.RemoveChecker(deletedChecker);
            }
            else
            {
                Player2.RemoveChecker(deletedChecker);
            }

            Board.DoWhenCheckerDeleted(positionOfCheckerToDelete);
        }

        private bool isCheckerBecomingKing(Checker i_Checker, Position i_NextPos)
        {
            return i_Checker.Type == eType.Man && ((i_Checker.r_Shape == eShape.X && i_NextPos.Row == 0) || (i_Checker.r_Shape == eShape.O && i_NextPos.Row == Board.r_Size - 1));
        }

        private void generateMoveForComputer(out Position o_CurrentPos, out Position o_NextPos, out bool o_IsJump)
        {
            List<Checker> canJumpCheckers = new List<Checker>(), canMoveCheckers = new List<Checker>();
            Checker chosenChecker;
            Random randomIndex = new Random();

            o_IsJump = false;
            o_CurrentPos = null;
            o_NextPos = null;
            foreach (Checker checker in CurrentPlayer.r_Checkers)
            {
                if (checker.ValidJumps.Count > 0)
                {
                    canJumpCheckers.Add(checker);
                }

                if (checker.ValidMoves.Count > 0)
                {
                    canMoveCheckers.Add(checker);
                }
            }

            if (canJumpCheckers.Count > 0)
            {
                chosenChecker = canJumpCheckers[randomIndex.Next(canJumpCheckers.Count)];
                o_NextPos = chosenChecker.GetRandomJump();
                o_IsJump = true;
            }
            else
            {
                chosenChecker = canMoveCheckers[randomIndex.Next(canMoveCheckers.Count)];
                o_NextPos = chosenChecker.GetRandomMove();
            }

            o_CurrentPos = chosenChecker.Position;
        }

        private Position generateAnotherJumpForComputer(Position i_CurrentPos)
        {
            Checker checkerToJump = Board.GetCheckerByPosition(i_CurrentPos);

            return checkerToJump.GetRandomJump();
        }

        public void SwitchCurrentPlayer()
        {
            if (CurrentPlayer.r_Shape == eShape.X)
            {
                CurrentPlayer = Player2;
            }
            else
            {
                CurrentPlayer = Player1;
            }
        }
    }
}