using System;
using System.Windows;
using WeaponLib;

namespace Assignment2c
{
    public partial class EditWeaponWindow : Window
    {
        EditWindowMode modeSelected = EditWindowMode.None;
        private Weapon _selectedWeapon = null;
        private Weapon _newWeapon = null;
        private MainWindow _owner = null;
        const int MAX_RARITY = 5;

        public EditWeaponWindow()
        {
            InitializeComponent();
            Owner = null;

            _newWeapon = new Weapon();

            for (int i = 0; i < MAX_RARITY; i++)
            {
                RarityBox.Items.Add(i + 1);
            }

            string[] weaponTypes = Enum.GetNames(typeof(WeaponType));
            foreach (string weaponType in weaponTypes)
            {
                if (weaponType == "All")
                {
                    continue;
                }

                TypeBox.Items.Add(weaponType);
            }
        }

        public void SetWindowMode(EditWindowMode mode, MainWindow owner)
        {
            modeSelected = mode;

            switch (modeSelected)
            {
                case EditWindowMode.None:
                    break;
                case EditWindowMode.Add:
                    this.Title = "Add Weapon";
                    SaveButton.Content = "Add";
                    break;
                case EditWindowMode.Edit:
                    this.Title = "Edit Weapon";
                    SaveButton.Content = "Save";
                    break;
                default:
                    Console.WriteLine($"Mode selected [{modeSelected}] is invalid! Please fix!");
                    break;
            }

            this.Owner = owner;
            _owner = owner;

            ResetWindow();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateWeapons();

            switch (modeSelected)
            {
                case EditWindowMode.None:
                    break;
                case EditWindowMode.Add:
                    _owner.RegisterNewWeapon(_newWeapon);
                    break;
                case EditWindowMode.Edit:
                    _owner.UpdateWeaponList();
                    break;
                default:
                    Console.WriteLine($"Mode selected [{modeSelected}] is invalid! Please fix!");
                    break;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            Random rand = new Random();

            TypeBox.SelectedIndex = rand.Next(1, TypeBox.Items.Count);
            RarityBox.SelectedIndex = rand.Next(0, RarityBox.Items.Count);
            BaseAttackBox.Text = rand.Next(15, 80).ToString();
        }

        private void ResetWindow()
        {
            _selectedWeapon = null;

            NameTextBox.Text = String.Empty;
            URLTextBox.Text = String.Empty;
            BaseAttackBox.Text = String.Empty;
            SecondaryStatBox.Text = String.Empty;
            PassiveBox.Text = String.Empty;
            RarityBox.SelectedIndex = -1;
            TypeBox.SelectedIndex = -1;
        }

        public void ShowSelectedWeaponData(Weapon weapon)
        {
            _selectedWeapon = weapon;

            NameTextBox.Text = weapon.Name;
            URLTextBox.Text = weapon.Image;
            BaseAttackBox.Text = weapon.BaseAttack.ToString();
            SecondaryStatBox.Text = weapon.SecondaryStat;
            PassiveBox.Text = weapon.Passive;
            RarityBox.SelectedIndex = weapon.Rarity - 1;
            TypeBox.SelectedIndex = (int)weapon.Type - 1;
        }

        private void UpdateWeapons()
        {
            int baseAttack = 0;

            switch (modeSelected)
            {
                case EditWindowMode.None:
                    break;
                case EditWindowMode.Add:
                    _newWeapon.Passive = PassiveBox.Text;
                    _newWeapon.SecondaryStat = SecondaryStatBox.Text;

                    if (int.TryParse(BaseAttackBox.Text, out baseAttack))
                    {
                        _newWeapon.BaseAttack = baseAttack;
                    }

                    _newWeapon.Image = URLTextBox.Text;

                    if (TypeBox.SelectedIndex == -1 || TypeBox.SelectedIndex == 0)
                    {
                        _newWeapon.Type = WeaponType.None;
                    }
                    else
                    {
                        var type = (WeaponType)TypeBox.SelectedIndex + 1;
                        _newWeapon.Type = type;
                    }

                    _newWeapon.Name = NameTextBox.Text;
                    _newWeapon.Rarity = RarityBox.SelectedIndex + 1;
                    break;
                case EditWindowMode.Edit:
                    _selectedWeapon.Passive = PassiveBox.Text;
                    _selectedWeapon.SecondaryStat = SecondaryStatBox.Text;

                    int.TryParse(BaseAttackBox.Text, out baseAttack);
                    _selectedWeapon.BaseAttack = baseAttack;

                    _selectedWeapon.Image = URLTextBox.Text;

                    if (TypeBox.SelectedIndex == -1 || TypeBox.SelectedIndex == 0)
                    {
                        _selectedWeapon.Type = WeaponType.None;
                    }
                    else
                    {
                        var type = (WeaponType)TypeBox.SelectedIndex + 1;
                        _selectedWeapon.Type = type;
                    }

                    _selectedWeapon.Name = NameTextBox.Text;
                    _selectedWeapon.Rarity = RarityBox.SelectedIndex + 1;
                    break;
                default:
                    Console.WriteLine($"Mode selected [{modeSelected}] is invalid! Please fix!");
                    break;
            }
        }
    }
}