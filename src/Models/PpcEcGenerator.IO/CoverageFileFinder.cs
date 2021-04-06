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

                if (IsEmpty(infPrefix))
                    throw new ArgumentException("Infeasible path prefix cannot be empty");
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
        public string PpcFile { get; private set; }
        public string EcFile { get; private set; }
        public List<string> TpFiles { get; private set; }
        public string InfFile { get; private set; }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        private void InitializeProperties()
        {
            PpcFile = string.Empty;
            EcFile = string.Empty;
            TpFiles = new List<string>();
            InfFile = string.Empty;
        }

        public void FindMetricsFilesAt(string rootPath)
        {
            InitializeProperties();

            foreach (string file in GetTextFilesFromDirectory(rootPath))
            {
                if (file.Contains(ppcPrefix))
                    PpcFile = file;
                else if (file.Contains(ecPrefix))
                    EcFile = file;
                else if (file.Contains(tpPrefix))
                    TpFiles.Add(file);
                else if (file.Contains(infPrefix))
                    InfFile = file;
            }
        }

        public static string[] GetTextFilesFromDirectory(string directoryPath)
        {
            return Directory.GetFiles(
                directoryPath, 
                "*.txt", 
                SearchOption.AllDirectories
            );
        }
    }
}
