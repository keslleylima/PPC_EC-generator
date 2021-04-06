using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PpcEcGenerator.Data
{
    public class EC : Metric
    {
        public EC(string filePath) : base(filePath)
        {
        }

        protected override void ParseTestPath(Requirement requirement, Test test)
        {
            if (requirement.covered == false)
            {
                test.newReqEcCovered = test.newReqEcCovered + 1;
                requirement.covered = true;
            }

            test.overallReqEcCovered = test.overallReqEcCovered + 1;
            test.requirements.Add(requirement.path);
            requirement.testPaths.Add(test.path);
        }

        public double CalculateCoverage(List<Test> listTestPath)
        {
            double coverage = 0.0;

            foreach (Test test in listTestPath)
            {
                coverage += ((double)test.newReqPpcCovered) / GetTotalRequirements();
            }

            return coverage;
        }
    }
}
