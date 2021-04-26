using System;
using System.Collections.Generic;
using System.IO;

namespace PpcEcGenerator.Data
{
    /// <summary>
    ///     Represents a code metric.
    /// </summary>
    public abstract class Metric
    {
        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        private List<Requirement> requirements;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        protected Metric(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException("File metrics cannot be empty");

            CreateRequirementsFrom(File.ReadAllLines(filePath));
        }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        private void CreateRequirementsFrom(string[] fileReq)
        {
            requirements = new List<Requirement>();

            foreach (string req in fileReq)
            {
                if (req.Length == 0)
                    continue;

                requirements.Add(new Requirement(ExtractPathFrom(req)));
            }
        }

        private string ExtractPathFrom(string str)
        {
            // StartPoint is used to remove all char before the "["
            int startPoint = str.IndexOf("[");
            string path = str.Substring(startPoint);
            
            return path.Trim(new char[] { ' ', '[', ']', '\n' });
        }

        public void CountReqCovered(List<Test> listTestPath)
        {
            if (listTestPath == null)
                throw new ArgumentException("Test path list cannot be null");

            foreach (Requirement requirement in requirements)
            {
                foreach (Test test in listTestPath)
                {
                    if (!test.HasPath(requirement.Path) || !requirement.Feasible)
                        continue;

                    ParseTestPath(requirement, test);
                }
            }
        }

        protected abstract void ParseTestPath(Requirement requirement, Test test);

        public void ParseInfeasiblePath(List<string> listInfeasiblePaths)
        {
            if (listInfeasiblePaths == null)
                throw new ArgumentException("Infeasible paths list cannot be null");

            foreach (string infeasiblePath in listInfeasiblePaths)
            {
                foreach (Requirement requirement in requirements)
                {
                    if (requirement.HasPath(infeasiblePath))
                    {
                        requirement.Feasible = false;
                    }
                }
            }
        }

        protected int GetTotalRequirements()
        {
            return requirements.Count;
        }

        public override string ToString()
        {
            return $"Metric [requirements: {requirements}]";
        }
    }
}
