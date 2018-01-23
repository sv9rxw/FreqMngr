using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreqMngr
{
    public static class Modulation
    {
        public static String CW { get; } = "CW";
        public static String USB { get; } = "USB";
        public static String LSB { get; } = "LSB";
        public static String DSB { get; } = "DSB";
        public static String AM { get; } = "AM";
        public static String FM { get; } = "AM";
        public static String FSK { get; } = "FSK";
        public static String PSK { get; } = "PSK";
        public static String MSK { get; } = "MSK";
        public static String CPM { get; } = "CPM";
        public static String APSK { get; } = "APSK";
        public static String PPM { get; } = "PPM";
        public static String SCFDMA { get; } = "SC-FDMA";
        public static String TCM { get; } = "TCM";
        public static String WDM { get; } = "WDM";
        public static String Unknown { get; } = "?";
    }
}
