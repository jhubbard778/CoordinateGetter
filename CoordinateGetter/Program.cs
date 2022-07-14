using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CoordinateGetter
{
    class Program {
        static void Main(string[] args) {

            // Instantiate ui class
            UserInterface ui = new UserInterface();

            while (!Globals.statuesExist) {
                ui.PromptStatues();
            }

            StreamReader statuesFile = File.OpenText(Environment.CurrentDirectory + "\\statues");
            Globals.numOfLines = File.ReadLines(Environment.CurrentDirectory + "\\statues").Count();
            
            ui.GetInfo();

            int lineNum = 0;
            bool doneProcessingBales = false;
            // Create output file
            Globals.outputFile = File.CreateText(Environment.CurrentDirectory + "\\output.txt");
            while (statuesFile.EndOfStream == false) {

                // Increment the line number, read the line and split it on a white space
                lineNum++;
                string line = statuesFile.ReadLine();
                string[] lineElements = line.Split(' ');

                // if we got flagger info from user and the line number is equal to the input then do flagger processing
                if (Globals.gotFlaggerInfo && lineNum == Globals.startFlaggerLineNum) {
                    FlaggerProcessing(lineElements);
                    continue;
                }

                // If we got bale info and we haven't finished processing bales do bale work
                if (Globals.gotBaleInfo && !doneProcessingBales) {

                    // If we're between line numbers inputted by user for moving bales do bale processing
                    if (lineNum >= Globals.balesUpStartLineNum && lineNum <= Globals.balesUpEndLineNum) {
                        BaleProcessing(lineElements, "up", lineNum);
                    }
                    if (lineNum >= Globals.balesDownStartLineNum && lineNum <= Globals.balesDownEndLineNum) {
                        BaleProcessing(lineElements, "down", lineNum);
                    }

                    // If we've reached a line number greater than the last line numbers for both moving down and up,
                    // we've finished processing bales
                    if (lineNum >= Globals.balesUpEndLineNum && lineNum >= Globals.balesDownEndLineNum) {
                        // Write the bale list to the output file
                        WriteListToFile(Globals.balesList);
                        doneProcessingBales = true;
                        Console.WriteLine(Colors.FgGreen + "Done Processing Bales" + Colors.Reset);
                    }
                    continue;
                }

                // If the line includes a doghouse png image do doghouse work
                if (line.Contains(Globals.DOGHOUSE_PNG)) {
                    Doghouse.DoDoghouseWork(lineElements);
                    continue;
                }

                // and etc for holeshot, finish, bleacher, and speakers
                if (line.Contains(Globals.HOLESHOT_PNG)) {
                    Holeshot.DoHoleshotWork(lineElements);
                    continue;
                }

                if (line.Contains(Globals.FINISH_PNG)) {
                    Finishline.DoFinishlineWork(lineElements);
                    continue;
                }

                if (line.Contains(Globals.BLEACHER_PNG)) {
                    DoBleacher(lineElements);
                    continue;
                }

                if (line.Contains(Globals.SPEAKER_PNG)) {
                    DoSpeaker(lineElements);
                    continue;
                }

            }

            // Write out we're done processing these items if we've reached the end of the file with no more items found

            if (Globals.wroteGateSoundsHeader) { Console.WriteLine(Colors.FgGreen + "Done Processing Gate Sounds" + Colors.Reset); }
            if (Globals.wroteSpeakerHeader) {
                Globals.speakersList.Add("];\n");
                WriteListToFile(Globals.speakersList);
                Console.WriteLine(Colors.FgGreen + "Done Processing Speakers" + Colors.Reset);
            }
            if (Globals.wroteBleacherHeader) {
                Globals.bleachersList.Add("];\n");
                WriteListToFile(Globals.bleachersList);
                Console.WriteLine(Colors.FgGreen + "Done Processing Bleachers" + Colors.Reset);
            }

            if (Globals.wroteFlameHeader) { 
                if (Globals.flamesCoords.Count > 0) {
                    WriteListToFile(Globals.flamesCoords);
                }
                if (Globals.flameBillboardOutput.Count > 0) {
                    WriteListToFile(Globals.flameBillboardOutput);
                }
                Console.WriteLine(Colors.FgGreen + "Done Processing Flames" + Colors.Reset); 
            }

            // close the output file
            Globals.outputFile.Close();
 
        }

        private static void FlaggerProcessing(string[] lineElements) {
            Console.WriteLine("\nGetting Start Flagger Coords...");

            Globals.outputFile.WriteLine("FLAGGER COORDINATES: \n");
            Globals.outputFile.WriteLine("var starterStartPos = " + lineElements[0] + ", " + lineElements[2]);
            Globals.outputFile.WriteLine("var starterEndPos = [" + Globals.startFlaggerEndCoords[0] + ", " + 
                Globals.startFlaggerEndCoords[1] + "]\n");

            Console.WriteLine(Colors.FgGreen + "Done Processing Flagger" + Colors.Reset);
        }

        private static void BaleProcessing(string[] lineElements, string direction, int lineNum) {
            if (!Globals.wroteBaleUpHeader && !Globals.wroteBaleDownHeader) {
               Console.WriteLine("\nGetting Bale Coords...");
               Globals.balesList.Add("BALE COORDINATES:");
            }
            if (direction == "up" && !Globals.wroteBaleUpHeader) {
                Globals.balesList.Add("\nconst baleUpStartIndex = " + (Globals.balesUpStartLineNum - 1).ToString());
                Globals.balesList.Add("const baleCoordsUp = [");
                Globals.wroteBaleUpHeader = true;
            }
            if (direction == "down" && !Globals.wroteBaleDownHeader) {
                Globals.balesList.Add("\nconst baleDownStartIndex = " + (Globals.balesDownStartLineNum - 1).ToString());
                Globals.balesList.Add("const baleCoordsDown = [");
                Globals.wroteBaleDownHeader = true;
            }

            lineElements[2] = lineElements[2].Replace("]", string.Empty);
            Globals.balesList.Add(lineElements[0] + ", " + lineElements[1] + ", " + lineElements[2] + ", " + lineElements[3] + "],");

            if (lineNum == Globals.balesDownEndLineNum || lineNum == Globals.balesUpEndLineNum) {
                Globals.balesList.Add("];");
            }
        }

        private static void DoBleacher(string[] lineElements) {
            if (!Globals.wroteBleacherHeader) {
                Console.WriteLine("\nGetting Bleacher Coords...");
                Globals.bleachersList.Add("\nBLEACHER COORDS:\n");
                Globals.bleachersList.Add("const bleacherSoundPositions = [");
                Globals.wroteBleacherHeader = true;
            }

            if (!double.TryParse(lineElements[1], out double y)) {
                Console.WriteLine("Error Processing Bleacher #" + (Globals.bleachersList.Count + 1).ToString());
                return;
            }

            Globals.bleachersList.Add('\t' + lineElements[0] + ", " + (Globals.BLEACHER_SOUND_HEIGHT + y).ToString() + ", " + lineElements[2]);
        }

        private static void DoSpeaker(string[] lineElements) {
            if (!Globals.wroteSpeakerHeader) {
                Console.WriteLine("\nGetting Speaker Coords...");
                Globals.speakersList.Add("\nSPEAKER COORDS:\n");
                Globals.speakersList.Add("const speakerPositions = [");
                Globals.wroteSpeakerHeader = true;
            }

            if (!double.TryParse(lineElements[1], out double y)) {
                Console.WriteLine("Error Processing Speaker #" + (Globals.speakersList.Count + 1).ToString());
                return;
            }

            Globals.speakersList.Add('\t' + lineElements[0] + ", " + (Globals.SPEAKER_SOUND_HEIGHT + y).ToString() + ", " + lineElements[2]);

        }

        private static void WriteListToFile(List<string> list) {
            foreach (string line in list) {
                Globals.outputFile.WriteLine(line);
            }
        }

        public static double Rotate_X(double x0, double z0, double xc, double zc, double angle) {
            return (x0 - xc) * Math.Cos(angle) - (z0 - zc) * Math.Sin(angle) + xc;
        }
        public static double Rotate_Z(double x0, double z0, double xc, double zc, double angle) {
            return (x0 - xc) * Math.Sin(angle) + (z0 - zc) * Math.Cos(angle) + zc;
        }

        public static void WriteFlameHeader() {
            Console.WriteLine("\nGetting Flame Coords...");
        }
    }
}
