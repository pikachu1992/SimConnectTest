using System;
using System.Runtime.InteropServices;

namespace SimLib
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class Position
    {
        // this is how you declare a fixed size string 
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public String title;
        public double latitude;
        public double longitude;
        public double altitude;
        public double pitch;
        public double bank;
    }
}
