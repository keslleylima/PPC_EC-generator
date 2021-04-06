using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace PpcEcGenerator.IO
{
    public class CoverageFileFinderTest
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private readonly string projectsFolder;
        private string methodPath;
        private string ppcPrefix;
        private string ecPrefix;
        private string tpPrefix;
        private CoverageFileFinder finder;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public CoverageFileFinderTest()
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
        public void TestFindMetricsFilesAt()
        {
            UsingMethodPath(@"Math\KalmanFilter.predict");
            WithPrimePathCoveragePrefix("TR_PPC");
            WithEdgeCoveragePrefix("TR_EC");
            WithTestPathPrefix("TR_EC");

            FindMetricsFiles();

            AssertPrimePathCoverageFilesWasFound("TR_PPC.txt");
            AssertEdgeCoverageFilesWasFound("TR_EC.txt");
            AssertTestPathFilesWereFound("TP_1.txt", "TP_2.txt");
        }

        [Fact]
        public void TestFinderWithoutPrimePathCoverageFilePrefix()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new CoverageFileFinder.Builder()
                    .EdgeCoveragePrefix("ec")
                    .TestPathPrefix("tp")
                    .Build();
            });
        }

        [Fact]
        public void TestFinderWithoutEdgeCoverageFilePrefix()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new CoverageFileFinder.Builder()
                    .PrimePathCoveragePrefix("ppc")
                    .TestPathPrefix("tp")
                    .Build();
            });
        }

        [Fact]
        public void TestFinderWithoutTestPathFilePrefix()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new CoverageFileFinder.Builder()
                    .PrimePathCoveragePrefix("ppc")
                    .EdgeCoveragePrefix("ec")
                    .Build();
            });
        }

        [Fact]
        public void TestFinderWitNullPrimePathCoverageFilePrefix()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new CoverageFileFinder.Builder()
                    .PrimePathCoveragePrefix(null)
                    .EdgeCoveragePrefix("ec")
                    .TestPathPrefix("tp")
                    .Build();
            });
        }

        [Fact]
        public void TestFinderWithNullEdgeCoverageFilePrefix()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new CoverageFileFinder.Builder()
                    .PrimePathCoveragePrefix("ppc")
                    .EdgeCoveragePrefix(null)
                    .TestPathPrefix("tp")
                    .Build();
            });
        }

        [Fact]
        public void TestFinderWithNullTestPathFilePrefix()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new CoverageFileFinder.Builder()
                    .PrimePathCoveragePrefix("ppc")
                    .EdgeCoveragePrefix("ec")
                    .TestPathPrefix(null)
                    .Build();
            });
        }

        [Fact]
        public void TestFinderWithEmptyPrimePathCoverageFilePrefix()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new CoverageFileFinder.Builder()
                    .PrimePathCoveragePrefix("")
                    .EdgeCoveragePrefix("ec")
                    .TestPathPrefix("tp")
                    .Build();
            });
        }

        [Fact]
        public void TestFinderWithEmptyEdgeCoverageFilePrefix()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new CoverageFileFinder.Builder()
                    .PrimePathCoveragePrefix("ppc")
                    .EdgeCoveragePrefix("")
                    .TestPathPrefix("tp")
                    .Build();
            });
        }

        [Fact]
        public void TestFinderWithEmptyTestPathFilePrefix()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new CoverageFileFinder.Builder()
                    .PrimePathCoveragePrefix("ppc")
                    .EdgeCoveragePrefix("ec")
                    .TestPathPrefix("")
                    .Build();
            });
        }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        private void UsingMethodPath(string path)
        {
            methodPath = path.Replace('\\', Path.DirectorySeparatorChar);
            methodPath = methodPath.Replace('/', Path.DirectorySeparatorChar);
        }

        private void WithPrimePathCoveragePrefix(string prefix)
        {
            ppcPrefix = prefix;
        }

        private void WithEdgeCoveragePrefix(string prefix)
        {
            ecPrefix = prefix;
        }

        private void WithTestPathPrefix(string prefix)
        {
            tpPrefix = prefix;
        }

        private void FindMetricsFiles()
        {
            finder = new CoverageFileFinder.Builder()
                .PrimePathCoveragePrefix(ppcPrefix)
                .EdgeCoveragePrefix(ecPrefix)
                .TestPathPrefix(tpPrefix)
                .Build();

            finder.FindMetricsFilesAt(projectsFolder + methodPath);
        }

        private void AssertPrimePathCoverageFilesWasFound(string file)
        {
            Assert.Equal(file, Path.GetFileName(finder.PrimePathCoverageFile));
        }

        private void AssertEdgeCoverageFilesWasFound(string file)
        {
            Assert.Equal(file, Path.GetFileName(finder.EdgeCoverageFile));
        }

        private void AssertTestPathFilesWereFound(params string[] files)
        {
            List<string> expectedFiles = finder.TestPathFiles;
            
            foreach (string file in files)
            {
                expectedFiles.Add(file);
            }

            foreach (string file in finder.TestPathFiles)
            {
                string filename = Path.GetFileName(file);

                Assert.Contains(filename, expectedFiles);
            }
        }
    }
}
