using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TabletOrder.Entity
{
    [Serializable]
    public class ProductGroupDto
    {
        public string GroupID { get; set; }
        public string GroupName { get; set; }
        public bool IsHaveTax { get; set; }
        public int PrinterId { get; set; }
    }
}