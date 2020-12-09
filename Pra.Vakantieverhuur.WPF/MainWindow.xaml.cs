using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Pra.Vakantieverhuur.CORE.Entities;
using Pra.Vakantieverhuur.CORE.Services;


namespace Pra.Vakantieverhuur.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Global variables
        Rentals rentals;
        Residences residences;
        Tenants tenants;
        #endregion

        #region Event-handlers
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            rentals = new Rentals();
            residences = new Residences();
            tenants = new Tenants();
            PopulateResidences();
        }
        private void btnResidenceNew_Click(object sender, RoutedEventArgs e)
        {
            WinResidences winResidences = new WinResidences();
            winResidences.selectedResidence = null;
            winResidences.reasonForEntry = WinResidences.ReasonForEntry.newResidence;
            winResidences.ShowDialog();

            if(winResidences.refreshRequired)
            {
                cmbKindOfResidence.SelectedIndex = 0;
                PopulateResidences();
                lstResidences.SelectedItem = winResidences.selectedResidence;
                lstResidences_SelectionChanged(null, null);
            }
        }
        private void btnResidenceEdit_Click(object sender, RoutedEventArgs e)
        {
            if (lstResidences.SelectedItem == null) return;

            WinResidences winResidences = new WinResidences();
            winResidences.selectedResidence = (Residence)lstResidences.SelectedItem;
            winResidences.reasonForEntry = WinResidences.ReasonForEntry.editResidence;
            winResidences.ShowDialog();

            if (winResidences.refreshRequired)
            {
                cmbKindOfResidence.SelectedIndex = 0;
                PopulateResidences();
                lstResidences.SelectedItem = winResidences.selectedResidence;
                lstResidences_SelectionChanged(null, null);
            }
        }
        private void btnResidenceDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lstResidences.SelectedItem == null) return;
            if(!residences.DeleteResidence((Residence)lstResidences.SelectedItem))
            {
                 MessageBox.Show("Er loopt nog een verhuring op deze residentie", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                PopulateResidences();
            }

        }
        private void btnResidenceView_Click(object sender, RoutedEventArgs e)
        {
            if (lstResidences.SelectedItem == null) return;

            WinResidences winResidences = new WinResidences();
            winResidences.selectedResidence = (Residence)lstResidences.SelectedItem;
            winResidences.reasonForEntry = WinResidences.ReasonForEntry.viewResidence;
            winResidences.ShowDialog();
        }
        private void lstResidences_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dgrRentals.Items.Clear();
            if (lstResidences.SelectedItem == null) return;

            Residence residence = (Residence)lstResidences.SelectedItem;
            foreach(Rental rental in rentals.AllRentals)
            {
                if(rental.HolidayResidence == residence)
                {
                    dgrRentals.Items.Add(rental);
                }
            }

        }
        private void cmbKindOfResidence_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cmbKindOfResidence.IsLoaded)
                PopulateResidences();
        }
        private void btnNewRental_Click(object sender, RoutedEventArgs e)
        {
            if (lstResidences.SelectedItem != null)
            {               
                WinRental winRental = new WinRental();
                winRental.selectedResidence = (Residence)lstResidences.SelectedItem;
                winRental.reasonForEntry = WinRental.ReasonForEntry.newRental;
                winRental.ShowDialog();

                if (winRental.refreshRequired)
                {
                    lstResidences_SelectionChanged(null, null);
                }
            }
        }
        private void btnRentalEdit_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Rental rental = (Rental)button.DataContext;

            WinRental winRental = new WinRental();
            winRental.selectedRental = rental;
            winRental.reasonForEntry = WinRental.ReasonForEntry.editRental;
            winRental.ShowDialog();

            if (winRental.refreshRequired)
            {
                lstResidences_SelectionChanged(null, null);
            }
        }
        private void btnRentalDelete_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Rental rental = (Rental)button.DataContext;

            if (MessageBox.Show("Deze verhuur annuleren?", "Verhuur verwijderen", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                rentals.DeleteRental(rental);
                lstResidences_SelectionChanged(null, null);
            }
        }
        #endregion

        #region Methods
        private void PopulateResidences()
        {
            dgrRentals.Items.Clear();
            lstResidences.ItemsSource = null;
            if (cmbKindOfResidence.SelectedIndex == 0)
            {
                lstResidences.ItemsSource = residences.AllResidences;
            }
            else if (cmbKindOfResidence.SelectedIndex == 1)
            {
                List<VacationHouse> vacationHouses = new List<VacationHouse>();
                foreach (Residence residence in residences.AllResidences)
                {
                    if (residence is VacationHouse)
                        vacationHouses.Add((VacationHouse)residence);
                }
                lstResidences.ItemsSource = vacationHouses;
            }
            else
            {
                List<Caravan> caravans = new List<Caravan>();
                foreach (Residence residence in residences.AllResidences)
                {
                    if (residence is Caravan)
                        caravans.Add((Caravan)residence);
                }
                lstResidences.ItemsSource = caravans;
            }
        }
        #endregion


    }
}
