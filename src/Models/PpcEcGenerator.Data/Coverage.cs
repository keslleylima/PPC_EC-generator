namespace PpcEcGenerator.Data
{
    public class Coverage
    {
        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public Coverage(double ppcCoverage, double ecCoverage)
        {
            EdgeCoverage = ppcCoverage;
            PrimePathCoverage = ecCoverage;
        }


        //---------------------------------------------------------------------
        //		Properties
        //---------------------------------------------------------------------
        public double EdgeCoverage { get; private set; }
        public double PrimePathCoverage { get; private set; }
    }
}
