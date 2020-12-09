using System;
using System.Collections.Generic;
using System.Text;
using Pra.Vakantieverhuur.CORE.Entities;

namespace Pra.Vakantieverhuur.CORE.Services
{
    public class Residences
    {
        #region PRIVATE FIELDS 
        private List<Residence> allResidences;
        #endregion

        #region PROPS 
        public List<Residence> AllResidences
        {
            get { return allResidences; }
        }
        #endregion

        #region CONSTRUCTORS   
        public Residences()
        {
            allResidences = new List<Residence>();
            allResidences.Add(new VacationHouse() { StreetAndNumber = "Klaverstraat 1", PostalCode = "8000", Town = "Brugge", ResidenceName = "'t Eeuwig leven", MaxPersons = 2, BasePrice = 70M, DaysForReduction = 7, ReducedPrice = 65M, Deposit = 140M, Microwave = true, TV = true, DishWasher = false, WashingMachine = false, WoodStove = false });
            allResidences.Add(new VacationHouse() { StreetAndNumber = "Steenstraat 123/7", PostalCode = "8000", Town = "Brugge", ResidenceName = "Kiekekot", MaxPersons = 4, BasePrice = 120M, DaysForReduction = 7, ReducedPrice = 110M, Deposit = 240M, Microwave = true, TV = true, DishWasher = true, WashingMachine = true, WoodStove = false });
            allResidences.Add(new VacationHouse() { StreetAndNumber = "Stoofstraat 99", PostalCode = "8000", Town = "Brugge", ResidenceName = "Zwaluwnest", MaxPersons = 2, BasePrice = 85M, DaysForReduction = 7, ReducedPrice = 75M, Deposit = 170M, Microwave = true, TV = true, DishWasher = true, WashingMachine = true, WoodStove = true });
            allResidences.Add(new Caravan() { StreetAndNumber = "Veltemweg 109 - P57", PostalCode = "8310", Town = "Brugge", ResidenceName = "Krot & Co", MaxPersons = 3, BasePrice = 45M, DaysForReduction = 7, ReducedPrice = 40M, Deposit = 90M, Microwave = false, TV = true, PrivateSanitaryBlock = false });
        }
        #endregion

        #region CRUD  
        public void AddResidence(Residence residence)
        {
            allResidences.Add(residence);
        }
        public void UpdateResidence(Residence residence)
        {
            // nothing to do in this situation
        }
        public bool DeleteResidence(Residence residence)
        {
            Rentals rentals = new Rentals();
            foreach(Rental rental in rentals.AllRentals)
            {
                if (rental.HolidayResidence == residence)
                {
                    return false;
                }
            }
            allResidences.Remove(residence);
            residence = null;
            return true;
        }
        #endregion
    }
}
