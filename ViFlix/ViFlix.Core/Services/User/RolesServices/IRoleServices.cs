using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Core.ViewModels.UsersViewModels;
using ViFlix.Data.Repository;
using ViFlix.Data.Users;

namespace ViFlix.Core.Services.User.RolesServices
{
    public interface IRoleServices : IGenericRepository<Roles>
    {
        IEnumerable<RoleViewModel> GetAllRoles();
        Task<RoleViewModel> GetRoleByIdAsync(long? Id);
        Task<long> AddRole(RoleViewModel role);
        Task EditRole(RoleViewModel role);
        Task DeleteRole(long? Id);

        RoleViewModel GetRoleById(long? Id);
        List<long> GetRolePermissions(long roleId);
        Task AddPermissionsToRole(long roleID, List<long> permissions);
        Task UpdatePermissionsRole(long roleId, List<long> permissions);
        bool CheckPermission(long permissionId, string userName);
    }
}
