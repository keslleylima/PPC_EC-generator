using System;
using System.Collections.Generic;
using System.IO;

namespace PpcEcGenerator.IO
{
    /// <summary>
    ///     Responsible for finding metrics files using prefixes.
    /// </summary>
    public class CoverageFileFinder
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private readonly string ppcPrefix;
        private readonly string ecPrefix;
        private readonly string tpPrefix;
        private readonly string infPrefix;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        private CoverageFileFinder(string ppcPrefix, string ecPrefix, 
                                   string tpPrefix, string infPrefix)
        {
            this.ppcPrefix = ppcPrefix;
            this.ecPrefix = ecPrefix;
            this.tpPrefix = tpPrefix;
            this.infPrefix = infPrefix;

            InitializeProperties();
        }


        //---------------------------------------------------------------------
        //		Builder
        //---------------------------------------------------------------------
        public class Builder
        {
            private string ppcPrefix;
            private string ecPrefix;
            private string tpPrefix;
            private string infPrefix;

            public Builder()
            {
                infPrefix = string.Empty;
            }

            public Builder PrimePathCoveragePrefix(string prefix)
            {
                ppcPrefix = prefix;

                return this;
            }

            public Builder EdgeCoveragePrefix(string prefix)
            {
                ecPrefix = prefix;

                return this;
            }

            public Builder TestPathPrefix(string prefix)
            {
                tpPrefix = prefix;

                return this;
            }

            public Builder InfeasiblePathPrefix(string prefix)
            {
                infPrefix = prefix;

                return this;
            }

            public CoverageFileFinder Build()
            {
                CheckRequiredFields();

                return new CoverageFileFinder(ppcPrefix, ecPrefix, tpPrefix, infPrefix);
            }

            private void CheckRequiredFields()
            {
                if (IsEmpty(ppcPrefix))
                    throw new ArgumentException("Prime path coverage prefix cannot be empty");

                if (IsEmpty(ecPrefix))
                    throw new ArgumentException("Edge coverage prefix cannot be empty");

                if (IsEmpty(tpPrefix))
                    throw new ArgumentException("Test path prefix cannot be empty");
            }

            private bool IsEmpty(string str)
            {
                return  (str == null)
                        || (str.Length == 0);
            }
        }


        //---------------------------------------------------------------------
        //		Properties
        //---------------------------------------------------------------------
        public string PrimePathCoverageFile { get; private set; }
        public string EdgeCoverageFile { get; private set; }
        public List<string> TestPathFiles { get; private set; }
        public string InfeasiblePathFile { get; private set; }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        private void InitializeProperties()
        {
            PrimePathCoverageFile = string.Empty;
            EdgeCoverageFile = string.Empty;
            TestPathFiles = new List<string>();
            InfeasiblePathFile = string.Empty;
        }

        public void FindMetricsFilesAt(string rootPath)
        {
            InitializeProperties();

            foreach (string file in GetTextFilesFromDirectory(rootPath))
            {
                if (file.Contains(ppcPrefix))
                    PrimePathCoverageFile = file;
                else if (file.Contains(ecPrefix))
                    EdgeCoverageFile = file;
                else if (file.Contains(tpPrefix))
                    TestPathFiles.Add(file);
                else if (file.Contains(infPrefix))
                    InfeasiblePathFile = file;
            }
        }

        private static string[] GetTextFilesFromDirectory(string directoryPath)
        {
            return Directory.GetFiles(
                directoryPath, 
                "*.txt", 
                SearchOption.AllDirectories
            );
        }
    }
}
