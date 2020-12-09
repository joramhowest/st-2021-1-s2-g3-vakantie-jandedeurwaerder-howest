using System;
using System.Collections.Generic;
using System.Text;

namespace Pra.Vakantieverhuur.CORE.Entities
{
    public class Tenant : Person
    {
        #region PROPS 
        public bool IsBlackListed { get; set; } = false;
        #endregion

        #region OVERRIDE 
        public override string ToString()
        {
            return $"{Name} {Firstname} - {Country}";
        }
        #endregion
    }
}
