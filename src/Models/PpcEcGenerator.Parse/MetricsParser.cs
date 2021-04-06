using PccEcGenerator.Data;
using PpcEcGenerator.Data;
using PpcEcGenerator.Export;
using PpcEcGenerator.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PpcEcGenerator.Parse
{
    public class MetricsParser
    {
        /// <summary>
        ///     Key:    idTestPathFile
        ///     Value:  Coverage data
        /// </summary>
        Dictionary<string, List<Coverage>> coverageData;

        List<Test> listTestPath = new List<Test>();
        string projectPath;

        public MetricsParser(string projectPath)
        {
            this.projectPath = projectPath;
        }

        public Dictionary<string, List<Coverage>> ParseMetrics(CoverageFileFinder finder)
        {
            List<string> listInfeasiblePaths = new List<string>();

            foreach (string methodPath in Directory.GetDirectories(projectPath))
            {
                finder.FindMetricsFilesAt(methodPath);

                foreach (string testPathFile in finder.TpFiles)
                {
                    List<Requirement> listReqPpc = CreatePPCList(finder.PpcFile);
                    List<Requirement> listReqEc = CreateECList(finder.EcFile);
                    ParseInfeasiblePaths(finder.InfFile, listInfeasiblePaths, listReqPpc, listReqEc);

                    String[] fileTestPath = File.ReadAllLines(testPathFile);
                    CreateListTestPaths(fileTestPath);

                    foreach (Test testPath in listTestPath)
                    {
                        testPath.pathLength = CalculatePathLength(testPath);
                    }

                    SortListByPathLength(listTestPath);
                    CountReqPpcCovered(listReqPpc);
                    CountReqNcCovered(listReqEc);

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
            }

            return coverageData;
            
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

        public void CreateListTestPaths(string[] fileTestPath)
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

        public void CountReqPpcCovered(List<Requirement> listReqPpc)
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

        public void CountReqNcCovered(List<Requirement> listReqNc)
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
