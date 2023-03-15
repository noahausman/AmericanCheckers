using System;
using System.Collections.Generic;

namespace GameLogic
{
    public class Checker
    {
        public readonly eShape r_Shape;
        public readonly bool r_MovingUp;

        public eType Type { get; set; } 

        public Position Position { get; set; } 

        public List<Position> ValidMoves { get; set; }

        public List<Position> ValidJumps { get; set; }

        public Checker(Position i_PositionInBoard, eShape i_Shape, bool i_MovingUp)
        {
            Position = i_PositionInBoard;
            r_MovingUp = i_MovingUp;
            r_Shape = i_Shape;
            Type = eType.Man;
            ValidMoves = new List<Position>();
            ValidJumps = new List<Position>();
        }

        internal void UpdateValidMoves(Checker[,] i_Board, int i_BoardSize)
        {
            ValidMoves = new List<Position>();
            if (Type == eType.Man)
            {
                if (r_MovingUp)
                {
                    updateValidMovesUp(i_Board, i_BoardSize);
                }
                else
                {
                    updateValidMovesDown(i_Board, i_BoardSize);
                }
            }
            else
            {
                updateValidMovesUp(i_Board, i_BoardSize);
                updateValidMovesDown(i_Board, i_BoardSize);
            }
        }

        private void updateValidMovesUp(Checker[,] i_Board, int i_BoardSize)
        {
            if (Position.Col < i_BoardSize - 1 && Position.Row > 0 && i_Board[Position.Row - 1, Position.Col + 1] == null)
            {
                ValidMoves.Add(new Position(Position.Col + 1, Position.Row - 1));
            }

            if (Position.Col > 0 && Position.Row > 0 && i_Board[Position.Row - 1, Position.Col - 1] == null)
            {
                ValidMoves.Add(new Position(Position.Col - 1, Position.Row - 1));
            }
        }

        private void updateValidMovesDown(Checker[,] i_Board, int i_BoardSize)
        {
            if (Position.Col < i_BoardSize - 1 && Position.Row < i_BoardSize - 1 && i_Board[Position.Row + 1, Position.Col + 1] == null)
            {
                ValidMoves.Add(new Position(Position.Col + 1, Position.Row + 1));
            }

            if (Position.Col > 0 && Position.Row < i_BoardSize - 1 && i_Board[Position.Row + 1, Position.Col - 1] == null)
            {
                ValidMoves.Add(new Position(Position.Col - 1, Position.Row + 1));
            }
        }

        internal void UpdateValidJumps(Checker[,] i_Board, int i_BoardSize)
        {
            ValidJumps = new List<Position>();
            if (Type == eType.Man)
            {
                if (r_MovingUp)
                {
                    updateValidJumpsUp(i_Board, i_BoardSize);
                }
                else
                {
                    updateValidJumpsDown(i_Board, i_BoardSize);
                }
            }
            else
            {
                updateValidJumpsUp(i_Board, i_BoardSize);
                updateValidJumpsDown(i_Board, i_BoardSize);
            }
        }

        private void updateValidJumpsUp(Checker[,] i_Board, int i_BoardSize)
        {
            if (Position.Col < i_BoardSize - 1 && Position.Row > 0 && i_Board[Position.Row - 1, Position.Col + 1] != null)
            {
                if (i_Board[Position.Row - 1, Position.Col + 1].r_Shape != r_Shape)
                {
                    if (Position.Col < i_BoardSize - 2 && Position.Row > 1 && i_Board[Position.Row - 2, Position.Col + 2] == null)
                    {
                        ValidJumps.Add(new Position(Position.Col + 2, Position.Row - 2));
                    }
                }
            }

            if (Position.Col > 0 && Position.Row > 0 && i_Board[Position.Row - 1, Position.Col - 1] != null)
            {
                if(i_Board[Position.Row - 1, Position.Col - 1].r_Shape != r_Shape)
                {
                    if (Position.Col > 1 && Position.Row > 1 && i_Board[Position.Row - 2, Position.Col - 2] == null)
                    {
                        ValidJumps.Add(new Position(Position.Col - 2, Position.Row - 2));
                    }
                }
            }
        }

        private void updateValidJumpsDown(Checker[,] i_Board, int i_BoardSize)
        {
            if (Position.Col < i_BoardSize - 1 && Position.Row < i_BoardSize - 1 && i_Board[Position.Row + 1, Position.Col + 1] != null)
            {
                if (i_Board[Position.Row + 1, Position.Col + 1].r_Shape != r_Shape)
                {
                    if (Position.Col < i_BoardSize - 2 && Position.Row < i_BoardSize - 2 && i_Board[Position.Row + 2, Position.Col + 2] == null)
                    {
                        ValidJumps.Add(new Position(Position.Col + 2, Position.Row + 2));
                    }
                }
            }

            if (Position.Col > 0 && Position.Row < i_BoardSize - 1 && i_Board[Position.Row + 1, Position.Col - 1] != null)
            {
                if (i_Board[Position.Row + 1, Position.Col - 1].r_Shape != r_Shape)
                { 
                    if (Position.Col > 1 && Position.Row < i_BoardSize - 2 && i_Board[Position.Row + 2, Position.Col - 2] == null)
                    {
                        ValidJumps.Add(new Position(Position.Col - 2, Position.Row + 2));
                    }
                }
            }
        }

        internal Position GetRandomJump()
        {
            Random randomIndex = new Random();

            return ValidJumps[randomIndex.Next(ValidJumps.Count)];
        }

        internal Position GetRandomMove()
        {
            Random randomIndex = new Random();

            return ValidMoves[randomIndex.Next(ValidMoves.Count)];
        }

        public override string ToString()
        {
            string shape;

            if (Type == eType.King)
            {
                shape = ((eKingShape)((int)r_Shape)).ToString();
            }
            else
            {
                shape = r_Shape.ToString();
            }

            return shape;
        }
    }
}