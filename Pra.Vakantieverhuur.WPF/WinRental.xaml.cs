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
    /// Interaction logic for WinRental.xaml
    /// </summary>
    public partial class WinRental : Window
    {
        public WinRental()
        {
            InitializeComponent();
        }

        public enum ReasonForEntry { newRental, editRental }
        public ReasonForEntry reasonForEntry;
        public Residence selectedResidence = null;
        public Rental selectedRental = null;
        public bool refreshRequired = false;

        private Rentals rentals = new Rentals();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(reasonForEntry == ReasonForEntry.editRental)
            {
                selectedResidence = selectedRental.HolidayResidence;
            }
            DisplayResidenceData();
            PopulateTenants();
        }
        private void dtpDateStart_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ProcessData();
        }
        private void dtpDateEnd_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ProcessData();
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if(cmbTenant.SelectedItem == null)
            {
                cmbTenant.Focus();
                return;
            }
            if(dtpDateStart.SelectedDate == null)
            {
                dtpDateStart.Focus();
                return;
            }
            if (dtpDateEnd.SelectedDate == null)
            {
                dtpDateEnd.Focus();
                return;
            }
            if(reasonForEntry == ReasonForEntry.newRental)
            {
                selectedRental = new Rental();
                // don't forget to add new object to list!
                rentals.AddRental(selectedRental);
            }
            selectedRental.HolidayTenant = (Tenant)cmbTenant.SelectedItem;
            selectedRental.HolidayResidence = selectedResidence;
            selectedRental.DateStart = (DateTime)dtpDateStart.SelectedDate;
            selectedRental.DateEnd = (DateTime)dtpDateEnd.SelectedDate;
            selectedRental.IsDeposidPaid = (bool)chkDepositPaid.IsChecked;
            selectedRental.ToPay = CalculateTotalToPay();
            decimal.TryParse(txtPaid.Text, out decimal paid);
            selectedRental.Paid = paid;
            refreshRequired = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DisplayResidenceData()
        {
            if (selectedResidence is VacationHouse)
            {
                chkPrivateSanitaryBlock.Visibility = Visibility.Hidden;
                chkDishwasher.Visibility = Visibility.Visible;
                chkWashingMachine.Visibility = Visibility.Visible;
                chkWoodStove.Visibility = Visibility.Visible;

                chkDishwasher.IsChecked = ((VacationHouse)selectedResidence).DishWasher;
                chkWashingMachine.IsChecked = ((VacationHouse)selectedResidence).WashingMachine;
                chkWoodStove.IsChecked = ((VacationHouse)selectedResidence).WoodStove;
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
                chkPrivateSanitaryBlock.IsChecked = ((Caravan)selectedResidence).PrivateSanitaryBlock;

            }
            txtResidenceName.Text = selectedResidence.ResidenceName;
            txtStreetAndNumber.Text = selectedResidence.StreetAndNumber;
            txtPostalCode.Text = selectedResidence.PostalCode.ToString();
            txtTown.Text = selectedResidence.Town;
            txtBasePrice.Text = selectedResidence.BasePrice.ToString();
            txtReducedPrice.Text = selectedResidence.ReducedPrice.ToString();
            txtDaysForReduction.Text = selectedResidence.DaysForReduction.ToString();
            txtGuarantee.Text = selectedResidence.Deposit.ToString();
            txtMaxPersons.Text = selectedResidence.MaxPersons.ToString();
            chkMicrowave.IsChecked = selectedResidence.Microwave;
            chkTV.IsChecked = selectedResidence.TV;
        }
        private void PopulateTenants()
        {
            Tenants tenants = new Tenants();
            foreach (Tenant tenant in tenants.AllTenants)
            {
                if (!tenant.IsBlackListed)
                {
                    cmbTenant.Items.Add(tenant);
                }
            }
        }
        private void ProcessData()
        {
            if (!this.IsLoaded) return;

            Rentals rentals = new Rentals();
            if (dtpDateStart.SelectedDate == null) return;
            if (dtpDateEnd.SelectedDate == null) return;

            DateTime dateStart = (DateTime)dtpDateStart.SelectedDate;
            DateTime dateEnd = (DateTime)dtpDateEnd.SelectedDate;
            if (dateEnd <= dateStart)
            {
                dateEnd = dateStart.AddDays(1);
                dtpDateEnd.SelectedDate = dateEnd;
            }

            lblOverbooking.Content = "GEEN OVERBOEKING";
            lblOverbooking.Background = Brushes.ForestGreen;
            lblOverbooking.Foreground = Brushes.White;
            btnSave.Visibility = Visibility.Visible;
            foreach (Rental rental in rentals.AllRentals)
            {
                if (rental.HolidayResidence == selectedResidence && (rental != selectedRental))
                {
                    if (dateStart >= rental.DateStart && dateStart < rental.DateEnd)
                    {
                        lblOverbooking.Background = Brushes.Tomato;
                        lblOverbooking.Foreground = Brushes.White;
                        lblOverbooking.Content = "OVERBOEKING";
                        btnSave.Visibility = Visibility.Hidden;
                    }
                    if (dateEnd > rental.DateStart && dateEnd <= rental.DateEnd)
                    {
                        lblOverbooking.Background = Brushes.Tomato;
                        lblOverbooking.Foreground = Brushes.White;
                        lblOverbooking.Content = "OVERBOEKING";
                        btnSave.Visibility = Visibility.Hidden;
                    }
                }
            }


            TimeSpan ts = (TimeSpan)(dtpDateEnd.SelectedDate - dtpDateStart.SelectedDate);
            int numberOfOvernightStays = (int)ts.TotalDays;
            lblNumberOfOvernightStays.Content = numberOfOvernightStays.ToString();

            int daysForReduction = selectedResidence.DaysForReduction;
            decimal toPay = CalculateTotalToPay();
            if (daysForReduction > numberOfOvernightStays)
            {
                lblTotalToPay.Content = $"{numberOfOvernightStays} x {selectedResidence.BasePrice} = {toPay}";
            }
            else
            {
                lblTotalToPay.Content = $"{numberOfOvernightStays} x {selectedResidence.ReducedPrice} = {toPay}";
            }

            decimal.TryParse(txtPaid.Text, out decimal paid);
            txtPaid.Text = paid.ToString();

            decimal toBePaid = toPay - paid;
            lblToBePaid.Content = toBePaid.ToString();

        }
        private decimal CalculateTotalToPay()
        {
            TimeSpan ts = (TimeSpan)(dtpDateEnd.SelectedDate - dtpDateStart.SelectedDate);
            int numberOfOvernightStays = (int)ts.TotalDays;
            int daysForReduction = selectedResidence.DaysForReduction;
            decimal toPay;
            if (daysForReduction > numberOfOvernightStays)
            {
                toPay = numberOfOvernightStays * selectedResidence.BasePrice;
            }
            else
            {
                toPay = numberOfOvernightStays * selectedResidence.ReducedPrice;
            }
            return toPay;
        }

     }
}
