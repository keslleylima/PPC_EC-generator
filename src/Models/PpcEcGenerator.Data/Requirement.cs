using System;
using System.Collections.Generic;
using System.Text;

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
        public string path { set; get; }
        public List<string> testPaths;
        public bool covered;
        public bool feasible;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public Requirement(string path)
        {
            this.path = path;
            this.covered = false;
            this.feasible = true;
            this.testPaths = new List<string>();
        }
    }
}
