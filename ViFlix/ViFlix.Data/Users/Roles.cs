using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Data.Context;

namespace ViFlix.Data.Users
{
    public class Roles : BaseEntitiies
    {
        public string Title { get; set; }

        #region Relations 
        public List<UserRoles> userRoles { get; set; }
        public List<RolePermissions> rolePermissions { get; set; }
        #endregion
    }
}
