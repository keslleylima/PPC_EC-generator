using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PpcEcGenerator.IO
{
    public class CoverageFileFinder
    {
        private readonly string ppcPrefix;
        private readonly string ecPrefix;
        private readonly string tpPrefix;
        private readonly string infPrefix;

        public CoverageFileFinder(string ppcPrefix, string ecPrefix, string tpPrefix, string infPrefix)
        {
            this.ppcPrefix = ppcPrefix;
            this.ecPrefix = ecPrefix;
            this.tpPrefix = tpPrefix;
            this.infPrefix = infPrefix;
        }

        public string ReqFilePpc { get; private set; }
        public string ReqFileEc { get; private set; }
        public List<string> TestPathsFiles { get; private set; }
        public string InfPathFile { get; private set; }

        public void FindMetricsFilesAt(string rootPath)
        {
            ReqFilePpc = string.Empty;
            ReqFileEc = string.Empty;
            TestPathsFiles = new List<string>();
            InfPathFile = string.Empty;

            foreach (string file in GetTextFilesFromDirectory(rootPath))
            {
                if (file.Contains(ppcPrefix))
                {
                    ReqFilePpc = file;
                }
                else if (file.Contains(ecPrefix))
                {
                    ReqFileEc = file;
                }
                else if (file.Contains(tpPrefix))
                {
                    TestPathsFiles.Add(file);
                }
                else if (file.Contains(infPrefix))
                {
                    InfPathFile = file;
                }
            }
        }

        public static string[] GetTextFilesFromDirectory(string directoryPath)
        {
            return Directory.GetFiles(
                directoryPath, 
                "*.txt", 
                SearchOption.AllDirectories
            );
        }
    }
}
