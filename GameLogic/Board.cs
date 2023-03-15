using System;

namespace GameLogic
{
    public class Board
    {
        public readonly int r_Size;

        public event Action<Position, Position> CheckerMoved;

        public event Action<Position> CheckerDeleted;

        public event Action<Position> CheckerBecameKing;

        internal Checker[,] CheckerBoard { get; private set; }

        public Board(int i_BoardSize)
        {
            r_Size = i_BoardSize;
            CheckerBoard = new Checker[i_BoardSize, i_BoardSize];
        }

        private void onCheckerMoved(Position i_CurrentPos, Position i_NextPos)
        {
            CheckerMoved.Invoke(i_CurrentPos, i_NextPos);
        }

        private void onCheckerDeleted(Position i_PositionOfDeletedChecker)
        {
            CheckerDeleted.Invoke(i_PositionOfDeletedChecker);
        }
        
        private void onCheckerBecameKing(Position i_Position)
        {
            CheckerBecameKing(i_Position);
        }

        internal void DoWhenCheckerBecameKing(Position i_Position)
        {
            onCheckerBecameKing(i_Position);
        }

        internal void DoWhenCheckerMoved(Position i_CurrentPos, Position i_NextPos)
        {
            onCheckerMoved(i_CurrentPos, i_NextPos);
        }

        internal void DoWhenCheckerDeleted(Position i_PositionOfDeletedChecker)
        {
            onCheckerDeleted(i_PositionOfDeletedChecker);
        }

        internal void InitializeCheckerBoard(Player i_Player1, Player i_Player2)
        {
            Checker newChecker;
            int row, col, numOfRowsToPlaceCheckers;
            Position newPosition;
            
            CheckerBoard = new Checker[r_Size, r_Size];
            if (r_Size <= 6)
            {
                numOfRowsToPlaceCheckers = 2;
            }
            else
            {
                numOfRowsToPlaceCheckers = 3;
            }

            for (int i = 0; i < numOfRowsToPlaceCheckers; i++)
            {
                for (int j = 0; j < r_Size; j += 2)
                {
                    row = r_Size - 1 - i;
                    col = j + 1;
                    if (i % 2 == 0)
                    {
                        newPosition = new Position(j, row);
                        newChecker = new Checker(newPosition, i_Player1.r_Shape, i_Player1.r_Shape == eShape.X);
                        i_Player1.AddChecker(newChecker);
                        CheckerBoard[row, j] = newChecker;
                        newPosition = new Position(col, i);
                        newChecker = new Checker(newPosition, i_Player2.r_Shape, i_Player2.r_Shape == eShape.X);
                        CheckerBoard[i, col] = newChecker;
                        i_Player2.AddChecker(newChecker);
                    }
                    else
                    {
                        newPosition = new Position(j, i);
                        newChecker = new Checker(newPosition, i_Player2.r_Shape, i_Player2.r_Shape == eShape.X);
                        CheckerBoard[i, j] = newChecker;
                        i_Player2.AddChecker(newChecker);
                        newPosition = new Position(col, row);
                        newChecker = new Checker(newPosition, i_Player1.r_Shape, i_Player1.r_Shape == eShape.X);
                        i_Player1.AddChecker(newChecker);
                        CheckerBoard[row, col] = newChecker;
                    }
                }
            }
        }

        public Checker GetCheckerByPosition(Position i_Position)
        {
            return CheckerBoard[i_Position.Row, i_Position.Col];
        }

        internal Checker DeleteChecker(Position i_Position)
        {
            Checker checkerToDelete = CheckerBoard[i_Position.Row, i_Position.Col];

            CheckerBoard[i_Position.Row, i_Position.Col] = null;

            return checkerToDelete;
        }

        internal void MoveChecker(Position i_CurrentPos, Position i_NextPos)
        {
            Checker CheckerToMove = GetCheckerByPosition(i_CurrentPos);

            CheckerBoard[i_CurrentPos.Row, i_CurrentPos.Col] = null;
            CheckerBoard[i_NextPos.Row, i_NextPos.Col] = CheckerToMove;
            CheckerToMove.Position = i_NextPos;
        }
    }
}