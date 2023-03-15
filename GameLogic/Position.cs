namespace GameLogic
{
    public class Position
    {
        public int Col { get; private set; }

        public int Row { get; private set; }

        public Position(int i_Col, int i_Row)
        {
            Col = i_Col;
            Row = i_Row;
        }

        public bool Equal(Position i_PositionToCompare)
        {
            return Row == i_PositionToCompare.Row && Col == i_PositionToCompare.Col;
        }
    }
}