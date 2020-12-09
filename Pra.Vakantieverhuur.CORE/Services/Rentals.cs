using System;
using System.Collections.Generic;
using System.Text;
using Pra.Vakantieverhuur.CORE.Entities;

namespace Pra.Vakantieverhuur.CORE.Services
{
    public class Rentals
    {
        #region PRIVATE FIELDS 
        private List<Rental> allRentals;
        #endregion

        #region PROPS  
        public List<Rental> AllRentals
        {
            get { return allRentals; }
        }
                #endregion

        #region CONSTRUCTORS 
        public Rentals()
        {
            allRentals = new List<Rental>();
        }
        #endregion

        #region CRUD  
        public void AddRental(Rental rental)
        {
            allRentals.Add(rental);
        }
        public void UpdateRental(Rental rental)
        {
            // nothing to do in this situation
        }
        public bool DeleteRental(Rental rental)
        {
            allRentals.Remove(rental);
            rental = null;
            return true;
        }
        #endregion


    }
}
