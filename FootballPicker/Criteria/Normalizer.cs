using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballPicker.Criteria
{
    public class Normalizer
    {
        private double? MinValue = null;
        private double? MaxValue = null;

        public Normalizer()
        {
        }

        internal void New(double spread)
        {
            if ((MinValue == null) ||
                (spread < MinValue))
            {
                MinValue = spread;
            }
            else if ((MaxValue == null) ||
                    (spread > MaxValue))
            {
                MaxValue = spread;
            }
        }

        internal double GetNormalized(double value)
        {
            double normalizedValue = 0;

            if (MinValue != null && MaxValue != null)
            {
                double denomenator = (double)MaxValue - (double)MinValue;

                if (denomenator == 0)
                {
                    normalizedValue = 0;
                }
                else
                {
                    normalizedValue = (value - (double)MinValue) / (double)(MaxValue - MinValue);
                }
            }

            return normalizedValue;
        }
    }
}
