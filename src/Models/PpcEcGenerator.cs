using PccEcGenerator.Data;
using PpcEcGenerator.Export;
using PpcEcGenerator.IO;
using PpcEcGenerator.Parse;
using System;
using System.Collections.Generic;
using System.IO;

namespace PpcEcGenerator
{
    /// <summary>
    ///     Generates code coverage using the following metrics:
    ///     
    ///     <list type="bullet">
    ///         <item>Prime path coverage</item>
    ///         <item>Edge coverage</item>
    ///     </list>
    /// </summary>
    public class PpcEcGenerator
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private readonly string projectPath;
        private readonly string outputPath;
        private readonly CoverageFileFinder finder;
        private IDictionary<string, List<Coverage>> coverageData;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        private PpcEcGenerator(string projectPath, string outputPath, CoverageFileFinder finder)
        {
            this.projectPath = projectPath;
            this.outputPath = outputPath;
            this.finder = finder;
        }


        //---------------------------------------------------------------------
        //		Builder
        //---------------------------------------------------------------------
        public class Builder
        {
            private string projectPath;
            private string outputPath;
            private string ppcPrefix;
            private string ecPrefix;
            private string tpPrefix;
            private string infPrefix;

            public Builder()
            {
            }

            public Builder ProjectPath(string path)
            {
                projectPath = path;

                return this;
            }

            public Builder OutputPath(string path)
            {
                outputPath = path;

                return this;
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

            public PpcEcGenerator Build()
            {
                CheckRequiredFields();

                CoverageFileFinder finder = new CoverageFileFinder.Builder()
                    .PrimePathCoveragePrefix(ppcPrefix)
                    .EdgeCoveragePrefix(ecPrefix)
                    .InfeasiblePathPrefix(infPrefix)
                    .Build();


                return new PpcEcGenerator(projectPath, outputPath, finder);
            }

            private void CheckRequiredFields()
            {
                if (IsEmpty(projectPath))
                    throw new ArgumentException("Project path cannot be empty");

                if (IsEmpty(outputPath))
                    throw new ArgumentException("Output path cannot be empty");

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
        //		Methods
        //---------------------------------------------------------------------
        public string GenerateCoverage()
        {
            if (File.Exists(outputPath))
                File.Delete(outputPath);

            DoParsing();
            ExportResults();
            
            return outputPath;
        }

        private void DoParsing()
        {
            MetricsParser parser = new MetricsParser(projectPath);

            coverageData = parser.ParseMetrics(finder);
        }

        private void ExportResults()
        {
            CoverageCSVExporter exporter = new CoverageCSVExporter(outputPath, coverageData);
            
            exporter.Export();
        }
    }
}
