using FootballPicker.InternetConnection;
using FootballPicker.Parsers;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FootballPicker.DataBaseManager
{
    public class GamesAndSpread
    {
        public static void Update(DataBase database, Week week, string site)
        {
            pullGameAndSpreadFromFootballLocks(database, week, site);
        }

        private static void pullGameAndSpreadFromFootballLocks(DataBase database, Week week, string site)
        {
            InternetConnector footballlocks = new InternetConnector(site);
            HtmlNodeCollection nflHTML = footballlocks.GetHtml().DocumentNode.SelectNodes("//table[contains(@id,'oddsTable')]");
            var games = nflHTML[0].ChildNodes[0];

            //New game
            try
            {
                var game = games.SelectNodes("//table[contains(@id,'matchupNameTablePreview')]")[0];
                var awayList = game.SelectNodes("//span[contains(@class,'teamAbbrPreview fright')]");
                var homeList = game.SelectNodes("//span[contains(@class,'teamAbbrPreview fleft')]");
                var spreadList = games.SelectNodes("//span[contains(@id,'lineNumber')]");
                var favoriteList = games.SelectNodes("//span[contains(@id,'lineTeam')]");
                for (int i = 0; i < awayList.Count; i++)
                {
                    database.AddWeek(week, new Team(homeList[i].InnerText), new Team(awayList[i].InnerText), new Team(favoriteList[i].InnerText), spreadList[i].InnerText);
                }
            }
            catch
            {
                var game = games.SelectNodes("//table[contains(@id,'matchupNameTable')]")[0];
                var gameList = game.SelectNodes("//span[contains(@class,'teamAbbrPreview')]");
                var awayScore = game.SelectNodes("//td[contains(@align,'left')]//span[contains(@class,'teamScore')]");
                var homeScore = game.SelectNodes("//td[contains(@align,'right')]//span[contains(@class,'teamScore')]");
                var spreadList = games.SelectNodes("//span[contains(@id,'lineNumber')]");
                var favoriteList = games.SelectNodes("//span[contains(@id,'lineTeam')]");
                for (int i = 0; i < gameList.Count/2; i++)
                {
                    try
                    {
                        database.AddWeek(week, new Team(gameList[i * 2+1].InnerText), new Team(gameList[i * 2].InnerText), new Team(favoriteList[i].InnerText), spreadList[i].InnerText, int.Parse(homeScore[i].InnerText), int.Parse(awayScore[i].InnerText));
                    }
                    catch
                    {
                        //yeah another one. We probably couldn't parse the string
                        Debugger.Break();
                    }
                }
            }


        }
        
    }
}
