using FootballPicker.DataBaseManager;
using FootballPicker.Parsers;
using FootballPicker.Predictions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace FootballPicker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        internal void RaisePropertyChanged(string prop)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (!string.IsNullOrEmpty(prop) && handler != null)
            {
                handler(this, new PropertyChangedEventArgs(prop));
            }
        }

        private WeekWinners thisWeekWinners;
        public WeekWinners ThisWeekWinners
        {
            get
            {
                return thisWeekWinners;
            }
            set
            {
                if(value != null)
                {
                    thisWeekWinners = value;
                    RaisePropertyChanged("ThisWeekWinners");
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void runPicker(int week)
        {
            DataBase database = new DataBase();
            UpdateDatabase updater = new UpdateDatabase(database);
            database.UpdateServer();

            Season season = new Season("2017");
            Week currentWeek = new Week(week, season);

            MachineLearner machineLearner = new MachineLearner();
            int totalScore = 0;

            for (int thisWeek = 1; thisWeek <= currentWeek.week; thisWeek++)
            {
                Console.WriteLine((int)thisWeek);
                List<Analyzers.Team> oldTeamAnalyzerList = new List<Analyzers.Team>();

                Parsers.Week tempWeek = new Parsers.Week((int)thisWeek, season);

                //Run the team analyzer for each team
                foreach (Parsers.Team name in Parsers.Team.GetAll())
                {
                    oldTeamAnalyzerList.Add(new Analyzers.Team(name, database.GetGames(name, tempWeek-2)));
                }

                Analyzers.Week lastWeek = new Analyzers.Week(oldTeamAnalyzerList, database.GetGames(tempWeek-1));

                machineLearner.Reset();
                while (machineLearner.HasMore())
                {
                    WeekWinners learningWinners = new WeekWinners(lastWeek, machineLearner.GetCriteriaRating());
                    //This is making a summation of how each ratio did over all of the weeks. We'll then use the best ratio to make "this weeks" rating
                    machineLearner.SetTotalScoreFromLastReading(learningWinners.GetLeagueScore());
                }

                List<Analyzers.Team> newTeamAnalyzerList = new List<Analyzers.Team>();

                //Run the team analyzer for each team
                foreach (Parsers.Team name in Parsers.Team.GetAll())
                {
                    newTeamAnalyzerList.Add(new Analyzers.Team(name, database.GetGames(name, tempWeek - 1)));
                }

                //Now lets make the ranking for this week!!!
                Analyzers.Week thisWeeks = new Analyzers.Week(newTeamAnalyzerList, database.GetGames(tempWeek));
                ThisWeekWinners = new WeekWinners(thisWeeks, machineLearner.GetBestRatio());

                int leageScore = ThisWeekWinners.GetLeagueScore();
                totalScore += leageScore;
                if (leageScore > 20)
                {
                    Console.WriteLine(leageScore+","+totalScore);
                }
                else
                {
                    int score = 16;
                    foreach(GameWinner winner in ThisWeekWinners)
                    {
                        Console.WriteLine(winner.Predicted.ToString() + "\t:" + winner.Confidence + "\t:" + score--);
                    }
                }
                Console.WriteLine();
            }
        }

        private void tbWeekToCalculate_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            //Need to add catching of non numbers
            try
            {
                runPicker(int.Parse(tbWeekToCalculate.Text));
            }
            catch { }
        }
    }
}
