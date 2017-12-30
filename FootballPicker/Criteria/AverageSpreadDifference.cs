using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballPicker.Criteria
{
    public class AverageSpreadDifference : Criteria
    {
        private double value;
        protected static Normalizer normal = new Normalizer();
        public static void Reset()
        {
            normal = new Normalizer();
        }
        public AverageSpreadDifference(double averageSpreadDifference, double spread)
        {
            value = averageSpreadDifference;
            normal.New(averageSpreadDifference - spread);
        }

        public override double GetNormalized()
        {
            return normal.GetNormalized(value);
        }
    }
}
