using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Core.ViewModels.UsersViewModels;
using ViFlix.Data.Context;
using ViFlix.Data.Repository;
using ViFlix.Data.Users;

namespace ViFlix.Core.Services.User.PermissionsServices
{
    public class PermissionServices : GenericRepository<Permissions>, IPermissionServices
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public PermissionServices(AppDbContext context , IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public bool CheckPermission(int permissionId, string userName)
        {
            long userId = _context.Users.Single(u => u.UserName == userName).Id;

            List<long> UserRoles = _context.UserRoles
                .Where(r => r.UserId == userId).Select(r => r.RoleId).ToList();

            if (!UserRoles.Any())
                return false;

            List<long> RolesPermission = _context.RolePermissions
                .Where(p => p.PermissionId == permissionId)
                .Select(p => p.RoleId).ToList();

            return RolesPermission.Any(p => UserRoles.Contains(p));
        }

        public IEnumerable<PermissionViewModel> GetAllPermission()
        {
            var PE = _mapper.Map<IEnumerable<Permissions>, IEnumerable<PermissionViewModel>>(GetAll());
            return PE;
        }

        public PermissionViewModel GetPermissionById(long Id)
        {
            var permissions = _mapper.Map<Permissions, PermissionViewModel>(GetAll().SingleOrDefault(p => p.Id == Id));

            return permissions;
        }
    }
}
