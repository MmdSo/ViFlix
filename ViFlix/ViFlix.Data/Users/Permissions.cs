using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Data.Context;

namespace ViFlix.Data.Users
{
    public class Permissions : BaseEntitiies
    {
        public string Title { get; set; }

        #region Relations 
        public List<RolePermissions>  rolePermissions { get; set; }
        #endregion
    }
}
