using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TabletOrder
{
    public class LoginedData
    {
        public bool IsAuthenticated { get; set; } = false;
        public int UserId { get; set; }
        public string UserName { get; set; }
    }
}