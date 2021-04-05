using PccEcGenerator.Data;
using PpcEcGenerator.Data;
using PpcEcGenerator.Export;
using PpcEcGenerator.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TestPathConsole
{
    class PpcEcGenerator
    {
        /// <summary>
        ///     Key:    idTestPathFile
        ///     Value:  Coverage data
        /// </summary>
        Dictionary<string, List<Coverage>> coverageData;


        public void Run()
        {
            string projectPath = @"C:\Users\william\Documents\workspace\vsstudio\PPC_EC-generator\Projects\Math";
            string ppcPrefix = "PPC_";
            string ecPrefix = "EC_";
            string tpPrefix = "TP_";
            string infPrefix = "INF_";
            CsvGeneratorTestCase(projectPath, ppcPrefix, ecPrefix, tpPrefix, infPrefix);
        }



        // ppc ec gen
        public string CsvGeneratorTestCase(string projectPath, string ppcPrefix, string ecPrefix, string tpPrefix, string infPrefix)
        {
            string filePath = projectPath + @"\TestCase_metrics.csv";
            Console.WriteLine("OUTPUT PATH: " + filePath);

            if (File.Exists(filePath))
                File.Delete(filePath);

            List<string> listInfeasiblePaths = new List<string>();
            CoverageFileFinder finder = new CoverageFileFinder(ppcPrefix, ecPrefix, tpPrefix, infPrefix);

            foreach (string methodPath in Directory.GetDirectories(projectPath))
            {
                finder.FindMetricsFilesAt(methodPath);
                ParseMetrics(finder, filePath, listInfeasiblePaths);
            }

            return filePath;
        }






        // PARSER
        private void ParseMetrics(CoverageFileFinder finder, string filePath, List<string> listInfeasiblePaths)
        {
            List<Test> listTestPath = new List<Test>();

            foreach (string testPathFile in finder.TestPathsFiles)
            {
                List<Requirement> listReqPpc = CreatePPCList(finder.ReqFilePpc);
                List<Requirement> listReqEc = CreateECList(finder.ReqFileEc);
                ParseInfeasiblePaths(finder.InfPathFile, listInfeasiblePaths, listReqPpc, listReqEc);

                String[] fileTestPath = File.ReadAllLines(testPathFile);
                CreateListTestPaths(fileTestPath, listTestPath);

                foreach (Test testPath in listTestPath)
                {
                    testPath.pathLength = CalculatePathLength(testPath);
                }

                SortListByPathLength(listTestPath);
                CountReqPpcCovered(listReqPpc, listTestPath);
                CountReqNcCovered(listReqEc, listTestPath);

                Coverage coverage = new Coverage();
                coverage.Calculate(listTestPath, listReqPpc, listReqEc);

                if (coverageData.ContainsKey(fileTestPath.First()))
                {
                    coverageData.TryGetValue(fileTestPath.First(), out List<Coverage> coverageList);
                    coverageList.Add(coverage);
                }
                else
                {
                    List<Coverage> coverageList = new List<Coverage>();
                    coverageList.Add(coverage);
                    coverageData.Add(fileTestPath.First(), coverageList);
                }
            }


            CoverageCSVExporter exporter = new CoverageCSVExporter(filePath, coverageData);
            exporter.Export();
        }

        private List<Requirement> CreatePPCList(string reqFilePpc)
        {
            string[] fileReqPpc = File.ReadAllLines(reqFilePpc);
            List<Requirement> listReqPpc = CreatListReq(fileReqPpc);
            return listReqPpc;
        }

        private List<Requirement> CreateECList(string reqFileEc)
        {
            string[] fileReqEc = File.ReadAllLines(reqFileEc);
            List<Requirement> listReqEc = CreatListReq(fileReqEc);
            return listReqEc;
        }

        private void ParseInfeasiblePaths(string infPathFile, List<string> listInfeasiblePaths, List<Requirement> listReqPpc, List<Requirement> listReqEc)
        {
            if (string.IsNullOrEmpty(infPathFile))
                return;
            
            string[] fileInfeasiblePaths = File.ReadAllLines(infPathFile);

            CreateListInfeasiblePaths(fileInfeasiblePaths, listInfeasiblePaths);
            CheckInfeasibleReq(listInfeasiblePaths, listReqPpc);
            CheckInfeasibleReq(listInfeasiblePaths, listReqEc);
        }

        public static int CalculatePathLength(Test testPath)
        {
            return testPath.path.Split(',').Length;
        }

        public static List<Requirement> CreatListReq(string[] fileReq)
        {
            List<Requirement> listReq = new List<Requirement>();

            foreach (string req in fileReq)
            {
                // StartPoint is used to remove all char before the "["
                int startPoint = req.IndexOf("[");
                string trProcessed = req.Substring(startPoint);
                Requirement requirement = new Requirement(trProcessed.Trim(new Char[] { ' ', '[', ']', '\n' }));
                listReq.Add(requirement);
            }

            return listReq;
        }

        public static void CreateListTestPaths(string[] fileTestPath, List<Test> listTestPath)
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

        public static void CheckInfeasibleReq(List<string> listInfeasiblePaths, List<Requirement> listReq)
        {
            foreach (string infeasiblePath in listInfeasiblePaths)
            {
                foreach (Requirement requirement in listReq)
                {
                    if (requirement.path.Contains(infeasiblePath))
                    {
                        requirement.feasible = false;
                    }
                }
            }
        }

        public static void CountReqPpcCovered(List<Requirement> listReqPpc, List<Test> listTestPath)
        {
            foreach (Requirement requirement in listReqPpc)
            {
                foreach (Test test in listTestPath)
                {
                    if (test.path.Contains(requirement.path) && requirement.feasible == true)
                    {
                        if (requirement.covered == false)
                        {
                            test.newReqPpcCovered = test.newReqPpcCovered + 1;
                            requirement.covered = true;
                        }
                        test.overallReqPpcCovered = test.overallReqPpcCovered + 1;
                        test.requirements.Add(requirement.path);
                        requirement.testPaths.Add(test.path);
                    }
                }
            }
        }

        public static void CountReqNcCovered(List<Requirement> listReqNc, List<Test> listTestPath)
        {
            foreach (Requirement requirement in listReqNc)
            {
                foreach (Test test in listTestPath)
                {
                    if (test.path.Contains(requirement.path) && requirement.feasible == true)
                    {
                        if (requirement.covered == false)
                        {
                            test.newReqEcCovered = test.newReqEcCovered + 1;
                            requirement.covered = true;
                        }
                        test.overallReqEcCovered = test.overallReqEcCovered + 1;
                        test.requirements.Add(requirement.path);
                        requirement.testPaths.Add(test.path);
                    }
                }
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