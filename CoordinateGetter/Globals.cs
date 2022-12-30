using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CoordinateGetter {
    public class Globals {

        public static bool statuesExist = false;
        public static bool gotFlaggerInfo = false;
        public static bool gotBaleInfo = false;
        public static bool wroteBaleUpHeader = false;
        public static bool wroteBaleDownHeader = false;
        public static bool wroteBleacherHeader = false;
        public static bool wroteSpeakerHeader = false;
        public static bool wroteGateSoundsHeader = false;
        public static bool wroteFlameHeader = false;

        public const string DOGHOUSE_PNG = "@os2022bgsxfobj/statue/other/doghouse/doghouse.png";
        public const string HOLESHOT_PNG = "@os2022bgsxfobj/statue/other/holeshot.png";
        public const string BLEACHER_PNG = "@os2022bgsxfobj/statue/other/estrades.png";
        public const string FINISH_PNG = "@os2022bgsxfobj/statue/other/finish/finishline.png";
        public const string SPEAKER_PNG = "@os2022bgsxfobj/statue/other/speaker.png";
        public const double DOGHOUSE_FLAME_OFFSET_FROM_ORIGIN = 2.31;
        public const double BLEACHER_SOUND_HEIGHT = 5.5;
        public const double HOLESHOT_FLAME_HEIGHT = 7.25;
        public const double FINISH_FLAME_HEIGHT = 20.5;
        public const double SPEAKER_SOUND_HEIGHT = 30;
        public const double START_FLAME_HEIGHT = 9;

        // Measurements in ft away from origin with respect to x direction
        public readonly static double[] START_FLAME_DISTANCES_FROM_ORIGIN = new double[] { -52.42, -39.04, -17.45, -4.09, 4.09, 17.45, 39.04, 52.42 };
        public readonly static double[] HOLESHOT_FLAME_DISTANCES_FROM_ORIGIN = new double[] { 2.42, 14.06 };
        public readonly static double[] FINISH_FLAME_DISTANCES_FROM_ORIGIN = new double[] { -12.87, -46.88 };
        public static double[] startFlaggerEndCoords;


        public static int startFlaggerLineNum;
        public static int balesUpStartLineNum;
        public static int balesUpEndLineNum;
        public static int balesDownStartLineNum;
        public static int balesDownEndLineNum;
        public static int numOfLines;

        public static List<string> balesList = new List<string>();
        public static List<string> speakersList = new List<string>();
        public static List<string> bleachersList = new List<string>();
        public static List<string> flamesCoords = new List<string>();
        public static List<string> flameBillboardOutput = new List<string>();
        public static List<string> flameIndices = new List<string>();

        public static StreamWriter outputFile;
    }
}
