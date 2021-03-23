using System;
using System.Collections.Generic;
using System.Text;

namespace PpcEcGenerator.Data
{
    public class Test
    {
        public string path { set; get; }
        public List<string> requirements;
        public int newReqPpcCovered;
        public int overallReqPpcCovered;
        public int newReqEcCovered;
        public int overallReqEcCovered;
        public int pathLength;
        public Test(string path)
        {
            this.path = path;
            this.newReqPpcCovered = 0;
            this.overallReqPpcCovered = 0;
            this.newReqEcCovered = 0;
            this.overallReqEcCovered = 0;
            this.requirements = new List<string>();
            this.pathLength = 0;
        }
    }
}
