using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Core.ViewModels.UsersViewModels;
using ViFlix.Data.Repository;
using ViFlix.Data.Users;

namespace ViFlix.Core.Services.User.PermissionsServices
{
    public interface IPermissionServices : IGenericRepository<Permissions>
    {
        IEnumerable<PermissionViewModel> GetAllPermission();
        PermissionViewModel GetPermissionById(long Id);
        bool CheckPermission(int permissionId, string userName);
    }
}
