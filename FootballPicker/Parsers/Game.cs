using System;
using System.Windows;
using System.Collections.Generic;
using System.Diagnostics;

namespace FootballPicker.Parsers
{
    public class Game
    {
        public bool IsFinished
        {
            get
            {
                return isFinished;
            }
        }

        public Week Week;
        public Team AwayTeam;
        public Team HomeTeam;

        Team HomeTeamCopy;

        private bool isFinished = true;
        private bool IsValid = true;
        private int AwayScore;
        private int AwayPowerRanking;

        private int HomeScore;
        private int HomePowerRanking;

        private Team Favorite;
        private double Spread;

        /// <summary>
        /// Update me if the DB ever changes!!!
        /// </summary>
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

        /// <summary>
        /// New Game
        /// </summary>
        /// <param name="away"></param>
        /// <param name="home"></param>
        /// <param name="favorite"></param>
        /// <param name="spread"></param>
        /// <param name="week"></param>
        public Game(Team away, Team home, Team favorite, double spread, Week week)
        {
            this.AwayTeam = away;
            this.HomeTeam = home;
            this.HomeTeamCopy = this.HomeTeam;
            this.Favorite = favorite;
            this.Spread = spread;
            this.Week = week;
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
        /// When parsing information from the internet we can use this to update the scores of a given game
        /// </summary>
        /// <param name="homeScore"></param>
        /// <param name="awayScore"></param>
        internal void UpdateScores(int homeScore, int awayScore)
        {
            this.HomeScore = homeScore;
            this.AwayScore = awayScore;

            //Assuming that there will never been a 0 to 0 tie
            if(this.HomeScore + this.AwayScore > 0)
            {
                isFinished = true;
            }
            else
            {
                //Why did we try to update a score if we don't have it
                Debugger.Break();
            }
        }

        /// <summary>
        /// This allows us to convert back to the DB
        /// </summary>
        /// <returns></returns>
        internal List<object> GetRaw()
        {
            List<object> rawGame = new List<object>();

            foreach(DATA_BASE_INDEX index in Enum.GetValues(typeof(DATA_BASE_INDEX)))
            {
                switch(index)
                {
                    case DATA_BASE_INDEX.SEASON:
                        {
                            rawGame.Add(Week.season.ToString());
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

        /// <summary>
        /// Used to update the power ranking of a given team in a given game
        /// </summary>
        /// <param name="teamName"></param>
        /// <param name="rank"></param>
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

        /// <summary>
        /// Returns if the true if the team was home
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
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

        /// <summary>
        /// For a game that is completed you can get the winner of a given game
        /// </summary>
        /// <returns></returns>
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

        #region Private Functions
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
                    isFinished = false;
                }
            }
            catch
            {
                isFinished = false;
            }

            try
            {
                Season season = new Season(rawGame[(int)DATA_BASE_INDEX.SEASON]);
                Week = new Week(rawGame[(int)DATA_BASE_INDEX.WEEK], season);
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
                MessageBox.Show(ex.ToString());
                //Lets set the validaity of the game to INVALID
                IsValid = false;

            }
        }

        #region Helper Private Functions
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

        #region Overrides
        public override bool Equals(object obj)
        {
            Game game = (Game)obj;
            return ((this.HomeTeam.Equals(game.HomeTeam)) &&
                (this.AwayTeam.Equals(game.AwayTeam)) &&
                (this.Week.Equals(game.Week)));
        }

        public override int GetHashCode()
        {
            return this.HomeTeam.GetHashCode() + this.AwayTeam.GetHashCode() + this.Week.GetHashCode();
        }
        #endregion
    }
    #endregion
    #endregion
}
