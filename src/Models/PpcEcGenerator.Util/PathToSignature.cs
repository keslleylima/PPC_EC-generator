using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PpcEcGenerator.Util
{
    public class PathToSignature
    {
        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        private PathToSignature()
        {
        }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        public static string TestPathToSignature(string path)
        {
            string directoryPath = Directory.GetParent(path)?.FullName ?? "";
            List<string> invertedSignature = TPPathToInvertedSignature(directoryPath);
            
            return InvertedSignatureToMethodSignature(invertedSignature);
        }

        private static List<string> TPPathToInvertedSignature(string path)
        {
            if (path.Length == 0)
                return new List<string>();

            string[] terms = path.Split("\\");
            List<string> invertedSignature = new List<string>();

            for (int i = terms.Length - 1; i >= 0; i--)
            {
                if (terms[i] == "results")
                    break;

                invertedSignature.Add(terms[i]);
            }

            return invertedSignature;
        }

        private static string InvertedSignatureToMethodSignature(List<string> invertedSignature)
        {
            StringBuilder builderFinal = new StringBuilder();
            
            for (int i = invertedSignature.Count - 1; i >= 0; i--)
            {
                builderFinal.Append(invertedSignature[i]);
                builderFinal.Append('.');
            }

            builderFinal.Remove(builderFinal.Length - 1, 1);

            return builderFinal.ToString().Replace('$', '.');
        }
    }
}
