using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Assignment2b
{
    class WeaponCollection : List<Weapon>, IPeristence, ICsvSerializable, IJsonSerializable, IXmlSerializable
    {
        private FileStream fileStream;
        public bool AppendToFile { get; set; }

        public bool Load(string path)
        {
            string extention =  Path.GetExtension(path);

            switch (extention)
            {
                case ".csv":
                    return LoadCSV(path);
                case ".json":
                    return LoadJSON(path);
                case ".xml":
                    return LoadXML(path);
                default:
                    Console.WriteLine($"File load path [{path}] is not valid, please use .csv, .json, or .xml!");
                    return false;
            }
        }

        public bool Save(string path)
        {
            string extention = Path.GetExtension(path);

            switch (extention)
            {
                case ".csv":
                    return SaveAsCSV(path);
                case ".json":
                    return SaveAsJSON(path);
                case ".xml":
                    return SaveAsXML(path);
                default:
                    Console.WriteLine($"File save path [{path}] is not valid, please use .csv, .json, or .xml!");
                    return false;
            }
        }

        public bool LoadCSV(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                Console.WriteLine($"No input file path specified.");

                return false;
            }
            else if (File.Exists(path) == false)
            {
                Console.WriteLine($"The input file path does not exist: {path}");

                return false;
            }
            else // Parse data to create a new WeaponCollection.
            {
                ParseWeaponData(path);

                return true;
            }
        }

        public bool SaveAsCSV(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }

            // Check if the append flag is set, and if so, then open the file in append mode.
            // Otherwise, create the file to write.
            if (AppendToFile && File.Exists(path))
            {
                fileStream = File.Open(path, FileMode.Append);
            }
            else
            {
                fileStream = File.Open(path, FileMode.Create);
            }

            // Opens a stream writer with the file handle to write to the output file.
            using (StreamWriter writer = new StreamWriter(fileStream))
            {
                writer.WriteLine($"Name,Type,Image,Rarity,BaseAttack,SecondaryStat,Passive");

                foreach (Weapon weapon in this)
                {
                    writer.WriteLine(weapon);
                }

                Console.WriteLine($"The file has been saved to {path}");
            }

            fileStream.Close();

            return true;
        }

        public bool LoadJSON(string path)
        {
            throw new NotImplementedException();
        }

        public bool SaveAsJSON(string path)
        {
            throw new NotImplementedException();
        }

        public bool LoadXML(string path)
        {
            throw new NotImplementedException();
        }

        public bool SaveAsXML(string path)
        {
            throw new NotImplementedException();
        }

        private void ParseWeaponData(string fileName)
        {
            using (StreamReader reader = new StreamReader(fileName))
            {
                Clear(); // Reset for new data

                // Skip the first line because header does not need to be parsed.
                string header = reader.ReadLine();

                // (Name,Type,Image,Rarity,BaseAttack,SecondaryStats,Passives)
                while (reader.Peek() > 0)
                {
                    string line = reader.ReadLine();

                    if (Weapon.TryParse(line, out Weapon weapon))
                    {
                        Add(weapon);
                    }
                }
            }
        }

        public int GetHighestBaseAttack()
        {
            return this.Max(weapon => weapon.BaseAttack);
        }

        public int GetLowestBaseAttack()
        {
            return this.Min(weapon => weapon.BaseAttack);
        }

        public List<Weapon> GetAllWeaponsOfType(Weapon.WeaponType weaponType)
        {
            return FindAll(weapon => weapon.Type == weaponType);
        }

        public List<Weapon> GetAllWeaponsOfRarity(int stars)
        {
            return FindAll(weapon => weapon.Rarity == stars);
        }

        public void SortBy(SortType columnName)
        {
            switch (columnName)
            {
                case SortType.Name:
                    Sort(Weapon.CompareByName);
                    break;
                case SortType.Type:
                    Sort(Weapon.CompareByType);
                    break;
                case SortType.Image:
                    Sort(Weapon.CompareByImage);
                    break;
                case SortType.Rarity:
                    Sort(Weapon.CompareByRarity);
                    break;
                case SortType.BaseAttack:
                    Sort(Weapon.CompareByBaseAttack);
                    break;
                case SortType.SecondaryStat:
                    Sort(Weapon.CompareBySecondaryStat);
                    break;
                case SortType.Passive:
                    Sort(Weapon.CompareByPassive);
                    break;
                default:
                    Console.WriteLine($"{columnName} is invalid. Please change!");
                    break;
            }
        }
    }
}