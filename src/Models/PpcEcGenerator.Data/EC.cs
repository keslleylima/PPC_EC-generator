using System.Collections.Generic;

namespace PpcEcGenerator.Data
{
    /// <summary>
    ///     Represents edge coverage metric.
    /// </summary>
    public class EC : Metric
    {
        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public EC(string filePath) : base(filePath)
        {
        }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        protected override void ParseTestPath(Requirement requirement, Test test)
        {
            if (requirement.Covered == false)
            {
                test.IncreaseNewEcCovered();
                requirement.Covered = true;
            }

            test.IncreaseEcCovered();
            test.AddRequirement(requirement.Path);
            requirement.testPaths.Add(test.Path);
        }

        public double CalculateCoverage(List<Test> listTestPath)
        {
            double coverage = 0.0;

            foreach (Test test in listTestPath)
            {
                coverage += ((double) test.NewEcCovered) / GetTotalRequirements();
            }

            return coverage;
        }
    }
}
