using PpcEcGenerator.IO;
using PpcEcGenerator.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PpcEcGenerator.Parse
{
    /// <summary>
    ///     Responsible for parsing metrics files.
    /// </summary>
    public class MetricsParser
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        /// <summary>
        ///     Key:    idTestPathFile
        ///     Value:  Coverage data
        /// </summary>
        private readonly IDictionary<string, List<Coverage>> coverageData;

        private readonly string projectPath;
        private List<Test> listTestPath;
        private List<string> listInfeasiblePaths;
        private Coverage coverage;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public MetricsParser(string projectPath)
        {
            if (string.IsNullOrEmpty(projectPath))
                throw new ArgumentException("Project path cannot be empty");

            this.projectPath = projectPath;
            coverageData = new Dictionary<string, List<Coverage>>();
        }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        public IDictionary<string, List<Coverage>> ParseMetrics(CoverageFileFinder finder)
        {
            if (finder == null)
                throw new ArgumentException("Coverage file finder cannot be null");

            listInfeasiblePaths = new List<string>();

            foreach (string methodPath in Directory.GetDirectories(projectPath))
            {
                listTestPath = new List<Test>();

                finder.FindMetricsFilesAt(methodPath);

                if (HasMissingMetrics(finder))
                    continue;

                ParseMetricsFiles(finder);
            }

            return coverageData;
        }

        private void ParseMetricsFiles(CoverageFileFinder finder)
        {
            foreach (string testPathFile in finder.TestPathFiles)
            {
                string[] testPathLines = File.ReadAllLines(testPathFile);
                PPC ppc = new PPC(finder.PrimePathCoverageFile);
                EC ec = new EC(finder.EdgeCoverageFile);

                ParseInfeasiblePaths(finder.InfeasiblePathFile, ppc, ec);
                ParseTestPathLines(testPathLines);
                SortListByPathLength(listTestPath);
                CalculateCoverage(ppc, ec);
                StoreCoverage(testPathLines.First());
            }
        }

        private void ParseTestPathLines(string[] fileTestPath)
        {
            foreach (string line in fileTestPath.Distinct().ToArray().Skip(1))
            {
                listTestPath.Add(new Test(ExtractCodePathFrom(line)));
            }
        }

        private string ExtractCodePathFrom(string line)
        {
            return line.Trim(new Char[] { ' ', '[', ']', '\n' });
        }

        private void CalculateCoverage(PPC ppc, EC ec)
        {
            ppc.CountReqCovered(listTestPath);
            ec.CountReqCovered(listTestPath);

            coverage = new Coverage(
                ppc.CalculateCoverage(listTestPath),
                ec.CalculateCoverage(listTestPath)
            );
        }

        private void StoreCoverage(string methodId)
        {
            if (coverageData.ContainsKey(methodId))
            {
                coverageData.TryGetValue(methodId, out List<Coverage> coverageList);

                coverageList.Add(coverage);
            }
            else
            {
                List<Coverage> coverageList = new List<Coverage>();

                coverageList.Add(coverage);

                coverageData.Add(methodId, coverageList);
            }
        }

        private static bool HasMissingMetrics(CoverageFileFinder finder)
        {
            return  (finder.PrimePathCoverageFile == string.Empty)
                    || (finder.EdgeCoverageFile == string.Empty)
                    || (finder.TestPathFiles.Count == 0);
        }

        private void ParseInfeasiblePaths(string infPathFile, PPC ppc, EC ec)
        {
            if (string.IsNullOrEmpty(infPathFile))
                return;

            foreach (string line in File.ReadAllLines(infPathFile))
            {
                listInfeasiblePaths.Add(ExtractCodePathFrom(line));
            }

            ppc.ParseInfeasiblePath(listInfeasiblePaths);
            ec.ParseInfeasiblePath(listInfeasiblePaths);
        }

        private static void SortListByPathLength(List<Test> listTestPath)
        {
            for (int i = 1; i < listTestPath.Count; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (listTestPath[i].PathLength > listTestPath[j].PathLength)
                    {
                        Test test = listTestPath[i];
                        listTestPath[i] = listTestPath[j];
                        listTestPath[j] = test;
                    }
                }
            }
        }
    }
}
