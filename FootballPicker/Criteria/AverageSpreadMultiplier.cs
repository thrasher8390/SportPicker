using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballPicker.Criteria
{
    public class AverageSpreadMultiplier : Criteria
    {
        
        private double value;
        protected static Normalizer normal = new Normalizer();
        public static void Reset()
        {
            normal = new Normalizer();
        }
        public AverageSpreadMultiplier(double averageSpreadMultiplier, double spread)
        {
            value = (averageSpreadMultiplier * Math.Abs(spread)) - spread;

            normal.New(value);
        }

        public override double GetNormalized()
        {
            return normal.GetNormalized(value);
        }
    }
}
