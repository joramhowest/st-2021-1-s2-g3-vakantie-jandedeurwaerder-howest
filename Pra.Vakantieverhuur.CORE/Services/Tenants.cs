using System;
using System.Collections.Generic;
using System.Text;
using Pra.Vakantieverhuur.CORE.Entities;

namespace Pra.Vakantieverhuur.CORE.Services
{
    public class Tenants
    {
        private List<Tenant> allTenants;

        public List<Tenant> AllTenants
        {
            get { return allTenants; }
        }
        public Tenants()
        {
            allTenants = new List<Tenant>();
        }
    }
}
