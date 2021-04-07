using System.Collections.Generic;

namespace PpcEcGenerator.Data
{
    /// <summary>
    ///     Manages metrics information for a test.
    /// </summary>
    public class Test
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private readonly List<string> requirements;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public Test(string path)
        {
            Path = path;
            OverallPpcCovered = 0;
            OverallEcCovered = 0;
            NewPpcCovered = 0;
            NewEcCovered = 0;
            requirements = new List<string>();
            PathLength = CalculatePathLength(path);
        }


        //---------------------------------------------------------------------
        //		Properties
        //---------------------------------------------------------------------
        public string Path { get; private set; }
        public int PathLength { get; private set; }
        public int NewPpcCovered { get; private set; }
        public int NewEcCovered { get; private set; }
        public int OverallPpcCovered { get; private set; }
        public int OverallEcCovered { get; private set; }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        private static int CalculatePathLength(string path)
        {
            return path.Split(',').Length;
        }

        public void AddRequirement(string requirement)
        {
            requirements.Add(requirement);
        }

        public void IncreaseNewPpcCovered()
        {
            NewPpcCovered++;
        }

        public void IncreaseNewEcCovered()
        {
            NewEcCovered++;
        }

        public void IncreasePpcCovered()
        {
            OverallPpcCovered++;
        }

        public void IncreaseEcCovered()
        {
            OverallEcCovered++;
        }

        public bool HasPath(string path)
        {
            return Path.Contains(path);
        }

        public override string ToString()
        {
            return $"Test ["
                + $"Path: {Path}; "
                + $"PathLength: {PathLength}; "
                + $"NewPpcCovered: {NewPpcCovered}; "
                + $"NewEcCovered: {NewEcCovered}; "
                + $"OverallPpcCovered: {OverallPpcCovered}; "
                + $"OverallEcCovered: {OverallEcCovered}; "
                + $"Requirements: {requirements}"
            + $"]";
        }
    }
}
