
namespace FootballPicker.Criteria
{
    public class AverageScoreDifference : Criteria
    {
        private double value;
        protected static Normalizer normal = new Normalizer();
        public AverageScoreDifference(double scoreDifference)
        {
            value = scoreDifference;
            normal.New(value);
        }

        public override double GetNormalized()
        {
            return normal.GetNormalized(value);
        }
        public static void Reset()
        {
            normal = new Normalizer();
        }
    }
}
