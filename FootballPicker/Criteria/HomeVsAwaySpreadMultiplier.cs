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

        public HomeVsAwaySpreadMultiplier(double homeGameSpreadMultiplier, double awayGameSpreadMultiplier, double spread, bool isHome)
        {
            this.isHome = isHome;

            this.HomeGameSpreadMultiplier = (homeGameSpreadMultiplier * Math.Abs(spread)) - spread;
            homeScore.New(HomeGameSpreadMultiplier);
       
            this.AwayGameSpreadMultiplier = (awayGameSpreadMultiplier * Math.Abs(spread)) - spread;
            awayScore.New(AwayGameSpreadMultiplier);
            
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
