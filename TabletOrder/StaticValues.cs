using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TabletOrder.Entity;

namespace TabletOrder
{
    public static class StaticValues
    {
        public static List<ProductGroupDto> ProductGroups { get; set; }
        
        public static int TaxPercent { get; set; }
        public static int DefaultFact { get; set; }//1=>A5 - 2=>8cm - 3=>6cm
        public static string SaleMali { get; set; }
        /// <summary>
        /// index=0 -> MasterPrinter
        /// index=1 -> KitchenPrinter
        /// index=2 -> CoffeShopPrinter
        /// </summary>
        public static List<PrinterSetupDto> PrinterSetupList { get; set; }

    }
}