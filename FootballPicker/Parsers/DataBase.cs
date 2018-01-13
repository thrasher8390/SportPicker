using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FootballPicker.Parsers
{
    public class DataBase
    {
        List<Game> GameList = new List<Game>();

        /// <summary>
        /// Lets parse all of our databases so that we can start to do something with the information
        /// </summary>
        public DataBase()
        {

            GoogleSheetsConnector gsc = new GoogleSheetsConnector();
            IList < IList < Object >> dataBase = gsc.GetDataBase();
            foreach (IList<Object> rawGame in dataBase)
            {
                GameList.Add(new Game(rawGame));
            }
        }

        /// <summary>
        /// Convert the GameList to what we get from gsc.GetDataBase()
        /// </summary>
        internal void UpdateServer()
        {
            IList<IList<Object>> updatedDatabase = new List<IList<Object>>();
            foreach( Game game in GameList)
            {
                updatedDatabase.Add(game.GetRaw());
            }
            GoogleSheetsConnector gsc = new GoogleSheetsConnector();
            gsc.UpdateDataBase(updatedDatabase);
        }

        /// <summary>
        /// Get all games that include the given team
        /// </summary>
        /// <param name="teamName"></param>
        /// <returns></returns>
        internal List<Game> GetGames(Team teamName)
        {
            List<Game> returnList = new List<Game>();
            foreach(Game game in this.GameList)
            {
                if (game.HasTeam(teamName))
                {
                    returnList.Add(game);
                }
            }

            return returnList;
        }

        internal void UpdateRank(Week week, Team teamName, int rank)
        {
            Game game = GetGames(teamName).Find(x => x.Week.Equals(week));
            //Skip the teams that have a buy week
            if (game != null)
            {
                game.UpdatePowerRanking(teamName, rank);
            }
        }

        internal List<Game> GetGames(Team teamName, Week week)
        {
            //Run it for 10 seasons in the past
            return GetGames(teamName, week, 119*10);
        }
        /// <summary>
        /// Get all games that include the given team up to a given week. E.g. all games up to week 12 season 2017
        /// </summary>
        /// <param name="teamName"></param>
        /// <returns></returns>
        internal List<Game> GetGames(Team teamName, Week week, int numberOfWeeks)
        {
            List<Game> returnList = new List<Game>();

            List<Week> listOfWeeks = new List<Week>();
            //Lets fill in a list of all the weeks that are relavent
            for (int i = 0; i<numberOfWeeks;i++)
            {
                listOfWeeks.Add(week - i);
            }

            foreach ( Game game in GetGames(teamName))
            {
                if(listOfWeeks.Contains(game.Week))
                {
                    returnList.Add(game);
                }
            }

            return returnList;
        }

        /// <summary>
        /// Get all the games within a season/week
        /// </summary>
        /// <param name="season"></param>
        /// <param name="week"></param>
        /// <returns></returns>
        internal List<Game> GetGames(Week week)
        {
            List<Game> returnList = new List<Game>();
            foreach (Game game in this.GameList)
            {
                if (game.Week.Equals(week))
                {
                    returnList.Add(game);
                }
            }

            return returnList;
        }
    }
}
