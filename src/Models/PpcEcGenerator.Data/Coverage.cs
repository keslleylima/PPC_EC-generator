using PpcEcGenerator.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PpcEcGenerator.Data
{
    public class Coverage
    {
        public Coverage(double ppcCoverage, double ecCoverage)
        {
            EdgeCoverage = ppcCoverage;
            PrimePathCoverage = ecCoverage;
        }

        public double EdgeCoverage { get; private set; }
        public double PrimePathCoverage { get; private set; }

        //public void Calculate(List<Test> listTestPath, List<Requirement> listReqPpc, List<Requirement> listReqEc)
        //{
        //    foreach (Test test in listTestPath)
        //    {
        //        EdgeCoverage += ((double)test.newReqEcCovered / (double)listReqEc.Count);
        //        PrimePathCoverage += ((double)test.newReqPpcCovered / (double)listReqPpc.Count);
        //    }
        //}
    }
}
