using System;
using System.Runtime.InteropServices;

namespace SimLib
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class AITraffic
    {
        // this is how you declare a fixed size string 
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public String trafficID;
    }
}
