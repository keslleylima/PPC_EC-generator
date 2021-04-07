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
        public List<string> testPaths;


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
            return Path.Contains(path);
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
