using FootballPicker.Criteria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballPicker
{
    public class MachineLearner
    {
        private int request = 0;
        private bool hasMore = true;

        List<List<double>> learningList = new List<List<double>>();
        List<double> learningScore = new List<double>();

        public MachineLearner()
        {
            buildList(7, null);
        }

        private void buildList(int numLists, List<double> newList)
        {
            double currentRating = 0;
            if(newList == null)
            {
                newList = new List<double>();
            }
            newList.Add(currentRating);
            numLists--;
            while ((currentRating - 1) < .0001)
            {
                if (checkAndAdd(numLists, newList))
                {
                    return;
                }
                if (numLists > 0)
                {
                    buildList(numLists, new List<double>(newList));
                }

                currentRating += .05;
                newList[newList.Count - 1] = currentRating;
            }
        }

        private Boolean checkAndAdd(int numLists, List<double> newList)
        {
            Boolean returnValue = false;
            double total = 0;
            foreach (double rate in newList)
            {
                total += rate;
            }

            if (Math.Abs(total-1) < .0001)
            {
                List<double> tempList = new List<double>(newList);
                //We need to fill in the other criteria ratings that we didn't get to
                for (int i = 0; i < numLists; i++)
                {
                    tempList.Add(0);
                }
                learningList.Add(tempList);
                //Initialize the learning score so that this can be our summation of how the particular ratio did
                learningScore.Add(0);
                returnValue = true;
            }

            return returnValue;
        }

        public List<double> GetCriteriaRating()
        {
            if(request == learningList.Count-1)
            {
                hasMore = false;
            }

            return learningList[request++];
        }

        internal void Reset()
        {
            request = 0;
            hasMore = true;
        }

        /// <summary>
        /// We expect that this is called after GetCriteriaRating hence the -1
        /// </summary>
        /// <param name="totalScore"></param>
        public void SetTotalScoreFromLastReading(int totalScore)
        {
            if(request > 0)
            {
                learningScore[request - 1] += totalScore;
            }
        }

        internal bool HasMore()
        {
            return hasMore;
        }

        internal List<double> GetBestRatio()
        {
            double maxValue = learningScore.Max();
            int maxValueIndex = learningScore.IndexOf(maxValue);
            Console.WriteLine("Index Ratio {" + maxValueIndex +"} Max Value with ratio {" + maxValue +"}");
            foreach (double doub in learningList[maxValueIndex])
            {
                Console.Write(doub.ToString("0.00") + ",\t");
            }
            Console.WriteLine();
            return learningList[maxValueIndex];
        }
    }
}
