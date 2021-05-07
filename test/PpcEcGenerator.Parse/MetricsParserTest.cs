using PpcEcGenerator.Data;
using PpcEcGenerator.IO;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace PpcEcGenerator.Parse
{
    public class MetricsParserTest
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private readonly string projectsFolder;
        private string projectName;
        private string ppcPrefix;
        private string ecPrefix;
        private string tpPrefix;
        private string signature;
        private IDictionary<string, List<Coverage>> parsingResult;
        private readonly IDictionary<string, List<Coverage>> expectedResult;



        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public MetricsParserTest()
        {
            projectsFolder = PathManager.GetResourcesPath()
                                + Path.DirectorySeparatorChar
                                + "projects"
                                + Path.DirectorySeparatorChar;

            expectedResult = new Dictionary<string, List<Coverage>>();
        }

        
        //---------------------------------------------------------------------
        //		Tests
        //---------------------------------------------------------------------
        [Fact]
        public void TestParse()
        {
            UsingProject("Math - Reduced");
            UsingPpcPrefix("TR_PPC");
            UsingEcPrefix("TR_EC");
            UsingTpPrefix("TP_");
            DoParsing();

            WithSignature("org.apache.commons.math4.analysis.solvers.FieldBracketingNthOrderBrentSolverTest.testConvergenceOnFunctionAccuracy()");
            ExpectCoverage(7.0/25.0, 18.0/43.0);
            WithSignature("org.apache.commons.math4.dfp.DfpTest.testLog10()");
            ExpectCoverage(1, 1);

            AssertParsingIsAsExpected();
        }

        [Fact]
        public void TestParserWithNullProjectPath()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new MetricsParser(null);
            });
        }

        [Fact]
        public void TestParserWithEmptyProjectPath()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new MetricsParser("");
            });
        }

        [Fact]
        public void TestParserWithNullFinder()
        {
            MetricsParser parser = new MetricsParser(projectsFolder);
            
            Assert.Throws<ArgumentException>(() =>
            {
                parser.ParseMetrics(null);
            });
        }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        private void UsingProject(string projectName)
        {
            this.projectName = projectName;
        }

        private void UsingPpcPrefix(string prefix)
        {
            ppcPrefix = prefix;
        }

        private void UsingEcPrefix(string prefix)
        {
            ecPrefix = prefix;
        }

        private void UsingTpPrefix(string prefix)
        {
            tpPrefix = prefix;
        }

        private void DoParsing()
        {
            MetricsParser parser = new MetricsParser(projectsFolder + projectName + Path.DirectorySeparatorChar);

            CoverageFileFinder finder = new CoverageFileFinder.Builder()
                .PrimePathCoveragePrefix(ppcPrefix)
                .EdgeCoveragePrefix(ecPrefix)
                .TestPathPrefix(tpPrefix)
                .Build();

            parsingResult = parser.ParseMetrics(finder);
        }

        private void WithSignature(string signature)
        {
            this.signature = signature;
        }

        private void ExpectCoverage(double ppc, double ec)
        {
            if (expectedResult.ContainsKey(signature))
            {
                expectedResult.TryGetValue(signature, out List<Coverage> expectedCoverage);

                expectedCoverage.Add(new Coverage(signature, "", ppc, ec));
            }
            else
            {
                List<Coverage> expectedCoverage = new List<Coverage>();
                
                expectedCoverage.Add(new Coverage(signature, "", ppc, ec));

                expectedResult.Add(signature, expectedCoverage);
            }
        }

        private void AssertParsingIsAsExpected()
        {
            AssertSameSize(expectedResult, parsingResult);

            foreach (KeyValuePair<string, List<Coverage>> kvp in expectedResult)
            {
                parsingResult.TryGetValue(kvp.Key, out List<Coverage> coverageObtained);
                List<Coverage> expectedCoverage = kvp.Value;

                AssertSameSize(expectedCoverage, coverageObtained);

                for (int i = 0; i < coverageObtained.Count; i++)
                {
                    AssertPrimePathCoverage(expectedCoverage[i], coverageObtained[i]);
                    AssertEdgeCoverage(expectedCoverage[i], coverageObtained[i]);
                }
            }
        }

        private void AssertSameSize<T1, T2>(IDictionary<T1,T2> d1, IDictionary<T1, T2> d2)
        {
            Assert.Equal(d1.Count, d2.Count);
        }

        private void AssertSameSize<T>(IList<T> l1, IList<T> l2)
        {
            Assert.Equal(l1.Count, l2.Count);
        }

        private void AssertPrimePathCoverage(Coverage expected, Coverage obtained)
        {
            Assert.Equal(
                expected.PrimePathCoverage,
                obtained.PrimePathCoverage,
                2
            );
        }

        private void AssertEdgeCoverage(Coverage expected, Coverage obtained)
        {
            Assert.Equal(
                expected.EdgeCoverage,
                obtained.EdgeCoverage,
                2
            );
        }
    }
}
