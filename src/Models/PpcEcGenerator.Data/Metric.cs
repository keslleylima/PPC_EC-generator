using System;
using System.Collections.Generic;
using System.IO;

namespace PpcEcGenerator.Data
{
    /// <summary>
    ///     Represents a code metric.
    /// </summary>
    public class Metric
    {
        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        private List<Requirement> requirements;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public Metric(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException("File metrics cannot be empty");

            requirements = new List<Requirement>();

            CreateRequirementsFrom(File.ReadAllLines(filePath));
        }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        private void CreateRequirementsFrom(string[] fileReq)
        {
            foreach (string req in fileReq)
            {
                if ((req.Length == 0) || !req.Contains("["))
                    continue;

                requirements.Add(new Requirement(GeneratePathFrom(req)));
            }
        }

        private List<int> GeneratePathFrom(string str)
        {
            List<int> path = new List<int>();

            foreach (string lineNumber in ExtractPathFrom(str))
            {
                path.Add(int.Parse(lineNumber));
            }

            return path;
        }

        private string[] ExtractPathFrom(string str)
        {
            // StartPoint is used to remove all char before the "["
            int startPoint = str.IndexOf("[");
            string path = str.Substring(startPoint);
            
            return path
                .Trim(new char[] { ' ', '[', ']', '\n' })
                .Replace(" ", "")
                .Split(",");
        }

        public double CalculateCoverage(List<TestPath> listTestPath)
        {
            ParseTestPaths(listTestPath);

            return CalculateCoverage();
        }

        private void ParseTestPaths(List<TestPath> listTestPath)
        {
            if (listTestPath == null)
                throw new ArgumentException("Test path list cannot be null");

            foreach (Requirement requirement in requirements)
            {
                bool covered = false;
                int i = 0;

                while (!covered && i < listTestPath.Count)
                {
                    if (listTestPath[i].HasPath(requirement.Path) && requirement.Feasible)
                    {
                        covered = true;
                        SetRequirementAsCovered(requirement, listTestPath[i]);
                    }

                    i++;
                }
            }
        }

        protected void SetRequirementAsCovered(Requirement requirement, TestPath test)
        {
            if (requirement.Covered == false)
            {
                requirement.Covered = true;
            }

            requirement.AddTestPath(test.Path);
        }

        private double CalculateCoverage()
        {
            int totalCovered = 0;

            foreach (Requirement requirement in requirements)
            {
                if (requirement.Covered)
                    totalCovered++;
            }

            return totalCovered / (double) requirements.Count;
        }

        public void ParseInfeasiblePath(List<List<int>> listInfeasiblePaths)
        {
            if (listInfeasiblePaths == null)
                throw new ArgumentException("Infeasible paths list cannot be null");

            foreach (List<int> infeasiblePath in listInfeasiblePaths)
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
