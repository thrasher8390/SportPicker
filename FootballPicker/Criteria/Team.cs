using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FootballPicker.Parsers;

namespace FootballPicker.Criteria
{
    public class Team
    {
        /// <summary>
        /// Predicts how much the team should win by based off the spread
        /// </summary>

        internal List<Criteria> CriteriaList = new List<Criteria>();
        internal Analyzers.Team TeamInfo;

        public Team(Analyzers.Team teamInfo, Parsers.Game game)
        {
            this.TeamInfo = teamInfo;
            double gameSpread = game.GetSpread(this.TeamInfo.Name);
            Boolean isHome = game.IsHome(TeamInfo.Name);

            CriteriaList.Add(new AverageScoreDifference(this.TeamInfo.AverageScoreDifference.Get()));
            CriteriaList.Add(new AverageSpreadDifference(this.TeamInfo.AverageSpreadDifference.Get(), gameSpread));
            CriteriaList.Add(new HomeVsAwayDifference(this.TeamInfo.HomeGameScoreDifferential.Get(), TeamInfo.AwayGameScoreDifferential.Get(), isHome));
            CriteriaList.Add(new HomeVsAwaySpreadMultiplier(this.TeamInfo.HomeGameSpreadMultiplier.Get(), TeamInfo.AwayGameSpreadMultiplier.Get(), isHome));
            CriteriaList.Add(new WinLossPercentage(this.TeamInfo.WinLossMultiplier.Get()));
            CriteriaList.Add(new AveragePowerRankingMultiplier(this.TeamInfo.AveragePowerRankingMultiplier.Get(), game.GetPowerRankingDifference(TeamInfo.Name)));
            CriteriaList.Add(new AverageSpreadMultiplier(this.TeamInfo.AverageSpreadMultiplier.Get(), gameSpread));
        }
    }
}
