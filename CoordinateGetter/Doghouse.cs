using System;
using System.Collections.Generic;
using System.Text;

namespace CoordinateGetter {
    public class Doghouse {

        public static void DoDoghouseWork(string[] lineElements) {
            if (!Globals.wroteFlameHeader) { 
                Console.WriteLine("\nGetting Flame Coords...");
                
                Globals.wroteFlameHeader = true;
            }
            if (!Globals.wroteGateSoundsHeader) {
                Console.WriteLine("\nGetting Gate Sound Coords...");
                Globals.wroteGateSoundsHeader = true;
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
                Console.WriteLine("Cannot Parse Doghouse Coords");
                return;
            }

            GateSounds(center_x, center_z, angle);
            StartFlames(center_x, center_y, center_z, angle);

        }

        private static void StartFlames(double center_x, double center_y, double center_z, double angle) {

            /*
            ##########################################
            Draw Out, Rotate All Points, & Add to List
            ##########################################
            */

            Globals.flameBillboardOutput.Add("FLAME BILLBOARD COORDINATES:\n");
            Globals.startFlameCoords.Add("START FLAME COORDINATES: \n");
            Globals.startFlameCoords.Add("const startFlameCoords = [");

            int modulusFlipper = 0;
            int aspect = 1;
            int size = 10;
            string path1 = "@os2022bgsxobj/js/pyro/startflames.seq";
            string path2 = "@os2022bgsxobj/js/pyro/startflames2.seq";

            List<string> flameShooterBillboards = new List<string>();

            double aspect2 = 0.75;
            int size2 = 35;
            string path3 = "@os2022bgsxobj/js/pyro/holeshotflames.seq";

            for (int i = 0; i < Globals.START_FLAME_DISTANCES_FROM_ORIGIN.Length; i++) {
                double x = center_x + Globals.START_FLAME_DISTANCES_FROM_ORIGIN[i];
                double z = center_z + Globals.DOGHOUSE_FLAME_OFFSET_FROM_ORIGIN;

                // flip z axis since sim is retarded
                z = -z;

                // If we've reached halfway we want to repeat what we just did in reverse so we need to flip the modulus result
                // to pick the correct path
                if (Globals.START_FLAME_DISTANCES_FROM_ORIGIN.Length % 2 == 0 &&
                    i == Globals.START_FLAME_DISTANCES_FROM_ORIGIN.Length / 2) {
                    modulusFlipper = 1;
                }

                string path = path2;
                if (i % 2 == (0 + modulusFlipper)) {
                    path = path1;
                }

                double rot_x = Program.Rotate_X(x, z, center_x, center_z, angle);
                double rot_z = Program.Rotate_Z(x, z, center_x, center_z, angle);

                // Flip back to sim axis
                rot_z = Math.Abs(rot_z);

                // Add to lists
                Globals.startFlameCoords.Add('[' + rot_x.ToString() + ", " + (Globals.START_FLAME_HEIGHT + center_y).ToString()
                    + ", " + rot_z.ToString() + "],");
                Globals.flameBillboardOutput.Add('[' + rot_x.ToString() + ", " + Globals.START_FLAME_HEIGHT.ToString() +
                    ", " + rot_z.ToString() + "] " + size.ToString() + ' ' + aspect.ToString() + ' ' + path);
                flameShooterBillboards.Add('[' + rot_x.ToString() + ", " + Globals.START_FLAME_HEIGHT.ToString() +
                    ", " + rot_z.ToString() + "] " + size2.ToString() + ' ' + aspect2.ToString() + ' ' + path3);
            }

            Globals.startFlameCoords.Add("];\n");
            Globals.flameBillboardOutput.AddRange(flameShooterBillboards);
        }

        private static void GateSounds(double center_x, double center_z, double angle) {
            double left_x = center_x + (Globals.START_FLAME_DISTANCES_FROM_ORIGIN[0] / 2);
            double left_z = center_z;

            double right_x = center_x + (Globals.START_FLAME_DISTANCES_FROM_ORIGIN[^1] / 2);
            double right_z = center_z;

            double rot_left_x = Program.Rotate_X(left_x, left_z, center_x, center_z, angle);
            double rot_left_z = Program.Rotate_Z(left_x, left_z, center_x, center_z, angle);

            double rot_right_x = Program.Rotate_X(right_x, right_z, center_x, center_z, angle);
            double rot_right_z = Program.Rotate_Z(right_x, right_z, center_x, center_z, angle);

            string gateSoundOutput = "var gateSoundPositions = [\n[" + rot_left_x.ToString() + 
                ", 0, " + rot_left_z.ToString() + "],\n[" + center_x.ToString() + ", 0, " + center_z.ToString() + "],\n[" +
                rot_right_x.ToString() + ", 0, " + rot_right_z.ToString() + "],\n];\n";

            Globals.outputFile.WriteLine("GATE SOUNDS: \n\n" + gateSoundOutput);

        }
    }
}
