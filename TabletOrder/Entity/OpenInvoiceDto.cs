using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TabletOrder.Entity
{
    [Serializable]
    public class OpenInvoiceDto
    {
        public long SaleHID { get; set; }
        public long FactNumber { get; set; }    
        public string InvoiceDate { get; set; } 
        public decimal InvoiceAmount { get; set; }
        public string Miz { get; set; }
    }
}