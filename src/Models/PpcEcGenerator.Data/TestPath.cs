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
        public TestPath(List<int> path)
        {
            Path = path;
            PathLength = path.Count;
        }


        //---------------------------------------------------------------------
        //		Properties
        //---------------------------------------------------------------------
        public List<int> Path { get; private set; }
        public int PathLength { get; private set; }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        public bool HasPath(List<int> path)
        {
            foreach (int lineNumber in path)
            {
                if (!Path.Contains(lineNumber))
                    return false;
            }

            return true;
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
