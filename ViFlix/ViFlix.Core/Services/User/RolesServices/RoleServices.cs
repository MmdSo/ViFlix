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

namespace ViFlix.Core.Services.User.RolesServices
{
    public class RoleServices : GenericRepository<Roles>, IRoleServices
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public RoleServices(AppDbContext context , IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddPermissionsToRole(long roleID, List<long> permissions)
        {
            foreach (var p in permissions)
            {
                _context.RolePermissions.Add(new RolePermissions()
                {
                    PermissionId = p,
                    RoleId = roleID
                });
            }

            await SaveChanges();
        }

        public async Task<long> AddRole(RoleViewModel role)
        {
            var RE = _mapper.Map<RoleViewModel, Roles>(role);
            await AddEntity(RE);
            _context.SaveChanges();
            return role.Id;
        }

        public bool CheckPermission(long permissionId, string userName)
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

        public async Task DeleteRole(long? Id)
        {
            RoleViewModel role = GetRoleById(Id);

            var ro = _mapper.Map<RoleViewModel, Roles>(role);

            ro.IsDelete = true;

            EditEntity(ro);
            await SaveChanges();
        }

        public async Task EditRole(RoleViewModel role)
        {
            var RE = _mapper.Map<RoleViewModel, Roles>(role);
            EditEntity(RE);
            await SaveChanges();
        }

        public IEnumerable<RoleViewModel> GetAllRoles()
        {
            var RE = _mapper.Map<IEnumerable<Roles>, IEnumerable<RoleViewModel>>(GetAll());
            return RE;
        }

        public RoleViewModel GetRoleById(long? Id)
        {
            var role = _mapper.Map<Roles, RoleViewModel>(GetAll().SingleOrDefault(p => p.Id == Id));
            return role;
        }

        public async Task<RoleViewModel> GetRoleByIdAsync(long? Id)
        {
            var RE = _mapper.Map<Roles, RoleViewModel>(await GetEntityByIdAsync(Id));
            return RE;
        }

        public List<long> GetRolePermissions(long roleId)
        {
            return _context.RolePermissions
                .Where(r => r.RoleId == roleId)
                .Select(r => r.PermissionId).ToList();
        }

        public async Task UpdatePermissionsRole(long roleId, List<long> permissions)
        {
            _context.RolePermissions.Where(p => p.RoleId == roleId)
               .ToList().ForEach(p => _context.RolePermissions.Remove(p));

            await AddPermissionsToRole(roleId, permissions);
        }
    }
}
