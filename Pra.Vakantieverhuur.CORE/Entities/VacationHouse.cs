using System;
using System.Collections.Generic;
using System.Text;

namespace Pra.Vakantieverhuur.CORE.Entities
{
    public class VacationHouse : Residence
    {
        #region PROPS 
        public bool? DishWasher { get; set; }
        public bool? WashingMachine { get; set; }
        public bool? WoodStove { get; set; }
        #endregion

        #region OVERRIDE 
        public override string ToString()
        {
            return $"H - {ResidenceName} - {Town}";
        }
        #endregion
    }
}
