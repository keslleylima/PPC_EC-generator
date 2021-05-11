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
        private List<string> testPaths;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public Requirement(string path)
        {
            testPaths = new List<string>();
            Path = path;
            Covered = false;
            Feasible = true;
        }


        //---------------------------------------------------------------------
        //		Properties
        //---------------------------------------------------------------------
        public string Path { get; private set; }
        public bool Covered { get; set; }
        public bool Feasible { get; set; }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        public bool HasPath(string path)
        {
            return Path.Contains(path.Trim());
        }

        public void AddTestPath(string testPath)
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
