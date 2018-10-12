using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballPicker.Parsers
{
    public class Week
    {
        public Season season;
        internal int week;

        public bool IsRegularSeason
        {
            get
            {
                return (0 < week) && (week <= 17);
            }
        }

        public Week PreviousRegularSeasonWeek
        {
            get
            {
                Week week = this;
                do
                {
                    week--;
                }
                while (!week.IsRegularSeason);

                return week;
            }
        }

        public Week(int week, Season season)
        {
            this.week = week;
            this.season = season;
        }

        public Week(string week, Season season)
        {
            this.season = season;
            try
            {
                this.week = Convert.ToInt32(week);
            }
            catch
            {
                throw new FormatException("Week is in wrong format {" + week + "}");
            }
        }

        public override bool Equals(object obj)
        {
            return ((this.week == ((Week)obj).week) && (this.season.Equals(((Week)obj).season)));
        }

        public static Week operator -(Week obj, int i)
        {
            Week thisWeek;

            if (obj.week <= i)
            {
                thisWeek = new Week(obj.week + 21 - i, obj.season-1);
            }
            else
            {
                thisWeek = new Week(obj.week - i, obj.season);
            }

            return thisWeek;
        }

        public static Week operator --(Week obj)
        {
            return obj - 1;
        }

        public static bool operator <=(Week obj, Week obj2)
        {
            return obj.IsLessThan(obj2) || obj.Equals(obj2);
        }

        public static bool operator >=(Week obj, Week obj2)
        {
            return !obj.IsLessThan(obj2);
        }

        public static Week operator ++(Week obj)
        {
            Week thisWeek;

            if(obj.week + 1 >= 21)
            {
                thisWeek = new Week(obj.week - 21 + 1, obj.season + 1);
            }
            else
            {
                thisWeek = new Week(obj.week + 1, obj.season);
            }


            return thisWeek;
        }

        internal bool IsLessThan(Week week)
        {
            return (this.week < week.week || this.season.IsLessThan(week.season));
        }

        public override string ToString()
        {
            return week.ToString();
        }
    }
}
