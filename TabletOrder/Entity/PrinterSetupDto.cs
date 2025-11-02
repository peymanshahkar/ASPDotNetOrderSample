using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TabletOrder.Entity
{
    [Serializable]
    public class PrinterSetupDto
    {
        public int Id { get; set; }
        public string PrinterName { get; set; }
        public bool AutoPrint { get; set; }

    }
}