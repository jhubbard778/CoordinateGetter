using System;
using System.Collections.Generic;
using System.Text;

namespace CoordinateGetter {
    public class Holeshot {
        public static void DoHoleshotWork(string[] lineElements) {
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
            DoHoleshot(center_x, center_y, center_z, angle);

        }

        private static void DoHoleshot(double center_x, double center_y, double center_z, double angle) {
            double p1x = center_x + Globals.HOLESHOT_FLAME_DISTANCES_FROM_ORIGIN[0];
            double p1z = center_z;

            double p2x = center_x + Globals.HOLESHOT_FLAME_DISTANCES_FROM_ORIGIN[1];
            double p2z = center_z;

            double p1rotx = Program.Rotate_X(p1x, p1z, center_x, center_z, angle);
            double p1rotz = Program.Rotate_Z(p1x, p1z, center_x, center_z, angle);

            double p2rotx = Program.Rotate_X(p2x, p2z, center_x, center_z, angle);
            double p2rotz = Program.Rotate_Z(p2x, p2z, center_x, center_z, angle);

            p1rotz = Math.Abs(p1rotz);
            p2rotz = Math.Abs(p2rotz);

            string holeshotOutput = "HOLESHOT FLAME COORDINATES:\n\nconst holeshotCoords = [\n\t[" + p1rotx.ToString() +
                ", " + (Globals.HOLESHOT_FLAME_HEIGHT + center_y).ToString() + ", " + p1rotz.ToString() + "],\n\t[" +
                p2rotx.ToString() + ", " + (Globals.HOLESHOT_FLAME_HEIGHT + center_y).ToString() + ", " +
                p2rotz.ToString() + "]\n];\n";


            int size = 25;
            double aspect = 0.5;
            string path = "@os2022bgsxobj/js/pyro/holeshotflames.seq";
            string holeshotBillboardOutput = '[' + p1rotx.ToString() + ", " + Globals.HOLESHOT_FLAME_HEIGHT.ToString() + ", " +
                p1rotz.ToString() + "] " + size.ToString() + ' ' + aspect.ToString() + ' ' + path + "\n[" +
                p2rotx.ToString() + ", " + Globals.HOLESHOT_FLAME_HEIGHT.ToString() + ", " + p2rotz.ToString() + "] " +
                size.ToString() + ' ' + aspect.ToString() + ' ' + path;

            Globals.flamesCoords.Add(holeshotOutput);
            Globals.flameBillboardOutput.Add(holeshotBillboardOutput);
        }

    }
}
