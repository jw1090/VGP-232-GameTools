using System;

// Assignment 2b
// NAME: Joseph Walden
// STUDENT NUMBER: 2042506

namespace Assignment2b
{
    public enum SortType
    {
        None = -1,
        Name,
        Type,
        Image,
        Rarity,
        BaseAttack,
        SecondaryStat,
        Passive,
    }

    class MainClass
    {
        public static void Main(string[] args)
        {
            WeaponCollection results = new WeaponCollection(); // The results to be output to a file or to the console.
            SortType sortColumnName = SortType.None;
            bool appendToFile = false; // Should we append the file (true), or overwrite (false)?
            bool displayCount = false; // The flag to determine if we display the number of entries.
            bool sortEnabled = false; // The flag to determine if we need to sort the results via name.
            string outputFile = string.Empty; // The path of the output file to save.

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
                        string inputFile = args[i];

                        if (results.Load(inputFile) == false)
                        {
                            Console.WriteLine($"Load Failed - File Path Invalid: {inputFile}");
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

                results.SortBy(sortColumnName);
            }

            if (displayCount == true)
            {
                Console.WriteLine($"There are {results.Count} entries");
            }

            if (results.Count > 0)
            {
                results.AppendToFile = appendToFile;

                if (results.Save(outputFile) == false) // Could not save.
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
    }
}