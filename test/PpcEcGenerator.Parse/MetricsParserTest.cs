using PpcEcGenerator.Data;
using PpcEcGenerator.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PpcEcGenerator.Parse
{
    public class MetricsParserTest
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private readonly string projectsFolder;
        private CoverageFileFinder finder;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public MetricsParserTest()
        {
            projectsFolder = PathManager.GetResourcesPath()
                                + Path.DirectorySeparatorChar
                                + "projects"
                                + Path.DirectorySeparatorChar;
        }


        //---------------------------------------------------------------------
        //		Tests
        //---------------------------------------------------------------------
        [Fact]
        public void TestParse()
        {
            string joptProject = projectsFolder + "exp4j";
            MetricsParser parser = new MetricsParser(joptProject);

            CoverageFileFinder finder = new CoverageFileFinder.Builder()
                .PrimePathCoveragePrefix("TR_PPC")
                .EdgeCoveragePrefix("TR_EC")
                .TestPathPrefix("TP_")
                .Build();

            IDictionary<string, List<Coverage>> dict = parser.ParseMetrics(finder);
        }
    }
}
