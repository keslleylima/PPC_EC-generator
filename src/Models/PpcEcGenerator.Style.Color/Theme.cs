using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PpcEcGenerator.Style.Color
{
    public class Theme
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private static readonly string hex = "#500047";
        private static readonly IBrush brush = Avalonia.Media.Brush.Parse(hex);


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        private Theme()
        {
        }


        //---------------------------------------------------------------------
        //		Properties
        //---------------------------------------------------------------------
        public static string Hex { get => hex; }
        public static IBrush Brush { get => brush; }
    }
}
