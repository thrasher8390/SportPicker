using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballPicker.Criteria
{
    public class WinLossPercentage : Criteria
    {
        private double value;
        protected static Normalizer normal = new Normalizer();
        public static void Reset()
        {
            normal = new Normalizer();
        }
        public WinLossPercentage(double winLossPercentage)
        {
            value = winLossPercentage;
            normal.New(winLossPercentage);
        }

        public override double GetNormalized()
        {
            return normal.GetNormalized(value);
        }
    }
}
