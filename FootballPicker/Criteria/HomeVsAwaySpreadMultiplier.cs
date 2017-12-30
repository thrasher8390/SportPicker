using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballPicker.Criteria
{
    public class HomeVsAwaySpreadMultiplier: Criteria
    {
        private static Normalizer homeScore = new Normalizer();
        private static Normalizer awayScore = new Normalizer();

        private double HomeGameSpreadMultiplier;
        private double AwayGameSpreadMultiplier;
        private bool isHome;

        public HomeVsAwaySpreadMultiplier(double homeGameSpreadMultiplier, double awayGameSpreadMultiplier, bool isHome)
        {
            this.HomeGameSpreadMultiplier = homeGameSpreadMultiplier;
            homeScore.New(homeGameSpreadMultiplier);
            this.AwayGameSpreadMultiplier = awayGameSpreadMultiplier;
            awayScore.New(awayGameSpreadMultiplier);
            this.isHome = isHome;
        }

        public override double GetNormalized()
        {
            double normalizedValue;
            if(isHome)
            {
                normalizedValue = homeScore.GetNormalized(this.HomeGameSpreadMultiplier);
            }
            else
            {
                normalizedValue = awayScore.GetNormalized(this.AwayGameSpreadMultiplier);
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
