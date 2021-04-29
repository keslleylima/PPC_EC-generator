namespace PpcEcGenerator.Data
{
    public class Coverage
    {
        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public Coverage(string testMethod, string coveredMethod, 
                        double ppcCoverage, double ecCoverage)
        {
            TestMethod = testMethod;
            CoveredMethod = coveredMethod;
            EdgeCoverage = ppcCoverage;
            PrimePathCoverage = ecCoverage;
        }


        //---------------------------------------------------------------------
        //		Properties
        //---------------------------------------------------------------------
        public string TestMethod { get; private set; }
        public string CoveredMethod { get; private set; }
        public double EdgeCoverage { get; private set; }
        public double PrimePathCoverage { get; private set; }
    }
}
