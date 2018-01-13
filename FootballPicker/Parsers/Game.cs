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
        public Team AwayTeam;
        public Team HomeTeam;

        Team HomeTeamCopy;

        private bool IsValid = true;
        private int AwayScore;
        private int AwayPowerRanking;

        private int HomeScore;
        private int HomePowerRanking;

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

        internal List<object> GetRaw()
        {
            List<object> rawGame = new List<object>();

            foreach(DATA_BASE_INDEX index in Enum.GetValues(typeof(DATA_BASE_INDEX)))
            {
                switch(index)
                {
                    case DATA_BASE_INDEX.SEASON:
                        {
                            rawGame.Add(Season.ToString());
                            break;
                        }
                    case DATA_BASE_INDEX.WEEK:
                        {
                            rawGame.Add(Week.ToString());
                            break;
                        }
                    case DATA_BASE_INDEX.AWAY_TEAM:
                        {
                            rawGame.Add(AwayTeam.ToString());
                            break;
                        }
                    case DATA_BASE_INDEX.AWAY_TEAM_SCORE:
                        {
                            rawGame.Add(AwayScore.ToString());
                            break;
                        }
                    case DATA_BASE_INDEX.AWAY_TEAM_POWER_RANKING:
                        {
                            rawGame.Add(AwayPowerRanking.ToString());
                            break;
                        }
                    case DATA_BASE_INDEX.HOME_TEAM:
                        {
                            rawGame.Add(HomeTeam.ToString());
                            break;
                        }
                    case DATA_BASE_INDEX.HOME_TEAM_SCORE:
                        {
                            rawGame.Add(HomeScore.ToString());
                            break;
                        }
                    case DATA_BASE_INDEX.HOME_TEAM_POWER_RANKING:
                        {
                            rawGame.Add(HomePowerRanking.ToString());
                            break;
                        }
                    case DATA_BASE_INDEX.HOME_TEAM_COPY:
                        {
                            rawGame.Add(HomeTeamCopy.ToString());
                            break;
                        }
                    case DATA_BASE_INDEX.FAVORITE_TEAM:
                        {
                            rawGame.Add(Favorite.ToString());
                            break;
                        }
                    case DATA_BASE_INDEX.SPREAD:
                        {
                            rawGame.Add(Spread.ToString());
                            break;
                        }
                    case DATA_BASE_INDEX.MAX:
                    default:
                        {
                            break;
                        }

                }
            }

            return rawGame;
        }

        internal void UpdatePowerRanking(Team teamName, int rank)
        {
            if(AwayTeam.Equals(teamName))
            {
                AwayPowerRanking = rank;
            }
            else if(HomeTeam.Equals(teamName))
            {
                HomePowerRanking = rank;
            }
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
            if (AwayTeam.Equals(name))
            {
                powerRankingDifference = HomePowerRanking - AwayPowerRanking;
            }
            else
            {
                powerRankingDifference = AwayPowerRanking - HomePowerRanking;
            }

            return powerRankingDifference;
        }

        internal bool IsHome(Team name)
        {
            return HomeTeamCopy.Equals(name);
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
            if(AwayTeam.Equals(name))
            {
                score = AwayScore - HomeScore;
            }
            else 
            {
                score = HomeScore - AwayScore;
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
                if (AwayTeam.Equals(teamName) ||
                    HomeTeam.Equals(teamName))
                {
                    gameHasTeam = true;
                }
            }

            return gameHasTeam;
        }

        internal Team GetActualWinner()
        {
            Team actualWinner = new Team();

            if(AwayScore > HomeScore)
            {
                actualWinner = AwayTeam;
            }
            else if( HomeScore > AwayScore)
            {
                actualWinner = HomeTeam;
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
                    AwayScore = Convert.ToInt16(rawGame[(int)DATA_BASE_INDEX.AWAY_TEAM_SCORE]);
                    HomeScore = Convert.ToInt16(rawGame[(int)DATA_BASE_INDEX.HOME_TEAM_SCORE]);
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
                AwayTeam = new Team(rawGame[(int)DATA_BASE_INDEX.AWAY_TEAM]);                
                HomeTeam = new Team(rawGame[(int)DATA_BASE_INDEX.HOME_TEAM]);
                HomeTeamCopy = new Team(rawGame[(int)DATA_BASE_INDEX.HOME_TEAM_COPY]);
                Favorite = new Team(rawGame[(int)DATA_BASE_INDEX.FAVORITE_TEAM]);
                Spread = Convert.ToDouble(rawGame[(int)DATA_BASE_INDEX.SPREAD]);

                HomePowerRanking = convertDBtoPowerRanking(rawGame[(int)DATA_BASE_INDEX.HOME_TEAM_POWER_RANKING]);
                AwayPowerRanking = convertDBtoPowerRanking(rawGame[(int)DATA_BASE_INDEX.AWAY_TEAM_POWER_RANKING]);
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

        private int convertDBtoPowerRanking(string powerRankingString)
        {
            int powerRanking = 0;

            if(powerRankingString.Length > 0)
            {
                powerRanking = Convert.ToInt32(powerRankingString);
            }

            return powerRanking;
        }

        /// <summary>
        /// This is a private function that is meant to throw an exception if the team is not right
        /// </summary>
        /// <param name="name"></param>
        private void checkIfTeamIsInGame(Team name)
        {
            if (!AwayTeam.Equals(name) && !HomeTeam.Equals(name))
            {
                throw new NotSupportedException("Team {" + name + "} Did not play in this game. It was {" + AwayTeam + " vs " + HomeTeam + "}");
            }
        }
    }
}
