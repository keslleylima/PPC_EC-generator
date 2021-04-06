using PpcEcGenerator.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PpcEcGenerator.Export
{
    public class CoverageCSVExporter : IExporter
    {
        private static readonly string DELIMITER = ";";
        private string output;
        private IDictionary<string, List<Coverage>> coverage;


        public CoverageCSVExporter(string output, IDictionary<string, List<Coverage>> coverage)
        {
            this.output = output;
            this.coverage = coverage;
        }


        public void Export()
        {
            StringBuilder sb = new StringBuilder();

            if (!File.Exists(output))
                sb.Append("Id;EdgeCoverage;PrimePathCoverage\n");

            foreach (KeyValuePair<string, List<Coverage>> kvp in coverage)
            {
                foreach (Coverage c in kvp.Value)
                {
                    sb.Append(kvp.Key);
                    sb.Append(DELIMITER);
                    sb.Append(c.EdgeCoverage);
                    sb.Append(DELIMITER);
                    sb.Append(c.PrimePathCoverage);
                    sb.Append(DELIMITER);
                    sb.Append('\n');
                }
            }

            File.AppendAllText(output, sb.ToString());
        }
    }
}
