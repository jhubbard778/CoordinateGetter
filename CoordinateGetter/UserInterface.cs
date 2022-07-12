using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CoordinateGetter {
    class UserInterface {

        public void PromptStatues() {
            Console.WriteLine("> Copy Statues File into Application Directory");
            Console.WriteLine("> Press any key to continue...");
            Console.ReadKey(true);
            if (File.Exists(Environment.CurrentDirectory + "\\statues")) {
                Globals.statuesExist = true;
                return;
            }
            Console.WriteLine("\n\x1b[31m> No File named 'statues' found!\x1b[0m\n");
        }

        public void GetInfo() {
            Console.WriteLine("\nBefore we start, we need some info.");
            Console.Write('\n' + Colors.FgYellow + "Do you want to output flagger coordinates? y or n" + Colors.Reset + "\n> ");
            bool answer = UserAnswer();
            bool returnVal;
            if (answer == true) {
                returnVal = GetStartFlaggerInfo();
                while (!returnVal) {
                    returnVal = GetStartFlaggerInfo();
                }
                Globals.gotFlaggerInfo = true;
            }
            Console.Write('\n' + Colors.FgYellow + "Do you want to output bale coordinates? y or n" + Colors.Reset + "\n> ");
            answer = UserAnswer();
            if (answer == true) {
                returnVal = GetBaleIndices();
                while (!returnVal) {
                    returnVal = GetBaleIndices();
                }
                Globals.gotBaleInfo = true;
            }
        }

        /// <summary>
        /// User confirms that their inputs are correct
        /// </summary>
        /// <returns>Boolean whether it is correct or not</returns>
        private static bool UserAnswer() {
            bool answer = false;
            string input = Console.ReadLine().ToLower();
            while (input != "y" && input != "n") {
                Console.Write(Colors.FgRed + "Enter y or n" + Colors.Reset + "\n> ");
                input = Console.ReadLine().ToLower();
            }
            if (input == "y") {
                answer = true;
            }
            return answer;
        }

        /// <summary>
        /// Prompts user and stores coordinate inputs as global variables
        /// </summary>
        private static bool GetStartFlaggerInfo() {

            Console.Write('\n' + Colors.FgGreen + "Enter Start Flagger Line Number: " + Colors.Reset + "\n> ");
            int ln = GetLineNumberInput();

            Console.Write('\n' + Colors.FgGreen + "Enter Start Flagger End X Coordinate: " + Colors.Reset + "\n> ");
            double x = GetCoordinateInput();

            Console.Write('\n' + Colors.FgGreen + "Enter Start Flagger Z Coordinate: " + Colors.Reset + "\n> ");
            double z = GetCoordinateInput();

            Console.WriteLine("\nCoordinates for flagger end position are [" + x.ToString() + ", " + z.ToString() + "]");

            // If the user does not confirm that these are correct and wants to redo, restart function
            Console.Write(Colors.FgYellow + "Are you sure? y or n" + Colors.Reset + "\n> ");
            bool confirmed = UserAnswer();
            if (!confirmed) {
                return false;
            }

            Globals.startFlaggerEndCoords = new double[] { x, z };
            Globals.startFlaggerLineNum = ln;
            return true;

        }

        /// <summary>
        /// Parses user input as double
        /// </summary>
        /// <returns>Parsed user input</returns>
        private static double GetCoordinateInput() {
            double output;
            while (!double.TryParse(Console.ReadLine(), out output)) {
                Console.Write(Colors.FgRed + "Enter a decimal or integer" + Colors.Reset + "\n> ");
            }
            return output;
        }

        /// <summary>
        /// Prompts user and stores line number inputs as global variables
        /// </summary>
        private static bool GetBaleIndices() {

            Console.Write('\n' + Colors.FgGreen + "Enter the First Line Number Where the Bales Move Up" + Colors.Reset + "\n> ");
            int firstUp = GetLineNumberInput();

            Console.Write('\n' + Colors.FgGreen + "Enter the Last Line Number Where the Bales Move Up" + Colors.Reset + "\n> ");
            int lastUp = GetLineNumberInput();

            if (lastUp <= firstUp) {
                Console.WriteLine(Colors.FgRed + "Error: Last Line Number Less than or Equal to First Line Number" + Colors.Reset);
                return false;
            }

            Console.Write('\n' + Colors.FgGreen + "Enter the First Line Number Where the Bales Move Down" + Colors.Reset + "\n> ");
            int firstDown = GetLineNumberInput();

            // if the inputs of the down bales are between the start and end line numbers of the bale up line numbers restart
            if (firstDown >= firstUp && firstDown <= lastUp) {
                Console.WriteLine(Colors.FgRed + "Error: Inputs Between Bale Up Line Numbers");
                return false;
            }

            Console.Write('\n' + Colors.FgGreen + "Enter the Last Line Number Where the Bales Move Down" + Colors.Reset + "\n> ");
            int lastDown = GetLineNumberInput();

            if (lastDown <= firstDown) {
                Console.WriteLine(Colors.FgRed + "Error: Last Line Number Less than or Equal to First Line Number" + Colors.Reset);
                return false;
            }

            if (lastDown >= firstUp && lastDown <= lastUp) {
                Console.WriteLine(Colors.FgRed + "Error: Inputs Between Bale Up Line Numbers");
                return false;
            }

            Console.WriteLine("\nInputs are as Follows...");
            Console.WriteLine("First Bale Up Line: " + firstUp.ToString());
            Console.WriteLine("Last Bale Up Line: " + lastUp.ToString());
            Console.WriteLine("First Bale Down Line: " + firstDown.ToString());
            Console.WriteLine("Last Bale Down Line: " + lastDown.ToString());

            // If the user does not confirm that these are correct and wants to redo, restart function
            Console.Write(Colors.FgYellow + "Are you sure? y or n" + Colors.Reset + "\n> ");
            bool confirmed = UserAnswer();
            if (!confirmed) {
                return false;
            }

            Globals.balesUpStartLineNum = firstUp;
            Globals.balesUpEndLineNum = lastUp;
            Globals.balesDownStartLineNum = firstDown;
            Globals.balesDownEndLineNum = lastDown;
            return true;
        }

        /// <summary>
        /// Parses user input as int
        /// </summary>
        /// <returns>Parsed user input</returns>
        private static int GetLineNumberInput() {
            int output;
            while (!int.TryParse(Console.ReadLine(), out output)) {
                Console.Write(Colors.FgRed + "Enter a decimal or integer" + Colors.Reset + "\n> ");
            }
            while (output > Globals.numOfLines || output < 1) {
                Console.Write(Colors.FgRed + "Enter a valid line number" + Colors.Reset + "\n> ");
                while (!int.TryParse(Console.ReadLine(), out output)) {
                    Console.Write(Colors.FgRed + "Enter a decimal or integer" + Colors.Reset + "\n> ");
                }
            }
            return output;
        }

    }
}
