using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Assignment2b
{
    class WeaponCollection : List<Weapon>, IPeristence, ICsvSerializable, IJsonSerializable, IXmlSerializable
    {
        private FileStream fileStream;

        public bool AppendToFile { get; set; }

        // Looks at the file extension to determine what type of loading it should do.
        public bool Load(string path) 
        {
            if (string.IsNullOrEmpty(path))
            {
                Console.WriteLine($"No input file path specified.");
                return false;
            }

            if (File.Exists(path) == false)
            {
                Console.WriteLine($"The input file path does not exist: {path}");
                return false;
            }

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

        // Looks at the file extension to determine what type of saving it should do.
        public bool Save(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                Console.WriteLine($"File path [{path}] is null or empty.");
                return false;
            }

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

        private void OpenFileStream(string path)
        {
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
        }

        public bool LoadCSV(string path)
        {
            Clear();

            using (StreamReader reader = new StreamReader(path))
            {
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

            return true;
        }

        public bool SaveAsCSV(string path)
        {
            OpenFileStream(path);

            using (StreamWriter writer = new StreamWriter(fileStream))
            {
                writer.WriteLine($"Name,Type,Image,Rarity,BaseAttack,SecondaryStat,Passive");

                foreach (Weapon weapon in this)
                {
                    writer.WriteLine(weapon);
                }

                Console.WriteLine($"The CSV file has been saved to {path}");
            }

            fileStream.Close();

            return true;
        }

        public bool LoadJSON(string path)
        {
            Clear();

            using (StreamReader reader = new StreamReader(path))
            {
                string jsonText = reader.ReadToEnd();

                WeaponCollectionData weaponCollectionData = JsonConvert.DeserializeObject<WeaponCollectionData>(jsonText);

                foreach (Weapon weapon in weaponCollectionData.Weapons)
                {
                    Add(weapon);
                }
            }

            return true;
        }

        public bool SaveAsJSON(string path)
        {
            OpenFileStream(path);

            WeaponCollectionData weaponCollectionData = new WeaponCollectionData();

            foreach (Weapon weapon in this)
            {
                weaponCollectionData.Weapons.Add(weapon);
            }

            using (StreamWriter writer = new StreamWriter(fileStream))
            {
                string jsonString = JsonConvert.SerializeObject(weaponCollectionData, Formatting.Indented);

                writer.WriteLine(jsonString);
            }
            fileStream.Close();

            Console.WriteLine($"The JSON file has been saved to {path}");


            return true;
        }

        public bool LoadXML(string path)
        {
            Clear();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(WeaponCollectionData));

            using (StreamReader streamReader = new StreamReader(path))
            {
                string xmlText = streamReader.ReadToEnd();

                using (TextReader textReader = new StringReader(xmlText))
                {
                    WeaponCollectionData weaponCollectionData = (WeaponCollectionData)xmlSerializer.Deserialize(textReader);

                    foreach (Weapon weapon in weaponCollectionData.Weapons)
                    {
                        Add(weapon);
                    }
                }
            }

            return true;
        }

        public bool SaveAsXML(string path)
        {
            OpenFileStream(path);

            WeaponCollectionData weaponCollectionData = new WeaponCollectionData();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(WeaponCollectionData));

            foreach (Weapon weapon in this)
            {
                weaponCollectionData.Weapons.Add(weapon);
            }

            var stringBuilder = new StringBuilder();
            using (TextWriter writer = new StringWriter(stringBuilder))
            {
                xmlSerializer.Serialize(writer, weaponCollectionData);
            }

            using (StreamWriter streamWriter = new StreamWriter(fileStream))
            {
                streamWriter.WriteLine(stringBuilder.ToString());
            }
            fileStream.Close();

            Console.WriteLine($"The XML file has been saved to {path}");

            return true;
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