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
    public class PowerRankings
    {
        public static void Update(DataBase database, Week week, string powerRankingSite)
        {
            if (powerRankingSite.Contains("nfl.com"))
            {
                //From NFL
                pullPowerRankingsFromNFL(database, week, powerRankingSite);
            }
            else if (powerRankingSite.Contains("cbssports.com"))
            {
                //From CBS
                pullPowerRankingsFromCBS(database, week, powerRankingSite);
            }
        }

        private static void pullPowerRankingsFromNFL(DataBase database, Week week, string powerRankingSite)
        {
            List<string> websites2017 = new List<string>();
            websites2017.Add("http://www.nfl.com/news/story/0ap3000000839325/article/nfl-power-rankings-week-1-seahawks-surging-toward-top-spot");
            websites2017.Add("http://www.nfl.com/news/story/0ap3000000843960/article/nfl-power-rankings-week-2-packers-chiefs-take-top-two-slots");
            websites2017.Add("http://www.nfl.com/news/story/0ap3000000847983/article/nfl-power-rankings-week-3-kansas-city-chiefs-hit-no-1-spot");
            websites2017.Add("http://www.nfl.com/news/story/0ap3000000851973/article/nfl-power-rankings-week-4-titans-hit-top-five-steelers-plunge");
            websites2017.Add("http://www.nfl.com/news/story/0ap3000000855928/article/nfl-power-rankings-week-5-lions-eagles-soar-into-top-three");
            websites2017.Add("http://www.nfl.com/news/story/0ap3000000859501/article/nfl-power-rankings-week-6-philadelphia-eagles-fly-up-to-no-2");
            websites2017.Add("http://www.nfl.com/news/story/0ap3000000862720/article/nfl-power-rankings-week-7-los-angeles-rams-enter-top-three");
            websites2017.Add("http://www.nfl.com/news/story/0ap3000000866435/article/nfl-power-rankings-week-8-philadelphia-eagles-hit-no-1-spot");
            websites2017.Add("http://www.nfl.com/news/story/0ap3000000870289/article/nfl-power-rankings-week-9-houston-texans-rise-despite-loss");
            websites2017.Add("http://www.nfl.com/news/story/0ap3000000873681/article/nfl-power-rankings-week-10-saints-hit-top-five-cowboys-rise");
            websites2017.Add("http://www.nfl.com/news/story/0ap3000000877334/article/nfl-power-rankings-week-11-patriots-rise-cowboys-plummet");
            websites2017.Add("http://www.nfl.com/news/story/0ap3000000881069/article/nfl-power-rankings-week-12-steelers-vikings-climb-rams-slip");
            websites2017.Add("http://www.nfl.com/news/story/0ap3000000884640/article/nfl-power-rankings-week-13-chargers-hit-top-10-chiefs-drop");
            websites2017.Add("http://www.nfl.com/news/story/0ap3000000888526/article/nfl-power-rankings-week-14-new-england-patriots-take-no-1");
            websites2017.Add("http://www.nfl.com/news/story/0ap3000000892076/article/nfl-power-rankings-week-15-pittsburgh-steelers-hit-no-1-spot");
            websites2017.Add("http://www.nfl.com/news/story/0ap3000000895545/article/nfl-power-rankings-week-16-new-england-patriots-reign-again");
            websites2017.Add("http://www.nfl.com/news/story/0ap3000000898747/article/nfl-power-rankings-week-17-chiefs-back-in-top-10-eagles-fall");

            InternetConnector nflConnect = new InternetConnector(powerRankingSite);
            var nflHTML = nflConnect.GetHtml().DocumentNode.SelectNodes("//div[contains(@class,'pr-item')]");

            foreach (HtmlNode node in nflHTML)
            {
                var team = node.ChildNodes[1].ChildNodes;
                var rank = int.Parse(team[1].ChildNodes[3].InnerText.Replace("\n", "").Replace(" ", ""));
                var teamName = new Parsers.Team(team[5].ChildNodes[3].InnerText.Replace("\n", "").Replace(" ", ""));
                database.UpdatePowerRanking(week, teamName, rank);
            }
        }

        private static void pullPowerRankingsFromCBS(DataBase database, Week week, string website)
        {
            website = "https://www.cbssports.com/nfl/powerrankings/";

            InternetConnector conn = new InternetConnector(website);
            var html = conn.GetHtml();
            var htmlBody = html.DocumentNode.SelectNodes("//table//tbody//tr");

            foreach (HtmlNode node in htmlBody)
            {
                var team = node.ChildNodes;
                var rank = int.Parse(team[0].InnerText.Replace("\n", "").Replace(" ", ""));
                var teamName = new Parsers.Team(team[1].InnerText.Replace("\n", "").Replace(" ", ""));
                database.UpdatePowerRanking(week, teamName, rank);
            }
        }
    }
}
