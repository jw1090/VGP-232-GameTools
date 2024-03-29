﻿using NUnit.Framework;
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
            if (File.Exists(_outputPathJSON))
            {
                File.Delete(_outputPathJSON);
            }
            if (File.Exists(_outputPathXML))
            {
                File.Delete(_outputPathXML);
            }

            string emptyCSV = CombineToAppPath("empty.csv");
            string emptyJSON = CombineToAppPath("empty.json");
            string emptyXML = CombineToAppPath("empty.xml");
            if (File.Exists(emptyCSV))
            {
                File.Delete(emptyCSV);
            }
            if (File.Exists(emptyJSON))
            {
                File.Delete(emptyJSON);
            }
            if (File.Exists(emptyXML))
            {
                File.Delete(emptyXML);
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

        // Weapon Collection Load JSON Tests
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

        // Weapon Collection Load CSV Tests
        [Test]
        public void WeaponCollection_Load_Save_Load_ValidCsv()
        {
            Assert.IsTrue(_weaponCollection.Load(_inputPathCSV));
            Assert.IsTrue(_weaponCollection.Save(_outputPathCSV));
            Assert.IsTrue(_weaponCollection.Load(_inputPathCSV));
            Assert.AreEqual(95, _weaponCollection.Count);
        }

        [Test]
        public void WeaponCollection_Load_SaveAsCSV_LoadCSV_ValidCsv()
        {
            Assert.IsTrue(_weaponCollection.Load(_inputPathCSV));
            Assert.IsTrue(_weaponCollection.SaveAsCSV(_outputPathCSV));
            Assert.IsTrue(_weaponCollection.LoadCSV(_inputPathCSV));
            Assert.AreEqual(95, _weaponCollection.Count);
        }

        // Weapon Collection Load XML Tests
        [Test]
        public void WeaponCollection_Load_Save_Load_ValidXml()
        {
            Assert.IsTrue(_weaponCollection.Load(_inputPathCSV));
            Assert.IsTrue(_weaponCollection.Save(_outputPathXML));
            Assert.IsTrue(_weaponCollection.Load(_inputPathXML));
            Assert.AreEqual(95, _weaponCollection.Count);
        }

        [Test]
        public void WeaponCollection_Load_SaveAsXML_LoadXML_ValidXml()
        {
            Assert.IsTrue(_weaponCollection.Load(_inputPathCSV));
            Assert.IsTrue(_weaponCollection.SaveAsXML(_outputPathXML));
            Assert.IsTrue(_weaponCollection.LoadXML(_inputPathXML));
            Assert.AreEqual(95, _weaponCollection.Count);
        }

        // Weapon Collection Empty Save Tests
        [Test]
        public void WeaponCollection_SaveEmpty_Load_ValidJson()
        {
           WeaponCollection emptyCollection = new WeaponCollection();
           string emptyPath = CombineToAppPath("empty.json");

            Assert.IsTrue(emptyCollection.SaveAsJSON(emptyPath));
            Assert.IsTrue(emptyCollection.Load(emptyPath));
            Assert.AreEqual(0, emptyCollection.Count);
        }

        [Test]
        public void WeaponCollection_SaveEmpty_Load_ValidCsv()
        {
            WeaponCollection emptyCollection = new WeaponCollection();
            string emptyPath = CombineToAppPath("empty.csv");

            Assert.IsTrue(emptyCollection.SaveAsCSV(emptyPath));
            Assert.IsTrue(emptyCollection.Load(emptyPath));
            Assert.AreEqual(0, emptyCollection.Count);
        }

        [Test]
        public void WeaponCollection_SaveEmpty_Load_ValidXml()
        {
            WeaponCollection emptyCollection = new WeaponCollection();
            string emptyPath = CombineToAppPath("empty.xml");

            Assert.IsTrue(emptyCollection.SaveAsXML(emptyPath));
            Assert.IsTrue(emptyCollection.Load(emptyPath));
            Assert.AreEqual(0, emptyCollection.Count);
        }

        // Weapon Collection Invalid Format Tests
        [Test]
        public void WeaponCollection_Load_SaveJSON_LoadXML_InvalidXml()
        {
            Assert.IsTrue(_weaponCollection.Load(_inputPathCSV));
            Assert.IsTrue(_weaponCollection.SaveAsJSON(_outputPathJSON));
            Assert.IsFalse(_weaponCollection.LoadXML(_outputPathJSON));
            Assert.AreEqual(0, _weaponCollection.Count);
        }

        public void WeaponCollection_Load_SaveXML_LoadJSON_InvalidJson()
        {
            Assert.IsTrue(_weaponCollection.Load(_inputPathCSV));
            Assert.IsTrue(_weaponCollection.SaveAsXML(_outputPathXML));
            Assert.IsFalse(_weaponCollection.LoadJSON(_outputPathXML));
            Assert.AreEqual(0, _weaponCollection.Count);
        }

        public void WeaponCollection_ValidCsv_LoadXML_InvalidXml()
        {
            Assert.IsTrue(_weaponCollection.LoadXML(_inputPathCSV));
            Assert.AreEqual(0, _weaponCollection.Count);
        }

        public void WeaponCollection_ValidCsv_LoadJSON_InvalidJson()
        {
            Assert.IsTrue(_weaponCollection.LoadJSON(_inputPathCSV));
            Assert.AreEqual(0, _weaponCollection.Count);
        }

        // Weapon Tests
        [Test]
        public void Weapon_TryParseValidLine_TruePropertiesSet()
        {
            Weapon expected = new Weapon()
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

            Assert.IsTrue(Weapon.TryParse(line, out Weapon actual));
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