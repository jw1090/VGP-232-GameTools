using System;
using System.Collections.Generic;
using System.IO;

// Assignment 2a
// NAME: Joseph Walden
// STUDENT NUMBER: 2042506

namespace Assignment2a
{
    public enum SortType
    {
        None = -1,
        Name,
        Type,
        Rarity,
        BaseAttack,
    }

    class MainClass
    {
        public static void Main(string[] args)
        {
            // Variables and flags.

            // The path to the input file to load.
            string inputFile = string.Empty;

            // The path of the output file to save.
            string outputFile = string.Empty;

            // The flag to determine if we overwrite the output file or append to it.
            bool appendToFile = false;

            // The flag to determine if we need to display the number of entries.
            bool displayCount = false;

            // The flag to determine if we need to sort the results via name.
            bool sortEnabled = false;

            // The column name to be used to determine which sort comparison function to use.
            SortType sortColumnName = SortType.None;

            // The results to be output to a file or to the console.
            List<Weapon> results = new List<Weapon>();

            for (int i = 0; i < args.Length; i++)
            {
                // h or --help for help to output the instructions on how to use it.
                if (args[i] == "-h" || args[i] == "--help")
                {
                    Console.WriteLine($"-i <path> or --input <path> : loads the input file path specified (required)");
                    Console.WriteLine($"-o <path> or --output <path> : saves result in the output file path specified (optional)");
                    Console.WriteLine($"-c or --count : displays the number of entries in the input file (optional).");
                    Console.WriteLine($"-a or --append : enables append mode when writing to an existing output file (optional)");
                    Console.WriteLine($"-s or --sort <column name> : outputs the results sorted by column name");

                    break;
                }
                else if (args[i] == "-i" || args[i] == "--input")
                {
                    // Check to make sure there's a second argument for the file name.
                    if (args.Length > i + 1)
                    {
                        // Stores the file name in the next argument to inputFile.
                        ++i;
                        inputFile = args[i];

                        if (string.IsNullOrEmpty(inputFile))
                        {
                            Console.WriteLine($"No input file path specified.");
                        }
                        else if (!File.Exists(inputFile))
                        {
                            Console.WriteLine($"The input file path does not exist: {inputFile}");
                        }
                        else
                        {
                            // This function returns a List<Weapon> once the data is parsed.
                            results = Parse(inputFile);
                        }
                    }
                }
                else if (args[i] == "-s" || args[i] == "--sort")
                {
                    if (args.Length > i + 1)
                    {
                        ++i;
                        if (string.IsNullOrEmpty(args[i]))
                        {
                            Console.WriteLine($"A sorting type has not been input.");
                        }
                        else if (Enum.TryParse(args[i], out sortColumnName))
                        {
                            sortEnabled = true;
                        }
                        else
                        {
                            Console.WriteLine($"{args[i]} is invalid.");
                        }
                    }
                }
                else if (args[i] == "-c" || args[i] == "--count")
                {
                    displayCount = true;
                }
                else if (args[i] == "-a" || args[i] == "--append")
                {
                    appendToFile = true;
                }
                else if (args[i] == "-o" || args[i] == "--output")
                {
                    // Validation to make sure we do have an argument after the flag.
                    if (args.Length > i + 1)
                    {
                        // Increment the index.
                        ++i;
                        string filePath = args[i];
                        if (string.IsNullOrEmpty(filePath))
                        {
                            Console.WriteLine($"No input file path specified.");
                        }
                        else
                        {
                            outputFile = filePath;
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"The argument Arg[{i}] = [{args[i]}] is invalid");
                }
            }

            if (sortEnabled)
            {
                Console.WriteLine($"Sorting by {sortColumnName}");

                switch (sortColumnName)
                {
                    case SortType.Name:
                        // Sorts the list based off of the Weapon Name.
                        results.Sort(Weapon.CompareByName);
                        break;
                    case SortType.Type:
                        // Sorts the list based off of the Weapon Type.
                        results.Sort(Weapon.CompareByType);
                        break;
                    case SortType.Rarity:
                        // Sorts the list based off of the Weapon Rarity.
                        results.Sort(Weapon.CompareByRarity);
                        break;
                    case SortType.BaseAttack:
                        // Sorts the list based off of the Weapon Base Attack.
                        results.Sort(Weapon.CompareByBaseAttack);
                        break;
                    default:
                        Console.WriteLine($"{sortColumnName} is invalid. Please change!");
                        break;
                }
            }

            if (displayCount)
            {
                Console.WriteLine($"There are {results.Count} entries");
            }

            if (results.Count > 0)
            {
                if (!string.IsNullOrEmpty(outputFile))
                {
                    FileStream fs;

                    // Check if the append flag is set, and if so, then open the file in append mode; otherwise, create the file to write.
                    if (appendToFile && File.Exists((outputFile)))
                    {
                        fs = File.Open(outputFile, FileMode.Append);
                    }
                    else
                    {
                        fs = File.Open(outputFile, FileMode.Create);
                    }

                    // Opens a stream writer with the file handle to write to the output file.
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        writer.WriteLine($"Name,Type,Rarity,BaseAttack");

                        foreach (Weapon weapon in results)
                        {
                            writer.WriteLine(weapon);
                        }

                        Console.WriteLine($"The file has been saved to {outputFile}");
                    }
                }
                else
                {
                    // Prints out each entry in the weapon list results.
                    foreach (Weapon weapon in results)
                    {
                        Console.WriteLine(weapon);
                    }
                }
            }

            Console.WriteLine("Done!");
        }

        /// <summary>
        /// Reads the file and line by line parses the data into a List of Weapons
        /// </summary>
        /// <param name="fileName">The path to the file</param>
        /// <returns>The list of Weapons</returns>
        public static List<Weapon> Parse(string fileName)
        {
            // streamreader https://msdn.microsoft.com/en-us/library/system.io.streamreader(v=vs.110).aspx
            // Use string split https://msdn.microsoft.com/en-us/library/system.string.split(v=vs.110).aspx

            List<Weapon> output = new List<Weapon>();

            using (StreamReader reader = new StreamReader(fileName))
            {
                // Skip the first line because header does not need to be parsed.
                // Name,Type,Rarity,BaseAttack

                string header = reader.ReadLine();

                // The rest of the lines looks like the following:
                // Skyward Blade,Sword,5,46
                while (reader.Peek() > 0)
                {
                    string line = reader.ReadLine();
                    string[] values = line.Split(',');

                    if (values.Length < 4)
                    {
                        Console.WriteLine($"Failed to add weapon - Not enough arguments [{values.Length}] to create a new weapon.");
                        continue;
                    }
                    else if (values.Length > 4)
                    {
                        Console.WriteLine($"Failed to add weapon - Too many arguments [{values.Length}] to create a new weapon.");
                        continue;
                    }

                    // Populate the properties of the Weapon
                    Weapon weapon = new Weapon();

                    weapon.Name = values[0];
                    weapon.Name = char.ToUpper(weapon.Name[0]) + weapon.Name.Substring(1); // Capitalize first letters of the Name.

                    weapon.Type = values[1];
                    weapon.Type = char.ToUpper(weapon.Type[0]) + weapon.Type.Substring(1); // Capitalize first letters of the Type.

                    if (int.TryParse(values[2], out int rarity))
                    {
                        weapon.Rarity = rarity;
                    }
                    else
                    {
                        Console.WriteLine($"Failed to add weapon - Rarity was not a valid entry [{rarity}].");
                        continue;
                    }

                    if (int.TryParse(values[3], out int baseAttack))
                    {
                        weapon.BaseAttack = baseAttack;
                    }
                    else
                    {
                        Console.WriteLine($"Failed to add weapon - Base Attack was not a valid entry [{rarity}].");
                        continue;
                    }

                    output.Add(weapon); // Add the Weapon to the list
                    Console.WriteLine($"Weapon added - {weapon.Name}");
                }
            }

            return output;
        }
    }
}