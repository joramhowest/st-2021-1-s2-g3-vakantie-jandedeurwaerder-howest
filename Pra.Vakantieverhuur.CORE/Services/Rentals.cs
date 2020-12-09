using System;
using System.Collections.Generic;
using System.Text;
using Pra.Vakantieverhuur.CORE.Entities;

namespace Pra.Vakantieverhuur.CORE.Services
{
    public class Rentals
    {
        private List<Rental> allRentals;

        public List<Rental> AllRentals
        {
            get { return allRentals; }
        }
        public Rentals()
        {
            allRentals = new List<Rental>();
        }
    }
}
