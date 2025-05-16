using AutoMapper;
using FirstShop.Core.Security;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Core.Tools;
using ViFlix.Core.ViewModels.UsersViewModels;
using ViFlix.Data.Context;
using ViFlix.Data.Repository;
using ViFlix.Data.Users;

namespace ViFlix.Core.Services.User.UserServices
{
    public class UserService : GenericRepository<SiteUsers>, IUserService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public UserService(AppDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public void AddRoleToUser(List<long> roleId, long userId)
        {
            foreach (var ro in roleId)
            {
                _context.UserRoles.Add(new UserRoles()
                {
                    UserId = userId,
                    RoleId = ro
                });
            }

            SaveChanges().GetAwaiter();
        }

        public async Task<long> AddUser(UserViewModel user )
        {
            var person = _mapper.Map<UserViewModel, SiteUsers>(user);
            await AddEntity(person);
            await SaveChanges();
            return user.Id;
        }

        public async Task ChangeEmail(ChangeEmailViewModel Email)
        {
            var person = GetUserById(Email.Id);
            person.Email = Email.NewEmail;
            var user = _mapper.Map<UserViewModel, SiteUsers>(person);

            EditEntity(user);
            await SaveChanges();
        }

        public async Task ChangePassword(ChangePasswordViewModel pass)
        {
            var person = GetUserById(pass.Id);
            person.Password = PasswordHelper.EncodePasswordMd5(pass.NewPassword);

            var user = _mapper.Map<UserViewModel, SiteUsers>(person);

            EditEntity(user);
            await SaveChanges();
        }

        public async Task DeleteUser(long UserId)
        {
            UserViewModel user = GetUserById(UserId);
            var person = _mapper.Map<UserViewModel, SiteUsers>(user);

            person.IsDelete = true;
            EditEntity(person);
            await SaveChanges();
        }

        public async Task EditProfile(ProfileViewModel prof, IFormFile AvatarImg)
        {
            if (AvatarImg != null)
            {
                prof.avatar = NameGenerator.GenerateUniqCode() + Path.GetExtension(AvatarImg.FileName);
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Images/avatar", prof.avatar);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    AvatarImg.CopyTo(stream);
                }
                Tools.ImageConverter ImgResizer = new Tools.ImageConverter();
                string thumbPath = Path.Combine(Directory.GetCurrentDirectory(), "Images/avatar", prof.avatar);
            }
            var user = GetUserById(prof.Id);

            user.avatar = prof.avatar;
            user.FirstName = prof.FirstName;
            user.LastName = prof.LastName;
            user.PhoneNumber = prof.PhoneNumber;
            user.age = prof.age;


            EditEntity(_mapper.Map<UserViewModel, SiteUsers>(user));
            

            await SaveChanges();
        }

        public async Task EditUser(UserViewModel user)
        {
            var person = _mapper.Map<UserViewModel, SiteUsers>(user);
            EditEntity(person);
            await SaveChanges();
        }

        public IEnumerable<UserViewModel> GetAllUsers()
        {
            var person = _mapper.Map<IEnumerable<SiteUsers>, IEnumerable<UserViewModel>>(GetAll());
            return person;
        }

        public UserViewModel GetUserById(long? id)
        {
            var user = _mapper.Map<SiteUsers, UserViewModel>(GetEntityById(id));
            return user;
        }

        public async Task<UserViewModel> GetUserByIdAsync(long? id)
        {
            var user = _mapper.Map<SiteUsers, UserViewModel>(await GetEntityByIdAsync(id));
            return user;
        }

        public async Task<ChangeEmailViewModel> GetUserByIdChangeEmail(long? id)
        {
            var user = _mapper.Map<UserViewModel, SiteUsers>(GetUserById(id));
            var Em = _mapper.Map<SiteUsers, ChangeEmailViewModel>(user);

            Em.CurrentEmail = user.Email;

            return Em;
        }

        public async Task<ChangePasswordViewModel> GetUserByIdChangePaswword(long? id)
        {
            var user = await GetEntityByIdAsync(id);

            var pass = _mapper.Map<SiteUsers, ChangePasswordViewModel>(user);
            pass.CurrentPassword = user.Password;

            return pass;
        }

        public ProfileViewModel GetUserByIdProfile(long? id)
        {
            var user = _mapper.Map<SiteUsers, ProfileViewModel>(GetEntityById(id));
            return user;
        }

        public long GetUserIdByUserName(string name)
        {
            var user = _mapper.Map<SiteUsers, UserViewModel>(GetAll().SingleOrDefault(u => u.UserName == name)).Id;
            return user;
        }

        public List<long> GetUserRolesByUserID(long userID)
        {
            return _context.UserRoles.Where(u => u.UserId == userID).Select(u => u.RoleId).ToList();
        }

        public LoginViewModel Login(LoginViewModel login)
        {
            string pass = PasswordHelper.EncodePasswordMd5(login.Password);
            string username = login.UserName;

            var user = _mapper.Map<SiteUsers, LoginViewModel>(GetAll().SingleOrDefault(u => u.UserName == username && u.Password == pass));

            return user;
        }

        public async Task<long> Register(RegisterViewModel register)
        {
            register.Password = PasswordHelper.EncodePasswordMd5(register.Password);
            var user = _mapper.Map<RegisterViewModel, SiteUsers>(register);

            await AddEntity(user);
            _context.SaveChanges();

            return user.Id;
        }

        public async Task UpdateUserAsync(SiteUsers user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public void UpdateUserRoles(long userId, List<long> Roles)
        {
            _context.UserRoles.Where(u => u.UserId == userId)
                .ToList().ForEach(r => _context.UserRoles.Remove(r));

            AddRoleToUser(Roles, userId);
        }
        
        public bool UserNameExist(string username)
        {
            var user = _mapper.Map<SiteUsers, UserViewModel>(GetAll().SingleOrDefault(u => u.UserName == username));
            if (user == null)
                return false;
            else
                return true;
        }
    }
}
