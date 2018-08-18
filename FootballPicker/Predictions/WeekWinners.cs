using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballPicker.Predictions
{
    public class WeekWinners : List<GameWinner>
    {

        public WeekWinners(Analyzers.Week thisWeek, List<double> criteriaRating)
        {

            //Now lets chose the winner of each game
            foreach (Analyzers.Game game in thisWeek.GameAnalyzerList)
            {
                //todo need to change this so that GameWinners is a class that we pass the games into. that way we have a Gamewinner(game). that calculates the game instead of game havin ga getWinner.
                this.Add(new GameWinner(game, criteriaRating));
            }

            this.Sort((q, p) => p.Confidence.CompareTo(q.Confidence));
        }

        public int GetLeagueScore()
        {
            int totalScore = 0;
            for (int i = 0; i < this.Count; i++)
            {
                if(this[i].IsPredictionCorrect())
                {
                    totalScore += (16 - i);
                }
            }

            return totalScore;
        }
    }
}
