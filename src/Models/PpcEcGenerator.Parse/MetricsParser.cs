using PpcEcGenerator.IO;
using PpcEcGenerator.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PpcEcGenerator.Parse
{
    public class MetricsParser
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        /// <summary>
        ///     Key:    idTestPathFile
        ///     Value:  Coverage data
        /// </summary>
        Dictionary<string, List<Coverage>> coverageData;

        List<Test> listTestPath = new List<Test>();
        List<string> listInfeasiblePaths;
        string projectPath;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public MetricsParser(string projectPath)
        {
            this.projectPath = projectPath;
        }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        public Dictionary<string, List<Coverage>> ParseMetrics(CoverageFileFinder finder)
        {
            listInfeasiblePaths = new List<string>();

            foreach (string methodPath in Directory.GetDirectories(projectPath))
            {
                finder.FindMetricsFilesAt(methodPath);

                if (HasMissingMetrics(finder))
                    continue;

                ParseTestPathFiles(finder);
            }

            return coverageData;
            
        }

        private void ParseTestPathFiles(CoverageFileFinder finder)
        {
            foreach (string testPathFile in finder.TestPathFiles)
            {
                string[] testPathLines = File.ReadAllLines(testPathFile);

                PPC ppc = new PPC(finder.PrimePathCoverageFile);
                EC ec = new EC(finder.EdgeCoverageFile);

                ParseInfeasiblePaths(finder.InfeasiblePathFile, listInfeasiblePaths, ppc, ec);
                ParseTestPathLines(testPathLines);
                CalculateTestPathLength();
                CalculateCoverage(ppc, ec);
                StoreCoverage(ppc, ec, testPathLines.First());
            }
        }

        private void CalculateTestPathLength()
        {
            foreach (Test testPath in listTestPath)
            {
                testPath.pathLength = CalculatePathLength(testPath);
            }

            SortListByPathLength(listTestPath);
        }

        private void CalculateCoverage(PPC ppc, EC ec)
        {
            ppc.CountReqCovered(listTestPath);
            ec.CountReqCovered(listTestPath);
        }

        private void StoreCoverage(PPC ppc, EC ec, string methodId)
        {
            Coverage coverage = new Coverage(
                                ppc.CalculateCoverage(listTestPath),
                                ec.CalculateCoverage(listTestPath)
                            );

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

        private bool HasMissingMetrics(CoverageFileFinder finder)
        {
            return  (finder.PrimePathCoverageFile == string.Empty)
                    || (finder.EdgeCoverageFile == string.Empty);
        }

        private void ParseInfeasiblePaths(string infPathFile, List<string> listInfeasiblePaths, PPC ppc, EC ec)
        {
            if (string.IsNullOrEmpty(infPathFile))
                return;

            string[] fileInfeasiblePaths = File.ReadAllLines(infPathFile);

            CreateListInfeasiblePaths(fileInfeasiblePaths, listInfeasiblePaths);
            ppc.ParseInfeasiblePath(listInfeasiblePaths);
            ec.ParseInfeasiblePath(listInfeasiblePaths);
        }

        public static int CalculatePathLength(Test testPath)
        {
            return testPath.path.Split(',').Length;
        }

        

        public void ParseTestPathLines(string[] fileTestPath)
        {
            // removing repeted test paths.
            String[] fileTestPathProcessed = fileTestPath.Distinct().ToArray();
            foreach (string item in fileTestPathProcessed.Skip(1))
            {
                string tmp = item.Trim(new Char[] { ' ', '[', ']', '\n' });
                Test test = new Test(tmp);

                listTestPath.Add(test);
            }
        }

        public static void CreateListInfeasiblePaths(string[] fileInfeasiblePaths, List<string> listInfeasiblePaths)
        {
            foreach (string item in fileInfeasiblePaths)
            {
                string tmp = item.Trim(new Char[] { ' ', '[', ']', '\n' });
                listInfeasiblePaths.Add(tmp);
            }
        }

        public static void SortListByPathLength(List<Test> listTestPath)
        {
            for (int i = 1; i < listTestPath.Count; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (listTestPath[i].pathLength > listTestPath[j].pathLength)
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
