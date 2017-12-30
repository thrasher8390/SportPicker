using System;
using System.Collections.Generic;
using FootballPicker.Parsers;

namespace FootballPicker.Analyzers
{
    public class Team
    {

        public Parsers.Team Name;

        /// <summary>
        /// This value can be negative. 0 means that the team often meets the spread
        /// > 0 Means the team often does better than the spread
        /// </summary>
        public Averager AverageSpreadDifference = new Averager();
        public Averager AverageSpreadMultiplier = new Averager();
        public Averager HomeGameSpreadMultiplier = new Averager();
        public Averager AwayGameSpreadMultiplier = new Averager();

        public Averager AverageScoreDifference = new Averager();
        public Averager HomeGameScoreDifferential = new Averager();
        public Averager AwayGameScoreDifferential = new Averager();

        public Averager WinLossMultiplier = new Averager();
        public Averager AveragePowerRankingMultiplier = new Averager();

        public Team(Parsers.Team teamName, List<Parsers.Game> gameList)
        {
            Name = teamName;
            foreach (Parsers.Game game in gameList)
            {
                if(game.IsFinished)
                {
                    analyzeSpread(game);
                    analyzeHomeVsAway(game);
                    analyzeWinLoss(game);
                    analyzePowerRanking(game);
                }
            }
        }

        private void analyzePowerRanking(Parsers.Game game)
        {
            double powerRankingDifference = game.GetPowerRankingDifference(this.Name);
            double scoreDifference = game.GetScoreDifference(this.Name);

            // This is meant to define the truth. Therefore if a team
            // was predicted to win and they win, the value will be negative
            // If a team was predicted to lose and they lost it'll result in a negative
            // That way later when we lose the value if a team is rank a certain way,
            // We can multiply by that games powerRankingDifference to get a confidence
            double value = scoreDifference / powerRankingDifference;
            AveragePowerRankingMultiplier.New(value);
        }

        private void analyzeWinLoss(Parsers.Game game)
        {
            if (game.GetActualWinner().Equals(this.Name))
            {
                WinLossMultiplier.New(1);
            }
            else
            {
                WinLossMultiplier.New(0);
            }
        }

        private void analyzeHomeVsAway(Parsers.Game game)
        {
            if(game.IsHome(this.Name))
            {
                HomeGameScoreDifferential.New(game.GetScoreDifference(this.Name));
            }
            else
            {
                AwayGameScoreDifferential.New(game.GetScoreDifference(this.Name));
            }
        }

        private void analyzeSpread(Parsers.Game game)
        {
            //returns + if Name won
            int scoreDifference = game.GetScoreDifference(this.Name);
            AverageScoreDifference.New(scoreDifference);
            //returns - if Name was favorite
            double spread = game.GetSpread(Name);

            //adjusted score with spread will be + if team beat the spread
            double newSpreadDifference = scoreDifference + spread;

            
            if (spread == 0)
            {

                spread = 1;
            }

            //this calcs fractionally how much the team beat the spread by. 
            // If you're favored to win by 7 but win by 14 then you have a
            // multiplier of 1
            double newSpreadMultiplier = newSpreadDifference / Math.Abs(spread);

            //This needs to be looked at
            AverageSpreadDifference.New(newSpreadDifference);
            AverageSpreadMultiplier.New(newSpreadMultiplier);

            if (game.IsHome(this.Name))
            {
                HomeGameSpreadMultiplier.New(newSpreadMultiplier);
            }
            else
            {
                AwayGameSpreadMultiplier.New(newSpreadMultiplier);
            }
        }
    }
}
