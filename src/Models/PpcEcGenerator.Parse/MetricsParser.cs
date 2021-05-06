using PpcEcGenerator.IO;
using PpcEcGenerator.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PpcEcGenerator.Util;
using System.Text.RegularExpressions;

namespace PpcEcGenerator.Parse
{
    /// <summary>
    ///     Responsible for parsing metrics files.
    /// </summary>
    public class MetricsParser : IClassObservable
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        /// <summary>
        ///     Key:    idTestPathFile
        ///     Value:  Coverage data
        /// </summary>
        private readonly IDictionary<string, List<Coverage>> coverageData;

        private readonly List<string> metricsDirectories;
        private List<TestPath> listTestPath;
        private List<List<int>> listInfeasiblePaths;
        private Coverage coverage;
        private readonly List<IClassObserver> observers;
        private readonly ProcessingProgress progress;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public MetricsParser(string projectPath)
        {
            if (string.IsNullOrEmpty(projectPath))
                throw new ArgumentException("Project path cannot be empty");

            coverageData = new Dictionary<string, List<Coverage>>();
            observers = new List<IClassObserver>();
            metricsDirectories = new List<string>();
            
            FindDirectories(projectPath);
            progress = new ProcessingProgress(0, metricsDirectories.Count, 0);
            listTestPath = default!;
            listInfeasiblePaths = default!;
            coverage = default!;
        }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        private void FindDirectories(string path)
        {
            foreach (string file in Directory.GetFiles(path, "*.txt", SearchOption.AllDirectories))
            {
                string directory = Directory.GetParent(file)?.FullName ?? "";

                if ((directory != "") && !metricsDirectories.Contains(directory))
                {
                    metricsDirectories.Add(directory);
                }
            }
        }

        public void Attach(IClassObserver observer)
        {
            observers.Add(observer);
        }

        public void Detach(IClassObserver observer)
        {
            observers.Remove(observer);
        }

        public void NotifyAll()
        {
            foreach (IClassObserver observer in observers)
            {
                observer.Update(this, progress);
            }
        }

        public IDictionary<string, List<Coverage>> ParseMetrics(CoverageFileFinder finder)
        {
            if (finder == null)
                throw new ArgumentException("Coverage file finder cannot be null");

            listInfeasiblePaths = new List<List<int>>();

            foreach (string methodPath in metricsDirectories)
            {
                listTestPath = new List<TestPath>();
                
                progress.Forward();
                NotifyAll();

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
                Metric ppc = new Metric(finder.PrimePathCoverageFile);
                Metric ec = new Metric(finder.EdgeCoverageFile);

                ParseInfeasiblePaths(finder.InfeasiblePathFile, ppc, ec);
                ParseTestPathLines(testPathLines);
                SortListByPathLength(listTestPath);
                CalculateCoverage(
                    testPathLines.First(),
                    PathToSignature.TestPathToSignature(finder.TestPathFiles[0]), 
                    ppc, 
                    ec
                );
                StoreCoverage(testPathLines.First());
            }
        }

        private void ParseTestPathLines(string[] fileTestPath)
        {
            foreach (string line in fileTestPath.Distinct().ToArray().Skip(1))
            {
                if (ContainsPath(line))
                    listTestPath.Add(new TestPath(GeneratePathFrom(line)));
            }
        }

        private bool ContainsPath(string line)
        {
            Regex pathRegex = new Regex(".*\\[.+\\].*");
            
            return pathRegex.IsMatch(line);
        }

        private List<int> GeneratePathFrom(string str)
        {
            List<int> path = new List<int>();

            foreach (string lineNumber in ExtractPathFrom(str))
            {
                path.Add(int.Parse(lineNumber));
            }

            return path;
        }

        private string[] ExtractPathFrom(string str)
        {
            // StartPoint is used to remove all char before the "["
            int startPoint = str.IndexOf("[");
            string path = str.Substring(startPoint);

            return path
                .Trim(new char[] { ' ', '[', ']', '\n' })
                .Replace(" ", "")
                .Split(",");
        }

        private void CalculateCoverage(string testMethod, string coveredMethod, Metric ppc, Metric ec)
        {
            coverage = new Coverage(
                testMethod,
                coveredMethod,
                ppc.CalculateCoverage(listTestPath),
                ec.CalculateCoverage(listTestPath)
            );
        }

        private void StoreCoverage(string methodId)
        {
            if (coverageData.ContainsKey(methodId))
            {
                coverageData.TryGetValue(methodId, out var coverageList);

                coverageList?.Add(coverage);
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

        private void ParseInfeasiblePaths(string infPathFile, Metric ppc, Metric ec)
        {
            if (string.IsNullOrEmpty(infPathFile))
                return;

            foreach (string line in File.ReadAllLines(infPathFile))
            {
                if (ContainsPath(line))
                    listInfeasiblePaths.Add(GeneratePathFrom(line));
            }

            ppc.ParseInfeasiblePath(listInfeasiblePaths);
            ec.ParseInfeasiblePath(listInfeasiblePaths);
        }

        private static void SortListByPathLength(List<TestPath> listTestPath)
        {
            for (int i = 1; i < listTestPath.Count; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (listTestPath[i].PathLength > listTestPath[j].PathLength)
                    {
                        TestPath test = listTestPath[i];
                        listTestPath[i] = listTestPath[j];
                        listTestPath[j] = test;
                    }
                }
            }
        }
    }
}
