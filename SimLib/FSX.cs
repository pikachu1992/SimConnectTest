using Microsoft.FlightSimulator.SimConnect;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimLib
{
    public static class FSX
    {
        public static SimConnect Sim;

        internal static Dictionary<int, Type> idMap = new Dictionary<int, Type>();

        internal static Dictionary<Type, int> typeMap = new Dictionary<Type, int>();
    }
}
