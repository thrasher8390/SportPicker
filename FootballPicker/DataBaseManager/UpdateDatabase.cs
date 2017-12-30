using FootballPicker.InternetConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballPicker.DataBaseManager
{
    public class UpdateDatabase
    {
        public UpdateDatabase()
        {
            UpdatePowerRankings();
        }

        private void UpdatePowerRankings()
        {
            InternetConnector conn = new InternetConnector("https://www.cbssports.com/nfl/powerrankings/");
            var html = conn.GetHtml();
        }
    }
}
