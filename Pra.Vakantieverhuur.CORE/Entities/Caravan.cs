using System;
using System.Collections.Generic;
using System.Text;

namespace Pra.Vakantieverhuur.CORE.Entities
{
    public class Caravan : Residence
    {
        #region PROPS 
        public bool? PrivateSanitaryBlock { get; set; }
        #endregion

        #region OVERRIDE
        public override string ToString()
        {
            return $"C - {ResidenceName} - {Town}";
        }
        #endregion
    }
}
