using System.Collections.Generic;

namespace PpcEcGenerator.Data
{
    /// <summary>
    ///     Represents a test requirement.
    /// </summary>
    public class Requirement
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private List<List<int>> testPaths;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public Requirement(List<int> path)
        {
            testPaths = new List<List<int>>();
            Path = path;
            Covered = false;
            Feasible = true;
        }


        //---------------------------------------------------------------------
        //		Properties
        //---------------------------------------------------------------------
        public List<int> Path { get; private set; }
        public bool Covered { get; set; }
        public bool Feasible { get; set; }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        public bool HasPath(List<int> path)
        {
            foreach (int lineNumber in path)
            {
                if (!path.Contains(lineNumber))
                    return false;
            }

            return true;
        }

        public void AddTestPath(List<int> testPath)
        {
            testPaths.Add(testPath);
        }

        public override string ToString()
        {
            return $"Requirement ["
                + $"Path: {Path}; "
                + $"Covered: {Covered}; "
                + $"Feasible: {Feasible}; "
                + $"testPaths: {testPaths}"
            + $"]";
        }
    }
}
