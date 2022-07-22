using System;

namespace Assignment2a
{
    public class Weapon
    {
        public enum WeaponType
        {
            None = -1,
            Sword,
            Polearm,
            Claymore,
            Catalyst,
            Bow,
        }

        public string Name { get; set; } = string.Empty;
        public WeaponType Type { get; set; } = WeaponType.None;
        public string Image { get; set; } = string.Empty;
        public int Rarity { get; set; } = 0;
        public int BaseAttack { get; set; } = 0;
        public string SecondaryStat { get; set; } = string.Empty;
        public string Passive { get; set; } = string.Empty;

        /// <summary>
        /// The Comparator function to check for name
        /// </summary>
        /// <param name="left">Left side Weapon</param>
        /// <param name="right">Right side Weapon</param>
        /// <returns> -1 (or any other negative value) for "less than", 0 for "equals", or 1 (or any other positive value) for "greater than"</returns>
        public static int CompareByName(Weapon left, Weapon right)
        {
            return left.Name.CompareTo(right.Name);
        }

        public static int CompareByType(Weapon left, Weapon right)
        {
            return left.Type.CompareTo(right.Type);
        }

        public static int CompareByImage(Weapon left, Weapon right)
        {
            return left.Image.CompareTo(right.Image);
        }

        public static int CompareByRarity(Weapon left, Weapon right)
        {
            return left.Rarity.CompareTo(right.Rarity);
        }

        public static int CompareByBaseAttack(Weapon left, Weapon right)
        {
            return left.BaseAttack.CompareTo(right.BaseAttack);
        }
        public static int CompareBySecondaryStat(Weapon left, Weapon right)
        {
            return left.SecondaryStat.CompareTo(right.SecondaryStat);
        }
        public static int CompareByPassive(Weapon left, Weapon right)
        {
            return left.Passive.CompareTo(right.Passive);
        }

        public static bool TryParse(string rawData, out Weapon weapon)
        {
            weapon = new Weapon();

            string[] values = rawData.Split(',');

            // Validate the length of values passed by raw data.
            if (values.Length < 7)
            {
                Console.WriteLine($"Failed to add weapon - Not enough arguments [{values.Length}] to create a new weapon.");

                weapon = null;
                return false;
            }
            else if (values.Length > 7)
            {
                Console.WriteLine($"Failed to create weapon - Too many arguments [{values.Length}] to create a new weapon.");

                weapon = null;
                return false;
            }

            // Populate Name
            weapon.Name = values[0];
            weapon.Name = char.ToUpper(weapon.Name[0]) + weapon.Name.Substring(1); // Capitalize first letters of the Name.

            // Populate Type
            if (Enum.TryParse(values[1], out WeaponType result))
            {
                if (result == WeaponType.None)
                {
                    Console.WriteLine($"Failed to create weapon - Type: [{result}] is invalid.");

                    weapon = null;
                    return false;
                }

                weapon.Type = result;
            }
            else
            {
                Console.WriteLine($"Failed to add weapon - [{values[1]}] is not an acceptable Weapon Type.");

                weapon = null;
                return false;
            }

            // Populate Image
            weapon.Image = values[2];

            // Populate Rarity
            if (int.TryParse(values[3], out int rarity))
            {
                weapon.Rarity = rarity;
            }
            else
            {
                Console.WriteLine($"Failed to create weapon - [{values[3]}] is not an acceptable Rarity.");

                weapon = null;
                return false;
            }

            // Populate Base Attack
            if (int.TryParse(values[4], out int baseAttack))
            {
                weapon.BaseAttack = baseAttack;
            }
            else
            {
                Console.WriteLine($"Failed to create weapon - [{values[4]}] is not an acceptable Base Attack.");

                weapon = null;
                return false;
            }

            // Populate Secondary Stat
            weapon.SecondaryStat = values[5];
            weapon.SecondaryStat = char.ToUpper(weapon.SecondaryStat[0]) + weapon.SecondaryStat.Substring(1); // Capitalize first letters of the Secondary Stat.

            // Populate Passive
            weapon.Passive = values[6];
            weapon.Passive = char.ToUpper(weapon.Passive[0]) + weapon.Passive.Substring(1); // Capitalize first letters of the Passive.

            return true;
        }

        /// <summary>
        /// The Weapon string with all the properties
        /// </summary>
        /// <returns>The Weapon formated string</returns>
        public override string ToString()
        {
            return $"{Name},{Type},{Image},{Rarity},{BaseAttack},{SecondaryStat},{Passive}";
        }
    }
}