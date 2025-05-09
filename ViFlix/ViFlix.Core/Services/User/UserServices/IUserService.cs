using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Core.ViewModels.UsersViewModels;
using ViFlix.Data.Repository;
using ViFlix.Data.Users;

namespace ViFlix.Core.Services.User.UserServices
{
    public interface IUserService : IGenericRepository<SiteUsers>
    {
        IEnumerable<UserViewModel> GetAllUsers();
        Task<UserViewModel> GetUserByIdAsync(long? id);
        UserViewModel GetUserById(long? id);
        ProfileViewModel GetUserByIdProfile(long? id);
        Task<ChangePasswordViewModel> GetUserByIdChangePaswword(long? id);
        Task<ChangeEmailViewModel> GetUserByIdChangeEmail(long? id);
        long GetUserIdByUserName(string name);
        bool UserNameExist(string username);
        Task<long> AddUser(UserViewModel user);
        LoginViewModel Login(LoginViewModel login);
        Task<long> Register(RegisterViewModel register);
        Task EditUser(UserViewModel user);
        Task EditProfile(ProfileViewModel prof, IFormFile AvatarImg);
        Task ChangePassword(ChangePasswordViewModel pass);
        Task ChangeEmail(ChangeEmailViewModel Email);
        Task DeleteUser(long UserId);
        void AddRoleToUser(List<long> roleID, long userID);
        List<long> GetUserRolesByUserID(long userID);
        void UpdateUserRoles(long userId, List<long> Roles);
        Task UpdateUserAsync(SiteUsers user);
    }
}
