using System;
using System.Collections.Generic;
using System.Text;

namespace CoordinateGetter {
    public class Holeshot {
        public static void DoHoleshotWork(string[] lineElements) {
            if (!Globals.wroteFlameHeader) {
                Console.WriteLine("\nGetting Flame Coords...");
                Globals.wroteFlameHeader = true;
            }
        }

    }
}
