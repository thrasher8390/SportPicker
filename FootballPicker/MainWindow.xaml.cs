using FootballPicker.DataBaseManager;
using FootballPicker.Parsers;
using FootballPicker.Predictions;
using System;
using System.Collections.Generic;
using System.Windows;

namespace FootballPicker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            UpdateDatabase updater = new UpdateDatabase();

            DataBase database = new DataBase();
            MachineLearner machineLearner = new MachineLearner();
            int totalScore = 0;

            for (int tempWeek = 1; tempWeek <= 17; tempWeek++)
            {
                Console.WriteLine(tempWeek);
                List<Analyzers.Team> teamAnalyzerList = new List<Analyzers.Team>();
            
                Parsers.Season season = new Parsers.Season("2017");
                Parsers.Week week = new Parsers.Week(tempWeek, season);

                //Run the team analyzer for each team
                foreach (Parsers.Team name in Parsers.Team.GetAll())
                {
                    teamAnalyzerList.Add(new Analyzers.Team(name, database.GetGames(name, week-2, 16)));
                }

                Analyzers.Week lastWeek = new Analyzers.Week(teamAnalyzerList, database.GetGames(week-1));

                machineLearner.Reset();
                while (machineLearner.HasMore())
                {
                    WeekWinners learningWinners = new WeekWinners(lastWeek, machineLearner.GetCriteriaRating());
                    //This is making a summation of how each ratio did over all of the weeks. We'll then use the best ratio to make "this weeks" rating
                    machineLearner.SetTotalScoreFromLastReading(learningWinners.GetLeagueScore());
                }

                //Now lets make the ranking for this week!!!
                Analyzers.Week thisWeek = new Analyzers.Week(teamAnalyzerList, database.GetGames(week));
                WeekWinners thisWeekWinners = new WeekWinners(thisWeek, machineLearner.GetBestRatio());

                int leageScore = thisWeekWinners.GetLeagueScore();
                totalScore += leageScore;
                if (leageScore > 20)
                {
                    Console.WriteLine(leageScore+","+totalScore);
                }
                else
                {
                    int score = 16;
                    foreach(GameWinner winner in thisWeekWinners.GameWinners)
                    {
                        Console.WriteLine(winner.Predicted.ToString() + "\t:" + score--);
                    }
                }
                Console.WriteLine();
            }
            InitializeComponent();
        }
    }
}
