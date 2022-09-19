using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using WeaponLib;

namespace Assignment2c
{
    public partial class MainWindow : Window
    {
        WeaponCollection _weaponCollection = new WeaponCollection();
        List<Weapon> helperListWeapons = new List<Weapon>();
        Weapon _selectedWeapon = null;
        EditWeaponWindow _editWeaponWindow = new EditWeaponWindow();
        SortType _currentSortOption = SortType.Name;
        WeaponType _currentTypeSelection = WeaponType.None;

        public MainWindow()
        {
            InitializeComponent();

            _editWeaponWindow.Closing += (object sender, System.ComponentModel.CancelEventArgs e) =>
            {
                e.Cancel = true;
                _editWeaponWindow.Hide();
            };

            var optionNames = Enum.GetNames(typeof(WeaponType));
            foreach (var option in optionNames)
            {
                ListBoxItem lbi = new ListBoxItem();
                lbi.Content = option;
                WeaponTypeBox.Items.Add(lbi);
            }
        }

        private void SaveClicked(object sender, RoutedEventArgs e)
        {
            if (_weaponCollection.Count == 0)
            {
                return;
            }
            
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "NewCollection";
            dlg.DefaultExt = ".csv";
            dlg.Filter = "CSV (.csv)|.csv;|JSON (.json)|.json|XML (.xml)|.xml";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                _weaponCollection.Save(filename);

                if (File.Exists(filename))
                {
                    MessageBox.Show("File saved correctly!");
                }
            }
        }

        private void LoadClicked(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV Files(*.csv)|*.csv|JSON Files(*.json)|*.json|XML Files(*.xml)|*.xml";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (openFileDialog.ShowDialog() == true)
            {
                _weaponCollection.Load(openFileDialog.FileName);
                FillWeaponList();
            }
        }

        void FillWeaponList()
        {
            if (_weaponCollection.Count == 0)
            {
                MessageBox.Show("Your collection is empty! Please revise your file.");
                return;
            }

            if (_currentSortOption != SortType.None)
            {
                _weaponCollection.SortBy(_currentSortOption);
            }

            if (WeaponTypeBox.SelectedIndex == -1)
            {
                WeaponTypeBox.SelectedIndex = 0;
            }

            helperListWeapons = null;
            helperListWeapons = _weaponCollection.GetAllWeaponsOfType(_currentTypeSelection);

            if (String.IsNullOrEmpty(FilterInput.Text) == false)
            {
                List<Weapon> temp = new List<Weapon>();
                foreach (var item in helperListWeapons)
                {
                    if (item.ToString().IndexOf(FilterInput.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        temp.Add(item);
                    }
                }

                helperListWeapons = temp;
            }

            WeaponsViewList.ItemsSource = null;
            WeaponsViewList.Items.Clear();
            WeaponsViewList.ItemsSource = helperListWeapons;
            WeaponsViewList.SelectedIndex = -1;
        }

        public void UpdateWeaponList()
        {
            FillWeaponList();
        }

        public void RegisterNewWeapon(Weapon weapon)
        {
            _weaponCollection.Add(weapon);
            FillWeaponList();
        }

        private void SortRadioSelected(object sender, RoutedEventArgs e)
        {
            var radioButton = sender as RadioButton;

            if (radioButton == null)
            {
                return;
            }

            string name = radioButton.Content.ToString();
            Enum.TryParse(name, out SortType radioType);

            _currentSortOption = radioType;

            FillWeaponList();
        }

        private void OpenWeaponEditWindow(EditWindowMode mode)
        {
            _editWeaponWindow.SetWindowMode(mode, this);
            _editWeaponWindow.Show();
        }

        private void AddClicked(object sender, RoutedEventArgs e)
        {
            OpenWeaponEditWindow(EditWindowMode.Add);
        }

        private void EditClicked(object sender, RoutedEventArgs e)
        {
            OpenWeaponEditWindow(EditWindowMode.Edit);
            _editWeaponWindow.ShowSelectedWeaponData(_selectedWeapon);
        }

        private void RemoveClicked(object sender, RoutedEventArgs e)
        {
            if (helperListWeapons.Count == 0)
            {
                MessageBox.Show("Your collection is empty. Nothing to remove.");
                return;
            }
            else if (WeaponsViewList.SelectedIndex == -1)
            {
                MessageBox.Show("No weapon is currently selected. Please make a selection before removing");
                return;
            }

            _selectedWeapon = helperListWeapons[WeaponsViewList.SelectedIndex];
            helperListWeapons.Remove(_selectedWeapon);
            _weaponCollection.Remove(_selectedWeapon);
            _selectedWeapon = null;

            FillWeaponList();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            FillWeaponList();
        }

        private void WeaponType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_weaponCollection.Count == 0)
            {
                MessageBox.Show("Your collection is empty. Nothing to Sort by! Load a file.");
                WeaponTypeBox.SelectedIndex = -1;
                return;
            }

            _currentTypeSelection = (WeaponType)Enum.Parse(typeof(WeaponType), WeaponTypeBox.SelectedIndex.ToString());

            FillWeaponList();
        }

        private void WeaponsViewList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (helperListWeapons.Count == 0 || WeaponsViewList.SelectedIndex == -1)
            {
                return;
            }

            _selectedWeapon = helperListWeapons[WeaponsViewList.SelectedIndex];
        }
    }
}
