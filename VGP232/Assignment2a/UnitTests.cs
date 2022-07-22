using NUnit.Framework;
using System;
using System.IO;

namespace Assignment2a
{
    [TestFixture]
    public class UnitTests
    {
        private WeaponCollection _weaponCollection;
        private string inputPath;
        private string outputPath;

        const string INPUT_FILE = "data2.csv";
        const string OUTPUT_FILE = "output.csv";

        // A helper function to get the directory of where the actual path is.
        private string CombineToAppPath(string filename)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
        }

        [SetUp]
        public void SetUp()
        {
            inputPath = CombineToAppPath(INPUT_FILE);
            outputPath = CombineToAppPath(OUTPUT_FILE);
            _weaponCollection = new WeaponCollection();
        }

        [TearDown]
        public void CleanUp()
        {
            // We remove the output file after we are done.
            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }
        }

        // WeaponCollection Unit Tests
        [Test]
        public void WeaponCollection_GetHighestBaseAttack_HighestValue()
        {
            _weaponCollection.Load(inputPath);
            Assert.AreEqual(48, _weaponCollection.GetHighestBaseAttack());
        }

        [Test]
        public void WeaponCollection_GetLowestBaseAttack_LowestValue()
        {
            _weaponCollection.Load(inputPath);
            Assert.AreEqual(23, _weaponCollection.GetLowestBaseAttack());
        }

        [TestCase(Weapon.WeaponType.Sword, 21)]
        public void WeaponCollection_GetAllWeaponsOfType_ListOfWeapons(Weapon.WeaponType type, int expectedValue)
        {
            Assert.AreEqual(expectedValue, _weaponCollection.GetAllWeaponsOfType(type));
        }

        [TestCase(5, 10)]
        public void WeaponCollection_GetAllWeaponsOfRarity_ListOfWeapons(int stars, int expectedValue)
        {
            // TODO: call WeaponCollection.GetAllWeaponsOfRarity(stars) and confirm that the weapons list returns Count matches the expected value using asserts.
        }

        [Test]
        public void WeaponCollection_LoadThatExistAndValid_True()
        {
            // TODO: load returns true, expect WeaponCollection with count of 95 .
        }

        [Test]
        public void WeaponCollection_LoadThatDoesNotExist_FalseAndEmpty()
        {
            // TODO: load returns false, expect an empty WeaponCollection
        }

        [Test]
        public void WeaponCollection_SaveWithValuesCanLoad_TrueAndNotEmpty()
        {
            // TODO: save returns true, load returns true, and WeaponCollection is not empty.
        }

        [Test]
        public void WeaponCollection_SaveEmpty_TrueAndEmpty()
        {
            // After saving an empty WeaponCollection, load the file and expect WeaponCollection to be empty.
            //WeaponCollection.Clear();
            //Assert.IsTrue(WeaponCollection.Save(outputPath));
            //Assert.IsTrue(WeaponCollection.Load(outputPath));
            //Assert.IsTrue(WeaponCollection.Count == 0);
        }

        // Weapon Unit Tests
        [Test]
        public void Weapon_TryParseValidLine_TruePropertiesSet()
        {
            // TODO: create a Weapon with the stats above set properly
            Weapon expected = null;
            // TODO: uncomment this once you added the Type1 and Type2
            //expected = new Weapon()
            //{
            //    Name = "Skyward Blade",
            //    Type = Weapon.Sword,
            //    Image = "https://vignette.wikia.nocookie.net/gensin-impact/images/0/03/Weapon_Skyward_Blade.png",
            //    Rarity = 5,
            //    BaseAttack = 46,
            //    SeconardStat = "Energy Recharge",
            //    Passive = "Sky-Piercing Fang"
            //};

            string line = "Skyward Blade,Sword,https://vignette.wikia.nocookie.net/gensin-impact/images/0/03/Weapon_Skyward_Blade.png,5,46,Energy Recharge,Sky-Piercing Fang";
            Weapon actual = null;

            // TODO: uncomment this once you have TryParse implemented.
            //Assert.IsTrue(Weapon.TryParse(line, out actual));
            Assert.Equals(expected.Name, actual.Name);
            Assert.Equals(expected.Type, actual.Type);
            Assert.Equals(expected.BaseAttack, actual.BaseAttack);
            // TODO: check for the rest of the properties, Image,Rarity,SecondaryStat,Passive
        }

        [Test]
        public void Weapon_TryParseInvalidLine_FalseNull()
        {
            // TODO: use "1,Bulbasaur,A,B,C,65,65", Weapon.TryParse returns false, and Weapon is null.
        }
    }
}
