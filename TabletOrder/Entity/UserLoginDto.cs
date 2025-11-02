using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TabletOrder.Entity
{
    [Serializable]
    public class UserLoginDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        
    }
}