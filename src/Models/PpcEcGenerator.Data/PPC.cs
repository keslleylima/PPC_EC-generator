using System.Collections.Generic;

namespace PpcEcGenerator.Data
{
    /// <summary>
    ///     Represents prime path coverage metric.
    /// </summary>
    public class PPC : Metric
    {
        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public PPC(string filePath) : base(filePath)
        {
        }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        protected override void ParseTestPath(Requirement requirement, Test test)
        {
            if (requirement.covered == false)
            {
                test.IncreaseNewPpcCovered();
                requirement.covered = true;
            }

            test.IncreasePpcCovered();
            test.AddRequirement(requirement.path);
            requirement.testPaths.Add(test.Path);
        }

        public double CalculateCoverage(List<Test> listTestPath)
        {
            double coverage = 0.0;

            foreach (Test test in listTestPath)
            {
                coverage += ((double) test.NewPpcCovered) / GetTotalRequirements();
            }

            return coverage;
        }
    }
}
