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

                lineNum++;
                string line = statuesFile.ReadLine();
                if (Globals.gotFlaggerInfo && lineNum == Globals.startFlaggerLineNum) {
                    FlaggerProcessing(line);
                }
                if (Globals.gotBaleInfo && !doneProcessingBales) {
                    if (lineNum >= Globals.balesUpStartLineNum && lineNum <= Globals.balesUpEndLineNum) {
                        BaleProcessing(line, "up", lineNum);
                    }
                    if (lineNum >= Globals.balesDownStartLineNum && lineNum <= Globals.balesDownEndLineNum) {
                        BaleProcessing(line, "down", lineNum);
                    }
                    if (lineNum >= Globals.balesUpEndLineNum && lineNum >= Globals.balesDownEndLineNum) {
                        WriteBaleListToFile();
                        doneProcessingBales = true;
                        Console.WriteLine(Colors.FgGreen + "Done Processing Bales" + Colors.Reset);
                    }
                }
            }

            Console.Write("\nGetting Flame Coords...");
            Console.WriteLine("Done");

            // Get start gate flame coords

            // Get holeshot flame coords

            // Get finish flame coords


            Console.Write("\nGetting Gate Sound Coords...");
            Console.WriteLine("Done");

            Console.Write("\nGetting Bleacher Coords...");
            Console.WriteLine("Done");

            Console.Write("\nGetting Speaker Coords...");
            Console.WriteLine("Done");


            Globals.outputFile.Close();
 
        }

        private static void FlaggerProcessing(string line) {
            Console.WriteLine("\nGetting Start Flagger Coords...");

            // Split line
            string[] lineElements = line.Split(' ');
            Globals.outputFile.WriteLine("FLAGGER COORDINATES: \n");
            Globals.outputFile.WriteLine("var starterStartPos = " + lineElements[0] + ", " + lineElements[2]);
            Globals.outputFile.WriteLine("var starterEndPos = [" + Globals.startFlaggerEndCoords[0] + ", " + 
                Globals.startFlaggerEndCoords[1] + ']');
            Console.WriteLine(Colors.FgGreen + "Done Processing Flagger" + Colors.Reset);
        }

        private static void BaleProcessing(string line, string direction, int lineNum) {
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

            string[] lineElements = line.Split(' ');
            lineElements[2] = lineElements[2].Replace("]", string.Empty);
            Globals.balesList.Add(lineElements[0] + ", " + lineElements[1] + ", " + lineElements[2] + ", " + lineElements[3] + "],");

            if (lineNum == Globals.balesDownEndLineNum || lineNum == Globals.balesUpEndLineNum) {
                Globals.balesList.Add("];");
            }
        }

        private static void WriteBaleListToFile() {
            foreach (string line in Globals.balesList) {
                Globals.outputFile.WriteLine(line);
            }
        }
    }
}
