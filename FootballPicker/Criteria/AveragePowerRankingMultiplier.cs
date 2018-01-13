using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballPicker.Criteria
{
    public class AveragePowerRankingMultiplier : Criteria
    {
        private double value = 0;
        protected static Normalizer normal = new Normalizer();
        public AveragePowerRankingMultiplier(double averagePowerRankingMultiplier, double powerRankingDifference)
        {
            value = ((averagePowerRankingMultiplier * Math.Abs(powerRankingDifference)) + powerRankingDifference);
            
            normal.New(value);
        }

        public override double GetNormalized()
        {
            return normal.GetNormalized(value);
        }

        public static void Reset()
        {
            normal = new Normalizer();
        }
    }
}
