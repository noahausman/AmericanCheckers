using System.Collections.Generic;

namespace GameLogic
{
    public class Player
    {
        public readonly eShape r_Shape;
        public readonly List<Checker> r_Checkers = new List<Checker>();
        public readonly string r_Name;
        public readonly bool r_Computer;
        private List<int> m_Scores = new List<int>();

        public Player(string i_Name, eShape i_Shape, bool i_IsComputer)
        {
            r_Name = i_Name;
            r_Computer = i_IsComputer;
            r_Shape = i_Shape;
        }

        internal void AddChecker(Checker i_NewChecker)
        {
            r_Checkers.Add(i_NewChecker);
        }

        internal void RemoveChecker(Checker i_CheckerToRemove)
        {
            r_Checkers.Remove(i_CheckerToRemove);
        }

        public int GetTotalScore()
        {
            int totalScore = 0;

            foreach (int score in m_Scores)
            {
                totalScore += score;
            }

            return totalScore;
        }

        internal void AddScore(int i_Score)
        {
            m_Scores.Add(i_Score);
        }

        internal bool IsCheckerOfPlayer(Checker i_Checker)
        {
            bool isCheckerOfPlayer = !true;

            if (r_Shape == i_Checker.r_Shape)
            {
                isCheckerOfPlayer = true;
            }

            return isCheckerOfPlayer;
        }
    }
}