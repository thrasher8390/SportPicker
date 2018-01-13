using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FootballPicker.Parsers
{
    public class Season
    {
        private int season;

        public Season(int year)
        {
            season = year;
        }

        public Season(string year)
        {
            try
            {
                if(year.Equals("2017"))
                {
                    season = 2017;
                }
            }
            catch
            {
                throw new FormatException("Year was in incorrect format {" + year + "}");
            }
        }

        public bool Equals(Season obj)
        {
            return this.season.Equals(obj.season);
        }

        public bool IsLessThan(Season obj)
        {
            return this.season < obj.season;
        }

        public static Season operator -(Season obj, int i)
        {
            return new Season(obj.season - 1);
        }

        public override string ToString()
        {
            return season.ToString();
        }
    }
}
