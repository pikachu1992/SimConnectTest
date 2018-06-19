using System.Runtime.InteropServices;

namespace SimLib
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class Radios
    {
        public int Transponder;
        public double ActiveCOM1;
        public double StandbyCOM1;
        public double ActiveCOM2;
        public double StandbyCOM2;
    }
}
