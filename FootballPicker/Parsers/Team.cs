using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballPicker.Parsers
{
    public class Team
    {
        //List of teams
        private enum Name
        {
            NONE,
            KANSAS_CITY_CHIEFS,
            NEW_ENGLAND_PATIROTS,
            TENNESEE_TITANS,
            SAN_FRANSISCO_49ERS,
            INDIONAPOLIS_COLTS,
            CAROLINA_PANTHERS,
            NY_JETS,
            PITTSBURG_STEELERS,
            JAGUARS,
            CLEVELEND_BROWNS,
            TAMPABAY_BUCCANEERS,
            MIAMA_DOLPHINS,
            BALTIMORE_RAVENS,
            GREENBAY_PACKERS,
            DETROIT_LIONS,
            CHICAGO_BEARS,
            LA_RAMS,
            MINNESOTA_VIKINGS,
            ARIZONA_CARDINALS,
            HOUSTON_TEXANS,
            NY_GIANTS,
            WASHINGTON_REDSKINS,
            NO_SAINTS,
            BUFFALO_BILLS,
            LA_CHARGERS,
            CIN_BENGALS,
            DENVER_BRONCOS,
            OAKLAND_RAIDERS,
            PHILADELPHIA_EAGLES,
            DALLAS_COWBOYS,
            ATLANTA_FALCONS,
            SEATLE_SEAHAWKS
        }

        /// <summary>
        /// This is weird but its a hack to keep Name private
        /// </summary>
        /// <returns></returns>
        internal static IEnumerable<Team> GetAll()
        {
            List<Team> list = new List<Team>();
            foreach (Name name in Enum.GetValues(typeof(Name)))
            {
                list.Add(new Team(name));
            }

            return list;
        }

        private Name TeamName;

        public Team()
        {
            TeamName = Name.NONE;
        }

        private Team(Name teamName)
        {
            this.TeamName = teamName;
        }

        public Team(string team)
        {
                TeamName = parseName(team);
        }

        private Name parseName(string team)
        {
            Name returnValue;
            team = team.ToLower();
            
            if(team.Length == 0)
            {
                throw new MissingFieldException("Team is empty");
            }
            if(team.Equals("-"))
            {
                returnValue = Name.NONE;
            }
            else if (team.Contains("chief") ||
                        team.Equals("kc"))
            {
                returnValue = Name.KANSAS_CITY_CHIEFS;
            }
            else if (team.Contains("england") ||
                        team.Equals("ne"))
            {
                returnValue = Name.NEW_ENGLAND_PATIROTS;
            }
            else if (team.Contains("panthers") ||
                team.Equals("car"))
            {
                returnValue = Name.CAROLINA_PANTHERS;
            }
            else if (team.Contains("colts") ||
                team.Equals("ind"))
            {
                returnValue = Name.INDIONAPOLIS_COLTS;
            }
            else if (team.Contains("jets") ||
                team.Equals("nyj"))
            {
                returnValue = Name.NY_JETS;
            }
            else if (team.Contains("cardinals") ||
            team.Equals("ari"))
            {
                returnValue = Name.ARIZONA_CARDINALS;
            }
            else if (team.Contains("falcons") ||
                        team.Equals("atl"))
            {
                returnValue = Name.ATLANTA_FALCONS;
            }
            else if (team.Contains("ravens") ||
                        team.Equals("bal"))
            {
                returnValue = Name.BALTIMORE_RAVENS;
            }
            else if (team.Contains("bills") ||
                        team.Equals("buf"))
            {
                returnValue = Name.BUFFALO_BILLS;
            }
            else if (team.Contains("bears") ||
                        team.Equals("chi"))
            {
                returnValue = Name.CHICAGO_BEARS;
            }
            else if (team.Contains("bengals") ||
                        team.Equals("cin"))
            {
                returnValue = Name.CIN_BENGALS;
            }
            else if (team.Contains("browns") ||
                        team.Equals("cle"))
            {
                returnValue = Name.CLEVELEND_BROWNS;
            }
            else if (team.Contains("cowboys") ||
                        team.Equals("dal"))
            {
                returnValue = Name.DALLAS_COWBOYS;
            }
            else if (team.Contains("broncos") ||
                        team.Equals("den"))
            {
                returnValue = Name.DENVER_BRONCOS;
            }
            else if (team.Contains("lions") ||
                        team.Equals("det"))
            {
                returnValue = Name.DETROIT_LIONS;
            }
            else if (team.Contains("packers") ||
                        team.Equals("gb"))
            {
                returnValue = Name.GREENBAY_PACKERS;
            }
            else if (team.Contains("texans") ||
                        team.Equals("hou"))
            {
                returnValue = Name.HOUSTON_TEXANS;
            }
            else if (team.Contains("jaguars") ||
                        team.Equals("jax"))
            {
                returnValue = Name.JAGUARS;
            }
            else if (team.Contains("chargers") ||
                        team.Equals("lac"))
            {
                returnValue = Name.LA_CHARGERS;
            }
            else if (team.Contains("rams") ||
                        team.Equals("lar"))
            {
                returnValue = Name.LA_RAMS;
            }
            else if (team.Contains("dolphins") ||
                        team.Equals("mia"))
            {
                returnValue = Name.MIAMA_DOLPHINS;
            }
            else if (team.Contains("vikings") ||
                        team.Equals("min"))
            {
                returnValue = Name.MINNESOTA_VIKINGS;
            }
            else if (team.Contains("saints") ||
                        team.Equals("no"))
            {
                returnValue = Name.NO_SAINTS;
            }
            else if (team.Contains("giants") ||
                        team.Equals("nyg"))
            {
                returnValue = Name.NY_GIANTS;
            }
            else if (team.Contains("raiders") ||
                        team.Equals("oak"))
            {
                returnValue = Name.OAKLAND_RAIDERS;
            }
            else if (team.Contains("eagles") ||
                        team.Equals("phi"))
            {
                returnValue = Name.PHILADELPHIA_EAGLES;
            }
            else if (team.Contains("steelers") ||
                        team.Equals("pit"))
            {
                returnValue = Name.PITTSBURG_STEELERS;
            }
            else if (team.Contains("seahawks") ||
                        team.Equals("sea"))
            {
                returnValue = Name.SEATLE_SEAHAWKS;
            }
            else if (team.Contains("buccaneers") ||
                        team.Equals("tb"))
            {
                returnValue = Name.TAMPABAY_BUCCANEERS;
            }
            else if (team.Contains("redskins") ||
                        team.Equals("was"))
            {
                returnValue = Name.WASHINGTON_REDSKINS;
            }
            else if (team.Contains("titans") ||
                        team.Equals("ten"))
            {
                returnValue = Name.TENNESEE_TITANS;
            }
            else if (team.Contains("49") ||
                       team.Equals("sf"))
            {
                returnValue = Name.SAN_FRANSISCO_49ERS;
            }
            else
            {
                throw new Exception("TeamName is not supported {" + team + "}");
            }

            return returnValue;
        }

        public bool Equals(Team teamName)
        {
            return teamName.TeamName.Equals(this.TeamName);
        }

        public override string ToString()
        {
            return this.TeamName.ToString();
        }
    }
}
