using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Assignment2a
{
    class WeaponCollection : List<Weapon>, IPeristence
    {
        public bool Load(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                Console.WriteLine($"No input file path specified.");

                return false;
            }
            else if (File.Exists(fileName) == false)
            {
                Console.WriteLine($"The input file path does not exist: {fileName}");

                return false;
            }
            else // Parse data to create a new WeaponCollection.
            {
                ParseWeaponData(fileName);

                return true;
            }
        }

        public bool Save(string filename, bool appendToFile = false)
        {
            if(string.IsNullOrEmpty(filename))
            {
                return false;
            }

            FileStream fs;

            // Check if the append flag is set, and if so, then open the file in append mode.
            // Otherwise, create the file to write.
            if (appendToFile && File.Exists(filename))
            {
                fs = File.Open(filename, FileMode.Append);
            }
            else
            {
                fs = File.Open(filename, FileMode.Create);
            }

            // Opens a stream writer with the file handle to write to the output file.
            using (StreamWriter writer = new StreamWriter(fs))
            {
                writer.WriteLine($"Name,Type,Image,Rarity,BaseAttack,SecondaryStat,Passive");

                foreach (Weapon weapon in this)
                {
                    writer.WriteLine(weapon);
                }

                Console.WriteLine($"The file has been saved to {filename}");
            }

            return true;
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