using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TabletOrder.Entity
{
    [Serializable]
    public class TablesDto
    {
        public string TableName { get; set; }
        public string TableId { get; set; }
    }
}