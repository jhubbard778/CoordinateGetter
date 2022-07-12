using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CoordinateGetter {
    public class Globals {

        public const string DOGHOUSE_PNG = "@os2022bgsxobj/statue/other/doghouse/doghouse.png";
        public const string HOLESHOT_PNG = "@@os2022bgsxobj/statue/other/holeshot.png";
        public const string BLEACHER_PNG = "@os2022bgsxobj/statue/other/estrades.png";
        public const string FINISH_PNG = "@os2022bgsxobj/statue/other/finish/finishline.png";
        public const string SPEAKER_PNG = "@os2022bgsxobj/statue/other/speaker.png";

        public static bool statuesExist = false;
        public static double[] startFlaggerEndCoords;
        public static int startFlaggerLineNum;

        public static int balesUpStartLineNum;
        public static int balesUpEndLineNum;
        public static int balesDownStartLineNum;
        public static int balesDownEndLineNum;

        public static int numOfLines;

        public static bool gotFlaggerInfo = false;
        public static bool gotBaleInfo = false;
        public static bool wroteBaleUpHeader = false;
        public static bool wroteBaleDownHeader = false;

        public static List<string> balesList = new List<string>();

        public static StreamWriter outputFile;
    }
}
