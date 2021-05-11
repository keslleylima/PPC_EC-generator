using System.Collections.Generic;

namespace PpcEcGenerator.Data
{
    /// <summary>
    ///     Manages metrics information for a test.
    /// </summary>
    public class TestPath
    {
        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public TestPath(string path)
        {
            Path = path;
            PathLength = path.Split(",").Length;
        }


        //---------------------------------------------------------------------
        //		Properties
        //---------------------------------------------------------------------
        public string Path { get; private set; }
        public int PathLength { get; private set; }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        public bool HasPath(string path)
        {
            return Path.Contains(path.Trim());
        }

        public override string ToString()
        {
            return $"TestPath ["
                + $"Path: {Path}; "
                + $"PathLength: {PathLength}; "
            + $"]";
        }
    }
}
