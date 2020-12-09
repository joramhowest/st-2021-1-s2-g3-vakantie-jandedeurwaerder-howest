using System;
using System.Collections.Generic;
using System.Text;

namespace Pra.Vakantieverhuur.CORE.Entities
{
    public class Rental
    {
        #region PROPS 
        public Residence HolidayResidence { get; set; }
        public Tenant HolidayTenant { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public bool IsDeposidPaid { get; set; }
        public decimal Paid { get; set; }
        public decimal ToPay { get; set; }
        #endregion
    }
}
