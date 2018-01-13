using System;
using System.Collections.Generic;
using FootballPicker.Analyzers;

namespace FootballPicker.Predictions
{
    public class GameWinner
    {
        public double Confidence;
        public Parsers.Team Predicted;
        public Parsers.Team Actual;
        private List<double> criteriaRating;
    
        private void getWinner(Game game)
        {
            this.Actual = game.Info.GetActualWinner();

            double team1Confidence = CalculateConfidence(game.Team1);
            double team2Confidence = CalculateConfidence(game.Team2);

            if (team1Confidence > team2Confidence)
            {
                Confidence = team1Confidence - team2Confidence;
                Predicted = game.Team1.TeamInfo.Name;
            }
            else
            {
                Confidence = team2Confidence - team1Confidence;
                Predicted = game.Team2.TeamInfo.Name;
            }
        }

        public GameWinner(Game game, List<double> criteriaRating)
        {
            double totalRatings = 0;

            //Add up each criteria to ensure it is == 1
            foreach(double rating in criteriaRating)
            {
                totalRatings += rating;
            }

            if (totalRatings < 0.99)
            {
                throw new Exception("totalRating != 1. {" + totalRatings + "}");
            }

            this.criteriaRating = criteriaRating;

            getWinner(game);
        }

        internal bool IsPredictionCorrect()
        {
            return (Predicted.Equals(Actual));
        }


        /// <summary>
        /// use all of the criteria to calculate a total confidence
        /// </summary>
        /// <param name="team1"></param>
        /// <returns></returns>
        private double CalculateConfidence(Criteria.Team team)
        {
            double confidence = 0;

            if(criteriaRating.Count != team.CriteriaList.Count)
            {
                throw new Exception("the Criteria Rating in Machine Learner does not match the size of the Criteria list in Criteria.Team");
            }

            //Figure out Team
            for(int i = 0; i< team.CriteriaList.Count; i++)
            {
                confidence += team.CriteriaList[i].GetNormalized() * criteriaRating[i];
            }

            return confidence;
        }
    }
}
