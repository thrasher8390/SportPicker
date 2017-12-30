using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballPicker.Analyzers
{
    public class Averager
    {
        private double numberToAverage = 0;
        private double summation = 0;
        public Averager()
        {

        }

        internal void New(double newSpreadMultiplier)
        {
            numberToAverage++;
            summation += newSpreadMultiplier;
        }

        internal double Get()
        {
            double returnValue;
            if(numberToAverage >0)
            {
                returnValue = summation / numberToAverage;
            }
            else
            {
                returnValue = 0;
            }
            return returnValue;
        }
    }
}
