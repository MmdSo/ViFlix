using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Data.Context;

namespace ViFlix.Data.Users
{
    public class RolePermissions : BaseEntitiies
    {
        public long RoleId { get; set; }
        public long PermissionId { get; set; }

        #region Relations
        [ForeignKey("RoleId")]
        public Roles role { get; set; }

        [ForeignKey("PermissionId")]
        public Permissions permission { get; set; }
        #endregion
    }
}
