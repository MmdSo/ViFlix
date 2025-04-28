using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Data.Context;

namespace ViFlix.Data.Users
{
    public class UserRoles : BaseEntitiies
    {
        public long UserId { get; set; }
        public long RoleId { get; set; }

        #region Relations
        [ForeignKey("UserId")]
        public SiteUsers users { get; set; }

        [ForeignKey("RoleId")]
        public Roles Role { get; set; }
        #endregion
    }
}
