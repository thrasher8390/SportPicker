using System;
using FootballPicker.Criteria;

namespace FootballPicker.Analyzers
{
    public class Game
    {
        public Criteria.Team Team1;
        public Criteria.Team Team2;
        public Parsers.Game Info;
        /// <summary>
        /// Start analyzing all of the criteria
        /// </summary>
        /// <param name="team1"></param>
        /// <param name="team2"></param>
        /// <param name="game"></param>
        public Game(Team team1, Team team2, Parsers.Game game)
        {
            // Need to pass along the team name
            this.Team1 = new Criteria.Team(team1, game);
            this.Team2 = new Criteria.Team(team2, game);
            this.Info = game;
        }
    }
}
