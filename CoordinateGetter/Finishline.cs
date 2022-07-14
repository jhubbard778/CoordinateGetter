using System;
using System.Collections.Generic;
using System.Text;

namespace CoordinateGetter {
    public class Finishline {
        public static void DoFinishlineWork(string[] lineElements) {
            if (!Globals.wroteFlameHeader) {
                Console.WriteLine("\nGetting Flame Coords...");
                Globals.flameBillboardOutput.Add("FLAME BILLBOARD COORDINATES:\n");
                Globals.wroteFlameHeader = true;
            }
                        
            // Replace the brackets then parse origin
            lineElements[0] = lineElements[0].Replace("[", string.Empty);
            lineElements[2] = lineElements[2].Replace("]", string.Empty);
            bool canParse = true;
            if (!double.TryParse(lineElements[0], out double center_x)) { canParse = false; }
            if (!double.TryParse(lineElements[1], out double center_y)) { canParse = false; }
            if (!double.TryParse(lineElements[2], out double center_z)) { canParse = false; }
            if (!double.TryParse(lineElements[3], out double angle)) { canParse = false; }
            if (!canParse) {
                Console.WriteLine("Cannot Parse Finish Coords");
                return;
            }
            center_z = -center_z;
            DoFinish(center_x, center_y, center_z, angle);

        }

        private static void DoFinish(double center_x, double center_y, double center_z, double angle) {

            List<string> finishLineFlames = new List<string>();
            List<string> finishFlameBillboards = new List<string>();
            finishLineFlames.Add("FINISH FLAME COORDINATES: \n");
            finishLineFlames.Add("const finishFlameCoords = [");

            int aspect = 1;
            int size = 45;
            string path1 = "@os2022bgsxobj/js/pyro/finishflames.seq";
            string path2 = "@os2022bgsxobj/js/pyro/finishflames2.seq";

            for (int i = 0; i < Globals.FINISH_FLAME_DISTANCES_FROM_ORIGIN.Length; i++) {

                double x = center_x + Globals.FINISH_FLAME_DISTANCES_FROM_ORIGIN[i];
                double z = center_z;
                
                string path = path2;
                if (i % 2 == 0) {
                    path = path1;
                }

                double rot_x = Program.Rotate_X(x, z, center_x, center_z, angle);
                double rot_z = Program.Rotate_Z(x, z, center_x, center_z, angle);
                rot_z = Math.Abs(rot_z);

                // Add to lists
                finishLineFlames.Add("\t[" + rot_x.ToString() + ", " + (Globals.FINISH_FLAME_HEIGHT + center_y).ToString()
                    + ", " + rot_z.ToString() + "],");
                finishFlameBillboards.Add('[' + rot_x.ToString() + ", " + Globals.FINISH_FLAME_HEIGHT.ToString() +
                    ", " + rot_z.ToString() + "] " + size.ToString() + ' ' + aspect.ToString() + ' ' + path);

            }

            finishLineFlames.Add("];\n");
            Globals.flameBillboardOutput.AddRange(finishFlameBillboards);
            Globals.flamesCoords.AddRange(finishLineFlames);
        }
    }
}
