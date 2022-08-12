using NUnit.Framework;
using System;
using System.IO;

namespace Assignment2b
{
    [TestFixture]
    public class UnitTests
    {
        private WeaponCollection _weaponCollection;
        private string _inputPathCSV;
        private string _inputPathJSON;
        private string _inputPathXML;
        private string _outputPathCSV;
        private string _outputPathJSON;
        private string _outputPathXML;

        const string INPUT_FILE_CSV = "data2.csv";
        const string INPUT_FILE_JSON = "data2.json";
        const string INPUT_FILE_XML = "data2.xml";

        const string OUTPUT_FILE_CSV = "weapons.csv";
        const string OUTPUT_FILE_JSON = "weapons.json";
        const string OUTPUT_FILE_XML = "weapons.xml";

        // A helper function to get the directory of where the actual path is.
        private string CombineToAppPath(string filename)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
        }

        [SetUp]
        public void SetUp()
        {
            _inputPathCSV = CombineToAppPath(INPUT_FILE_CSV);
            _inputPathJSON = CombineToAppPath(INPUT_FILE_JSON);
            _inputPathXML = CombineToAppPath(INPUT_FILE_XML);

            _outputPathCSV = CombineToAppPath(OUTPUT_FILE_CSV);
            _outputPathJSON = CombineToAppPath(OUTPUT_FILE_JSON);
            _outputPathXML = CombineToAppPath(OUTPUT_FILE_XML);

            _weaponCollection = new WeaponCollection();
        }

        [TearDown]
        public void CleanUp()
        {
            // We remove the output file after we are done.
            if (File.Exists(_outputPathCSV))
            {
                File.Delete(_outputPathCSV);
            }
        }

        // WeaponCollection Unit Tests
        [Test]
        public void WeaponCollection_GetHighestBaseAttack_HighestValue()
        {
            Assert.IsTrue(_weaponCollection.Load(_inputPathCSV));
            Assert.AreEqual(48, _weaponCollection.GetHighestBaseAttack());
        }

        [Test]
        public void WeaponCollection_GetLowestBaseAttack_LowestValue()
        {
            Assert.IsTrue(_weaponCollection.Load(_inputPathCSV));
            Assert.AreEqual(23, _weaponCollection.GetLowestBaseAttack());
        }

        [TestCase(Weapon.WeaponType.Sword, 21)]
        public void WeaponCollection_GetAllWeaponsOfType_ListOfWeapons(Weapon.WeaponType type, int expectedValue)
        {
            Assert.IsTrue(_weaponCollection.Load(_inputPathCSV));
            Assert.AreEqual(expectedValue, _weaponCollection.GetAllWeaponsOfType(type).Count);
        }

        [TestCase(5, 10)]
        public void WeaponCollection_GetAllWeaponsOfRarity_ListOfWeapons(int stars, int expectedValue)
        {
            Assert.IsTrue(_weaponCollection.Load(_inputPathCSV));
            Assert.AreEqual(10, _weaponCollection.GetAllWeaponsOfRarity(stars).Count);
        }

        [Test]
        public void WeaponCollection_LoadThatExistAndValid_True()
        {
            Assert.IsTrue(_weaponCollection.Load(_inputPathCSV));
            Assert.AreEqual(95, _weaponCollection.Count);
        }

        [Test]
        public void WeaponCollection_LoadThatDoesNotExist_FalseAndEmpty()
        {
            _weaponCollection.Clear();
            Assert.IsFalse(_weaponCollection.Load(""));
            _weaponCollection.Load(_inputPathCSV);
        }

        [Test]
        public void WeaponCollection_SaveWithValuesCanLoad_TrueAndNotEmpty()
        {
            Assert.IsTrue(_weaponCollection.Load(_inputPathCSV));
            Assert.IsTrue(_weaponCollection.Save(_outputPathCSV));
            Assert.IsTrue(_weaponCollection.Count != 0);
        }

        [Test]
        public void WeaponCollection_SaveEmpty_TrueAndEmpty()
        {
            _weaponCollection.Clear();
            Assert.IsTrue(_weaponCollection.Save(_outputPathCSV));
            Assert.IsTrue(_weaponCollection.Load(_outputPathCSV));
            Assert.IsTrue(_weaponCollection.Count == 0);
        }

        // Weapon Collection Test Load JSON
        [Test]
        public void WeaponCollection_Load_Save_Load_ValidJson()
        {
            Assert.IsTrue(_weaponCollection.Load(_inputPathCSV));
            Assert.IsTrue(_weaponCollection.Save(_outputPathJSON));
            Assert.IsTrue(_weaponCollection.Load(_inputPathJSON));
            Assert.AreEqual(95, _weaponCollection.Count);
        }

        [Test]
        public void WeaponCollection_Load_SaveAsJSON_Load_ValidJson()
        {
            Assert.IsTrue(_weaponCollection.Load(_inputPathCSV));
            Assert.IsTrue(_weaponCollection.SaveAsJSON(_outputPathJSON));
            Assert.IsTrue(_weaponCollection.Load(_inputPathJSON));
            Assert.AreEqual(95, _weaponCollection.Count);
        }

        [Test]
        public void WeaponCollection_Load_SaveAsJSON_LoadJSON_ValidJson()
        {
            Assert.IsTrue(_weaponCollection.Load(_inputPathCSV));
            Assert.IsTrue(_weaponCollection.SaveAsJSON(_outputPathJSON));
            Assert.IsTrue(_weaponCollection.LoadJSON(_inputPathJSON));
            Assert.AreEqual(95, _weaponCollection.Count);
        }

        [Test]
        public void WeaponCollection_Load_Save_LoadJSON_ValidJson()
        {
            Assert.IsTrue(_weaponCollection.Load(_inputPathCSV));
            Assert.IsTrue(_weaponCollection.Save(_outputPathJSON));
            Assert.IsTrue(_weaponCollection.LoadJSON(_inputPathJSON));
            Assert.AreEqual(95, _weaponCollection.Count);
        }

        // Weapon Unit Tests
        [Test]
        public void Weapon_TryParseValidLine_TruePropertiesSet()
        {
            Weapon expected = null;
            expected = new Weapon()
            {
                Name = "Skyward Blade",
                Type = Weapon.WeaponType.Sword,
                Image = "https://vignette.wikia.nocookie.net/gensin-impact/images/0/03/Weapon_Skyward_Blade.png",
                Rarity = 5,
                BaseAttack = 46,
                SecondaryStat = "Energy Recharge",
                Passive = "Sky-Piercing Fang"
            };

            string line = "Skyward Blade,Sword,https://vignette.wikia.nocookie.net/gensin-impact/images/0/03/Weapon_Skyward_Blade.png,5,46,Energy Recharge,Sky-Piercing Fang";
            Weapon actual = null;

            Assert.IsTrue(Weapon.TryParse(line, out actual));
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Type, actual.Type);
            Assert.AreEqual(expected.Image, actual.Image);
            Assert.AreEqual(expected.Rarity, actual.Rarity);
            Assert.AreEqual(expected.BaseAttack, actual.BaseAttack);
            Assert.AreEqual(expected.SecondaryStat, actual.SecondaryStat);
            Assert.AreEqual(expected.Passive, actual.Passive);
        }

        [Test]
        public void Weapon_TryParseInvalidLine_FalseNull()
        {
            string test = "1,Bulbasaur,A,B,C,65,65";

            Assert.IsFalse(Weapon.TryParse(test, out Weapon result));
            Assert.IsTrue(result == null);
        }
    }
}