using Microsoft.FlightSimulator.SimConnect;
using System;
using System.Runtime.InteropServices;

namespace SimLib
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class AircraftState
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public String title;
        public double latitude;
        public double longitude;
        public double altitude;
        public double pitch;
        public double bank;
        public uint magHeading;
        public double heading;
        public uint airspeed;
        public uint onGround;
        public uint gearPosition;
        public uint landingLight;
        public uint strobeLight;
        public uint beaconLight;
        public uint navLight;
        public uint taxiLight;
        public uint recognitionLight;
        public double flapsPosition;
        public uint isGearRetractable;
        public double elevatorPosition;
        public double aileronPosition;
        public double rudderPosition;
        public uint engineRunning1;
        public uint engineRunning2;
        public uint engineRunning3;
        public uint engineRunning4;
        public uint throttlePositionEngine1;
        public uint throttlePositionEngine2;
        public uint throttlePositionEngine3;
        public uint throttlePositionEngine4;
        public uint strobeLight;
        public uint beaconLight;
        public uint navLight;
        public uint taxiLight;
        public uint recognitionLight;
    }
}
