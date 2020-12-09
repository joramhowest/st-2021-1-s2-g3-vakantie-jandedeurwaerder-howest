using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Pra.Vakantieverhuur.CORE.Entities;
using Pra.Vakantieverhuur.CORE.Services;

namespace Pra.Vakantieverhuur.WPF
{
    /// <summary>
    /// Interaction logic for WinResidences.xaml
    /// </summary>
    /// 
    

    public partial class WinResidences : Window
    {
        public enum ReasonForEntry { viewResidence, newResidence, editResidence }

        #region Global variables
        public ReasonForEntry reasonForEntry;
        public Residence selectedResidence = null;
        public bool refreshRequired = false;

        private Residences residences = new Residences();
        #endregion

        public WinResidences()
        {
            InitializeComponent();
        }

        #region Event-handlers
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (reasonForEntry == ReasonForEntry.viewResidence)
            {
                DisplayView();
            }
            else if(reasonForEntry == ReasonForEntry.newResidence)
            {
                DisplayNew();
            }
            else
            {
                DisplayEdit();
            }
            if (selectedResidence == null)
            {
                ClearControls();
            }
            else
            {
                PopulateControls();
                cmbKindOfResidence.IsEnabled = false;
            }
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            PrepareBeforeSave();

            // read all content
            string houseName = txtHouseName.Text.Trim();
            string streetAndNumber = txtStreetAndNumber.Text.Trim();
            string postalCode = txtPastalCode.Text.Trim();
            string town = txtTown.Text.Trim();
            decimal.TryParse(txtBasePrice.Text, out decimal basePrice);
            decimal.TryParse(txtReducedPrice.Text, out decimal reducedPrice);
            byte.TryParse(txtDaysForReduction.Text, out byte daysForReduction);
            decimal.TryParse(txtGuarantee.Text, out decimal deposit);
            int.TryParse(txtMaxPersons.Text, out int maxPersons);
            bool rentable = (bool)chkRentable.IsChecked;
            bool? microwave = chkMicrowave.IsChecked;
            bool? tv = chkTV.IsChecked;
            bool? privateSanitaryBlock = chkPrivateSanitaryBlock.IsChecked;
            bool? dishWasher = chkDishwasher.IsChecked;
            bool? washingMachine = chkWashingMachine.IsChecked;
            bool? woodStove = chkWoodStove.IsChecked;

            // check the content
            bool errors = false;
            if (houseName.Length == 0)
            {
                errors = true;
                ColorError(txtHouseName);
            }
            if (streetAndNumber.Length == 0)
            {
                errors = true;
                ColorError(txtStreetAndNumber);
            }
            if (postalCode.Length == 0)
            {
                errors = true;
                ColorError(txtPastalCode);
            }
            if (town.Length == 0)
            {
                errors = true;
                ColorError(txtTown);
            }
            if (basePrice == 0)
            {
                errors = true;
                ColorError(txtBasePrice);
            }
            if (reducedPrice == 0)
            {
                errors = true;
                ColorError(txtReducedPrice);
            }
            if (daysForReduction == 0)
            {
                errors = true;
                ColorError(txtDaysForReduction);
            }
            if (deposit == 0)
            {
                errors = true;
                ColorError(txtGuarantee);
            }
            if (maxPersons == 0)
            {
                errors = true;
                ColorError(txtMaxPersons);
            }
            if (errors)
                return;

            // if new : create instance
            if (reasonForEntry == ReasonForEntry.newResidence)
            {
                if (cmbKindOfResidence.SelectedIndex == 0)
                    selectedResidence = new VacationHouse();
                else
                    selectedResidence = new Caravan();
                // don't forget to add new object to list!
                residences.AddResidence(selectedResidence);
            }

            // fill props
            selectedResidence.ResidenceName = houseName;
            selectedResidence.StreetAndNumber = streetAndNumber;
            selectedResidence.PostalCode = postalCode;
            selectedResidence.Town = town;
            selectedResidence.BasePrice = basePrice;
            selectedResidence.ReducedPrice = reducedPrice;
            selectedResidence.DaysForReduction = daysForReduction;
            selectedResidence.Deposit = deposit;
            selectedResidence.MaxPersons = maxPersons;
            selectedResidence.IsRentable = rentable;
            selectedResidence.Microwave = microwave;
            selectedResidence.TV = tv;
            if (selectedResidence is VacationHouse)
            {
                ((VacationHouse)selectedResidence).DishWasher = dishWasher;
                ((VacationHouse)selectedResidence).WashingMachine = washingMachine;
                ((VacationHouse)selectedResidence).WoodStove = woodStove;
            }
            else
            {
                ((Caravan)selectedResidence).PrivateSanitaryBlock = privateSanitaryBlock;
            }

            // close this window
            refreshRequired = true;
            this.Close();

        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void cmbKindOfResidence_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbKindOfResidence.IsLoaded)
                ManageCheckBoxes();
        }
        #endregion


        #region Methods
        private void DisplayView()
        {
            grpData.IsEnabled = false;
            lblTitle.Content = "Verblijf bekijken";
            btnSave.Visibility = Visibility.Hidden;
            btnCancel.Content = "Sluiten";
        }
        private void DisplayNew()
        {
            grpData.IsEnabled = true;
            btnSave.Visibility = Visibility.Visible;
            btnCancel.Content = "Annuleren";
            lblTitle.Content = "Verblijf toevoegen";
        }
        private void DisplayEdit()
        {
            grpData.IsEnabled = true;
            btnSave.Visibility = Visibility.Visible;
            btnCancel.Content = "Annuleren";
            lblTitle.Content = "Verblijf wijzigen";

        }
        private void ClearControls()
        {
            cmbKindOfResidence.SelectedIndex = 0;
            txtHouseName.Text = "";
            txtStreetAndNumber.Text = "";
            txtPastalCode.Text = "";
            txtTown.Text = "";
            txtBasePrice.Text = "";
            txtReducedPrice.Text = "";
            txtDaysForReduction.Text = "";
            txtGuarantee.Text = "";
            txtMaxPersons.Text = "";
            chkMicrowave.IsChecked = false;
            chkTV.IsChecked = false;
            chkDishwasher.IsChecked = false;
            chkWashingMachine.IsChecked = false;
            chkWoodStove.IsChecked = false;
            chkPrivateSanitaryBlock.IsChecked = false;
            chkPrivateSanitaryBlock.Visibility = Visibility.Hidden;
            chkRentable.IsChecked = false;
            cmbKindOfResidence.IsEnabled = true;
        }
        private void PopulateControls()
        {
            txtHouseName.Text = selectedResidence.ResidenceName;
            txtStreetAndNumber.Text = selectedResidence.StreetAndNumber;
            txtPastalCode.Text = selectedResidence.PostalCode.ToString();
            txtTown.Text = selectedResidence.Town;
            txtBasePrice.Text = selectedResidence.BasePrice.ToString();
            txtReducedPrice.Text = selectedResidence.ReducedPrice.ToString();
            txtDaysForReduction.Text = selectedResidence.DaysForReduction.ToString();
            txtGuarantee.Text = selectedResidence.Deposit.ToString();
            txtMaxPersons.Text = selectedResidence.MaxPersons.ToString();
            chkMicrowave.IsChecked = selectedResidence.Microwave;
            chkTV.IsChecked = selectedResidence.TV;
            chkRentable.IsChecked = selectedResidence.IsRentable;
            if (selectedResidence is VacationHouse)
            {
                cmbKindOfResidence.SelectedIndex = 0;
                ManageCheckBoxes();
                chkDishwasher.IsChecked = ((VacationHouse)selectedResidence).DishWasher;
                chkWashingMachine.IsChecked = ((VacationHouse)selectedResidence).WashingMachine;
                chkWoodStove.IsChecked = ((VacationHouse)selectedResidence).WoodStove;

            }
            else
            {
                cmbKindOfResidence.SelectedIndex = 1;
                ManageCheckBoxes();
                chkPrivateSanitaryBlock.IsChecked = ((Caravan)selectedResidence).PrivateSanitaryBlock;
            }
        }
        private void ManageCheckBoxes()
        {
            if (cmbKindOfResidence.SelectedIndex == 0)
            {
                chkPrivateSanitaryBlock.Visibility = Visibility.Hidden;
                chkDishwasher.Visibility = Visibility.Visible;
                chkWashingMachine.Visibility = Visibility.Visible;
                chkWoodStove.Visibility = Visibility.Visible;
                chkPrivateSanitaryBlock.IsChecked = false;
            }
            else
            {
                chkPrivateSanitaryBlock.Visibility = Visibility.Visible;
                chkDishwasher.Visibility = Visibility.Hidden;
                chkWashingMachine.Visibility = Visibility.Hidden;
                chkWoodStove.Visibility = Visibility.Hidden;
                chkDishwasher.IsChecked = false;
                chkWashingMachine.IsChecked = false;
                chkWoodStove.IsChecked = false;
            }
        }
        private void ColorError(Control ctrl)
        {
            ctrl.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFBEDED"));
            ctrl.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF0000"));
            ctrl.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF0000"));
        }
        private void ColorOK(Control ctrl)
        {
            ctrl.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
            ctrl.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF000000"));
            ctrl.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF000000"));
        }
        private void PrepareBeforeSave()
        {
            ColorOK(txtHouseName);
            ColorOK(txtStreetAndNumber);
            ColorOK(txtPastalCode);
            ColorOK(txtTown);
            ColorOK(txtBasePrice);
            ColorOK(txtReducedPrice);
            ColorOK(txtDaysForReduction);
            ColorOK(txtGuarantee);
            ColorOK(txtMaxPersons);
        }
        #endregion
    }
}
