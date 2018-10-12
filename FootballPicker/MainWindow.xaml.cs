using FootballPicker.DataBaseManager;
using FootballPicker.Parsers;
using FootballPicker.Predictions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

        /// <summary>
        /// Run picker runs though all past picks and determines what the best picks for this week are.
        /// </summary>
        /// <param name="week">This is the week of which you would like the predictions for :)</param>
        private void runPicker(Week currentWeek)
        {
            DataBase database = new DataBase();

            MachineLearner machineLearner = new MachineLearner();
            int totalScore = 0;

            for (Week thisWeek = new Week(8,new Season(2017)); thisWeek <= currentWeek; thisWeek++)
            {
                Console.WriteLine(thisWeek.ToString());

                Analyzers.Week lastWeek = analyzeWeek(database, thisWeek.PreviousRegularSeasonWeek);

                machineLearner.Reset();
                while (machineLearner.HasMore())
                {
                    WeekWinners learningWinners = new WeekWinners(lastWeek, machineLearner.GetCriteriaRating());
                    //This is making a summation of how each ratio did over all of the weeks. We'll then use the best ratio to make "this weeks" rating
                    machineLearner.SetTotalScoreFromLastReading(learningWinners.GetLeagueScore());
                }

                //Now lets make the ranking for this week!!!
                Analyzers.Week thisWeeks = analyzeWeek(database, thisWeek);
                ThisWeekWinners = new WeekWinners(thisWeeks, machineLearner.GetBestRatio());

                int leageScore = ThisWeekWinners.GetLeagueScore();
                totalScore += leageScore;
                Console.WriteLine(leageScore+","+totalScore);

                int score = 16;
                foreach(GameWinner winner in ThisWeekWinners)
                {
                    Console.WriteLine(winner.Predicted.ToString() + "\t:" + winner.Confidence + "\t:" + score--);
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Analyze a given week of football
        /// </summary>
        /// <param name="database"></param>
        /// <param name="week"></param>
        /// <returns></returns>
        private Analyzers.Week analyzeWeek(DataBase database, Week week)
        {
            List<Analyzers.Team> oldTeamAnalyzerList = new List<Analyzers.Team>();

            //Run the team analyzer for each team

            Week previousRegularSeasonWeek = week.PreviousRegularSeasonWeek;
            foreach (Team name in Parsers.Team.GetAll())
            {
                //Lets analyze each team for all of the previous weeks
                oldTeamAnalyzerList.Add(new Analyzers.Team(name, database.GetGames(name, previousRegularSeasonWeek)));
            }

            //Now given how we analyzed all of the past teams for each weeks. Lets figure out how we would have chosen
            Analyzers.Week weekAnalyzer = new Analyzers.Week(oldTeamAnalyzerList, database.GetGames(week, true));

            return weekAnalyzer;
        }

        private void UpdateDatabase(Week week)
        {
            DataBase database = new DataBase();
            UpdateDatabase updater = new UpdateDatabase(database, week);
            database.UpdateServer(week);
        }

        #region Event Handlers
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UpdateDatabase(getGUIWeek());
            }
            catch
            {
                Debugger.Break();
            }
        }

        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            //Need to add catching of non numbers
            try
            {
                runPicker(getGUIWeek());
            }
            catch
            {
                // Lets add better handling for this
                Debugger.Break();
            }
        }
        #endregion

        #region UI Helpers
        private Week getGUIWeek()
        {
            return new Week(int.Parse(tbWeek.Text), new Season(int.Parse(tbSeason.Text)));
        }
        #endregion
    }
}
