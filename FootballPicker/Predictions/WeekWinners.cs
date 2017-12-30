using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballPicker.Predictions
{
    public class WeekWinners
    {
        public List<Predictions.GameWinner> GameWinners = new List<Predictions.GameWinner>();

        public WeekWinners(Analyzers.Week thisWeek, List<double> criteriaRating)
        {

            //Now lets chose the winner of each game
            foreach (Analyzers.Game game in thisWeek.GameAnalyzerList)
            {
                //todo need to change this so that GameWinners is a class that we pass the games into. that way we have a Gamewinner(game). that calculates the game instead of game havin ga getWinner.
                GameWinners.Add(new GameWinner(game, criteriaRating));
            }

            GameWinners.Sort((q, p) => p.Confidence.CompareTo(q.Confidence));
        }

        public int GetLeagueScore()
        {
            int totalScore = 0;
            for (int i = 0; i < GameWinners.Count; i++)
            {
                if(GameWinners[i].IsPredictionCorrect())
                {
                    totalScore += (16 - i);
                }
            }

            return totalScore;
        }
    }
}
