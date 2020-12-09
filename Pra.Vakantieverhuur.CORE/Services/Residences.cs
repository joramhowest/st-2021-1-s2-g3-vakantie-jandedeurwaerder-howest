using System;
using System.Collections.Generic;
using System.Text;
using Pra.Vakantieverhuur.CORE.Entities;

namespace Pra.Vakantieverhuur.CORE.Services
{
    public class Residences
    {
        private List<Residence> allResidences;

        public List<Residence> AllResidences
        {
            get { return allResidences; }
        }
        public Residences()
        {
            allResidences = new List<Residence>();
        }
    }
}
