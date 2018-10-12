using FootballPicker.InternetConnection;
using FootballPicker.Parsers;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballPicker.DataBaseManager
{
    public class UpdateDatabase
    {
        public UpdateDatabase(DataBase database, Week week)
        {
            Week thisWeek = new Week(6, new Season(2018));
            
            string gameAndSpreadSite = "https://www.cbssports.com/nfl/features/writers/expert/picks/against-the-spread/" + week.ToString();
            GamesAndSpread.Update(database, thisWeek, gameAndSpreadSite);

            Week previousWeek = thisWeek - 1;
            string scoreSite = "https://www.cbssports.com/nfl/features/writers/expert/picks/against-the-spread/" + previousWeek.ToString();
            GamesAndSpread.Update(database, previousWeek, scoreSite);

            string powerRankingSite = "http://www.nfl.com/news/story/0ap3000000972240/article/nfl-power-rankings-week-6-saints-enter-top-three-eagles-dip";
            if (week.Equals(thisWeek))
            {
                PowerRankings.Update(database, thisWeek, powerRankingSite);
            }
            else
            {
                Debugger.Break();
            }
        }
    }
}
