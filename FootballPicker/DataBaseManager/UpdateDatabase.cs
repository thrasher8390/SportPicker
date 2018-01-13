using FootballPicker.InternetConnection;
using FootballPicker.Parsers;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballPicker.DataBaseManager
{
    public class UpdateDatabase
    {
        public UpdateDatabase(DataBase database)
        {
            Week staticWeek = new Week(19, new Season(2017));
            string powerRankingSite = "http://www.nfl.com/news/story/0ap3000000904608/article/nfl-power-rankings-saints-falcons-soar-into-divisional-round";

            PowerRankings.Update(database, staticWeek, powerRankingSite);
        }
    }
}
