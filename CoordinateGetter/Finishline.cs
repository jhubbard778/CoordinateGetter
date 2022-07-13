using System;
using System.Collections.Generic;
using System.Text;

namespace CoordinateGetter {
    public class Finishline {
        public static void DoFinishlineWork(string[] lineElements) {
            if (!Globals.wroteFlameHeader) {
                Console.WriteLine("\nGetting Flame Coords...");
                Globals.wroteFlameHeader = true;
            }
        }
    }
}
