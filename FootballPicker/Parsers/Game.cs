using System;
using System.Windows;
using System.Collections.Generic;

namespace FootballPicker.Parsers
{
    public class Game
    {
        public bool IsFinished = true;
        public Season Season;
        public Week Week;
        public Team Team1;
        public Team Team2;

        Team Home;

        private bool IsValid = true;
        private int team1Score;
        private int team1PowerRanking;

        private int team2Score;
        private int team2PowerRanking;

        private Team Favorite;
        private double Spread;

        private enum DATA_BASE_INDEX : int
        {
            SEASON = 0,
            WEEK = 1,
            AWAY_TEAM,
            AWAY_TEAM_SCORE,
            AWAY_TEAM_POWER_RANKING,
            HOME_TEAM,
            HOME_TEAM_SCORE,
            HOME_TEAM_POWER_RANKING,
            HOME_TEAM_COPY,
            FAVORITE_TEAM,
            SPREAD,
            //This always needs to be at the bottom
            MAX
        }

        public Game(IList<Object> rawGame)
        {
            string[] stringArray = new string[(int)DATA_BASE_INDEX.MAX];
            
            for(int i = 0; i< stringArray.Length; i++)
            {
                if (i < rawGame.Count)
                {
                    stringArray[i] = rawGame[i].ToString();
                }
                else
                {
                    //This is UGLY Business but we need to set the array to ! null for rest of code to run
                    stringArray[i] = "";
                }
            }
            parseStringArayIntoGame(stringArray);
        }

        /// <summary>
        /// Will return a positive value if the team has a HIGHER ranking
        /// Note that a higher ranking is a lower number
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal double GetPowerRankingDifference(Team name)
        {
            checkIfTeamIsInGame(name);

            int powerRankingDifference;
            if (Team1.Equals(name))
            {
                powerRankingDifference = team2PowerRanking - team1PowerRanking;
            }
            else
            {
                powerRankingDifference = team1PowerRanking - team2PowerRanking;
            }

            return powerRankingDifference;
        }

        internal bool IsHome(Team name)
        {
            return Home.Equals(name);
        }

        /// <summary>
        /// This is the parser that takes rawGame as an input and creates an actual game out of it
        /// </summary>
        /// <param name="rawGame"></param>
        public Game(string[] rawGame)
        {
            parseStringArayIntoGame(rawGame);
        }

        /// <summary>
        /// Returns the spread in terms of the team requested
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal double GetSpread(Team name)
        {
            double spread;

            checkIfTeamIsInGame(name);

            if(Favorite.Equals(name))
            {
                spread = -Spread;
            }
            else
            {
                spread = Spread;
            }

            return spread;
        }

        /// <summary>
        /// return positive score if team won
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal int GetScoreDifference(Team name)
        {
            // We know the team was in the game if this passes
            checkIfTeamIsInGame(name);

            int score;
            if(Team1.Equals(name))
            {
                score = team1Score - team2Score;
            }
            else 
            {
                score = team2Score - team1Score;
            }

            return score;
        }

        /// <summary>
        /// Returns true if this game has the given team
        /// </summary>
        /// <param name="teamName"></param>
        /// <returns></returns>
        internal bool HasTeam(Team teamName)
        {
            Boolean gameHasTeam = false;
            if (IsValid)
            {
                if (Team1.Equals(teamName) ||
                    Team2.Equals(teamName))
                {
                    gameHasTeam = true;
                }
            }

            return gameHasTeam;
        }

        internal Team GetActualWinner()
        {
            Team actualWinner = new Team();

            if(team1Score > team2Score)
            {
                actualWinner = Team1;
            }
            else if( team2Score > team1Score)
            {
                actualWinner = Team2;
            }

            return actualWinner;
        }

        /// <summary>
        /// Lets create a real Game out of the data
        /// </summary>
        /// <param name="rawGame"></param>
        public void parseStringArayIntoGame(string[] rawGame)
        {
            //Try to pars the scores if we fail then the game isn't finished
            try
            {
                if (rawGame[(int)DATA_BASE_INDEX.AWAY_TEAM_SCORE].Length > 0 &&
                    rawGame[(int)DATA_BASE_INDEX.HOME_TEAM_SCORE].Length > 0)
                {
                    team1Score = Convert.ToInt16(rawGame[(int)DATA_BASE_INDEX.AWAY_TEAM_SCORE]);
                    team2Score = Convert.ToInt16(rawGame[(int)DATA_BASE_INDEX.HOME_TEAM_SCORE]);
                }
                else
                {
                    IsFinished = false;
                }
            }
            catch
            {
                IsFinished = false;
            }

            try
            {
                Season = new Season(rawGame[(int)DATA_BASE_INDEX.SEASON]);
                Week = new Week(rawGame[(int)DATA_BASE_INDEX.WEEK], Season);
                Team1 = new Team(rawGame[(int)DATA_BASE_INDEX.AWAY_TEAM]);
                Team2 = new Team(rawGame[(int)DATA_BASE_INDEX.HOME_TEAM]);
                Home = new Team(rawGame[(int)DATA_BASE_INDEX.HOME_TEAM_COPY]);
                Favorite = new Team(rawGame[(int)DATA_BASE_INDEX.FAVORITE_TEAM]);
                Spread = Convert.ToDouble(rawGame[(int)DATA_BASE_INDEX.SPREAD]);
            }
            catch (FormatException ex)
            {
                MessageBox.Show(ex.ToString());
                IsValid = false;
            }
            catch (MissingFieldException ex)
            {
                //Lets set the validaity of the game to INVALID
                IsValid = false;

            }
        }

        /// <summary>
        /// This is a private function that is meant to throw an exception if the team is not right
        /// </summary>
        /// <param name="name"></param>
        private void checkIfTeamIsInGame(Team name)
        {
            if (!Team1.Equals(name) && !Team2.Equals(name))
            {
                throw new NotSupportedException("Team {" + name + "} Did not play in this game. It was {" + Team1 + " vs " + Team2 + "}");
            }
        }
    }
}
