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
            UsingProject("Math - reduced");
            UsingPpcPrefix("TR_PPC");
            UsingEcPrefix("TR_EC");
            UsingTpPrefix("TP_");
            DoParsing();

            WithSignature("org.apache.commons.math4.analysis.solvers.FieldBracketingNthOrderBrentSolverTest.testConvergenceOnFunctionAccuracy()");
            ExpectCoverage(0.28, 0.41);
            WithSignature("org.apache.commons.math4.dfp.DfpTest.testLog10()");
            ExpectCoverage(2.75, 2.63);
            WithSignature("org.apache.commons.math4.dfp.DfpTest.testIsZero()");
            ExpectCoverage(6.5, 5.14);
            WithSignature("org.apache.commons.math4.ExtendedFieldElementAbstractTest.testLinearCombinationFaFa()");
            ExpectCoverage(2.83, 4.88);
            WithSignature("org.apache.commons.math4.ExtendedFieldElementAbstractTest.testMultiplyInt()");
            ExpectCoverage(9, 9.79);

            AssertParsingIsAsExpected();
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
            MetricsParser parser = new MetricsParser(projectsFolder + projectName);

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

                expectedCoverage.Add(new Coverage(ppc, ec));
            }
            else
            {
                List<Coverage> expectedCoverage = new List<Coverage>();
                
                expectedCoverage.Add(new Coverage(ppc, ec));

                expectedResult.Add(signature, expectedCoverage);
            }

            signature = string.Empty;
            ppc = 0.0;
            ec = 0.0;
        }

        private void AssertParsingIsAsExpected()
        {
            Assert.Equal(expectedResult, parsingResult);
        }
    }
}
