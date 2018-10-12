using FootballPicker.Criteria;
using System;
using System.Collections.Generic;

namespace FootballPicker.Analyzers
{
    public class Week
    {
        public List<Game> GameAnalyzerList = new List<Game>();
        private Parsers.Week week;
        public Week(List<Team> teamAnalyzerList, List<Parsers.Game> thisWeeksGames)
        {
            //This is hacky but allows us to debug
            if (thisWeeksGames.Count > 0)
            {
                this.week = thisWeeksGames[0].Week;
            }

            //Reset all criteria for this week!
            AveragePowerRankingMultiplier.Reset();
            AverageScoreDifference.Reset();
            AverageSpreadDifference.Reset();
            AverageSpreadMultiplier.Reset();
            HomeVsAwayDifference.Reset();
            HomeVsAwaySpreadMultiplier.Reset();
            WinLossPercentage.Reset();

            //Figure out all of the games and analyze each team
            foreach(Parsers.Game game in thisWeeksGames)
            {
                Team team1 = teamAnalyzerList.Find(team => game.AwayTeam.Equals(team.Name));
                Team team2 = teamAnalyzerList.Find(team => game.HomeTeam.Equals(team.Name));

                GameAnalyzerList.Add(new Game(team1, team2, game));
            }
        }
    }
}
 