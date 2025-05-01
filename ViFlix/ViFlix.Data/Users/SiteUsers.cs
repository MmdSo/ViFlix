﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Data.Context;

namespace ViFlix.Data.Users
{
    public  class SiteUsers : BaseEntitiies
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? age { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; }
        public string? avatar { get; set; }
        public long RoleId { get; set; }

        #region Relations
        public List<Roles> role { get; set; }
        #endregion
    }
}
