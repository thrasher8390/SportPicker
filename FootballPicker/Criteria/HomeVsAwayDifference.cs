using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballPicker.Criteria
{
    public class HomeVsAwayDifference: Criteria
    {
        private static Normalizer homeScore = new Normalizer();
        private static Normalizer awayScore = new Normalizer();

        private double homeGameScoreDifferential;
        private double awayGameScoreDifferential;
        private bool isHome;

        public HomeVsAwayDifference(double homeGameScoreDifferential, double awayGameScoreDifferential, bool isHome)
        {
            this.homeGameScoreDifferential = homeGameScoreDifferential;
            homeScore.New(homeGameScoreDifferential);
            this.awayGameScoreDifferential = awayGameScoreDifferential;
            awayScore.New(awayGameScoreDifferential);
            this.isHome = isHome;
        }

        public override double GetNormalized()
        {
            double normalizedValue;
            if(isHome)
            {
                normalizedValue = homeScore.GetNormalized(homeGameScoreDifferential);
            }
            else
            {
                normalizedValue = awayScore.GetNormalized(awayGameScoreDifferential);
            }

            return normalizedValue;
        }

        public static void Reset()
        {
            homeScore = new Normalizer();
            awayScore = new Normalizer();
        }
    }
}
