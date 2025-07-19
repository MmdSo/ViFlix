using AutoMapper;
using FirstShop.Core.Security;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Core.Mapper;
using ViFlix.Core.Services.User.UserServices;
using ViFlix.Core.ViewModels.UsersViewModels;
using ViFlix.Data.Context;
using ViFlix.Data.Users;
using Xunit;

namespace ViflixUnitTests
{
    public class UserServiceTest : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserService _userService;

        public UserServiceTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MapperProfile>();
            });

            _mapper = config.CreateMapper();
            _userService = new UserService(_context, _mapper);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        #region userNameExist
        [Fact]
        public void UserNameExist_WithExistUserTest_ReturnTrue()
        {
            _context.Users.Add(new SiteUsers
            {
                UserName = "mmd_sohrabi",
                FirstName = "mmd",
                LastName = "sohrabi",
                Password = "123456",
            });
            _context.SaveChanges();

            var result = _userService.UserNameExist("mmd_sohrabi");

            Assert.True(result);
        }

        [Fact]
        public void UserNameExist_WithNonExistingUser_ReturnsFalse()
        {
            _context.Users.Add(new SiteUsers
            {
                UserName = "mmd_sohrabi",
                FirstName = "mmd",
                LastName = "sohrabi",
                Password = "123456",
            });
            _context.SaveChanges();

            var result = _userService.UserNameExist("unknown");

            Assert.False(result);
        }
        #endregion

        #region Register
        [Fact]
        public async Task Register_WithUniqUserName_ReturnsTrue()
        {
            var registerModel = new RegisterViewModel
            {
                UserName = "mmdi",
                FirstName = "mmd",
                LastName = "so",
                Email = "mmd@gmailcom",
                Password = "123456",
                ConfirmPassword = "123456"
            };

            var result = await _userService.Register(registerModel);

            Assert.True(result > 0);
            Assert.Single(_context.Users);
            Assert.Equal("mmdi", _context.Users.First().UserName);
        }

        [Fact]
        public async Task Register_WithExistingUsername_ReturnsFalse()
        {
            _context.Users.Add(new SiteUsers
            {
                UserName = "mmdi",
                FirstName = "mmd",
                LastName = "so",
                Password = "123456"
            });
            _context.SaveChanges();

            var registerModel = new RegisterViewModel
            {
                UserName = "mmdi",
                FirstName = "mmd",
                LastName = "so",
                Email = "mmd@gmailcom",
                Password = "123456",
                ConfirmPassword = "123456"
            };

            var result = await _userService.Register(registerModel);

            Assert.False(result > 0);
            Assert.Single(_context.Users);
        }
        #endregion

        #region Login
        [Fact]
        public void Login_WithValidCredential_ReturnUser()
        {
            var hashedPassword = PasswordHelper.EncodePasswordMd5("123456");

            _context.Users.Add(new SiteUsers
            {
                UserName = "mmdi",
                Password = hashedPassword,
                FirstName = "mmd",
                LastName = "sohrabi",
                Email = "mmd@gmail.com"
            });
            _context.SaveChanges();

            var loginViewmodel = new LoginViewModel
            {
                UserName = "mmdi",
                Password = "123456"
            };

            var result = _userService.Login(loginViewmodel);

            Assert.NotNull(result);
            Assert.Equal("mmdi", result.UserName);
        }

        [Fact]
        public void Login_WithInvalidCredential_ReturnNull()
        {
            var hashedPassword = PasswordHelper.EncodePasswordMd5("123456");

            _context.Users.Add(new SiteUsers
            {
                UserName = "mmdi",
                Password = hashedPassword,
                FirstName = "mmd",
                LastName = "sohrabi",
                Email = "mmd@gmail.com"
            });
            _context.SaveChanges();

            var loginModel = new LoginViewModel
            {
                UserName = "wrongUser",
                Password = "wrongPass"
            };

            var result = _userService.Login(loginModel);

            Assert.Null(result);
        }
        #endregion

        #region GetUserNamebyUserId

        [Fact]
        public void GetUserIdByUserName_WIthValidUserName_ReturnTrue()
        {
            //Arrange

            var user = new SiteUsers
            {
                UserName = "mmdi",
                FirstName = "mmd",
                LastName = "so",
                Password = PasswordHelper.EncodePasswordMd5("1234"),
                Email = "test@gmail.com"
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            //Act 

            var result = _userService.GetUserIdByUserName("mmdi");

            //Assert

            Assert.Equal(user.Id, result);
        }

        #endregion

        #region AddUser

        [Fact]
        public async Task AddUser_withValidData_returnTrue()
        {
            //Arrange
            var user = new UserViewModel
            {
                UserName = "mmd_sohrabi",
                FirstName = "mmd",
                LastName = "sohrabi",
                Password = "123456",
                Email = "test@gmail.com"
            };

            //Act
            var result = await _userService.AddUser(user);

            //Assert
            Assert.True(result > 0);
            var userInDb = _context.Users.FirstOrDefault(u => u.Id == result);
            Assert.NotNull(userInDb);
            Assert.Equal("mmd_sohrabi", userInDb.UserName);
        }

        [Fact]
        public async Task Adduser_withInvalidData_ReturnFalse()
        {
            //Arrange
            var user = new UserViewModel
            {
                UserName = "mmd_sohrabi",
                FirstName = "mmd",
                LastName = "sohrabi",
                Password = "123456",
                Email = "test@gmail.com"
            };
            await _userService.AddUser(user);

            //Act
            var result = await _userService.AddUser(user);

            //Assert
            Assert.Equal(-1, result);
        }

        #endregion

        #region EditUser
        [Fact]
        public async Task EditUser_WithValidData_ReturnTrue()
        {
            //Arrange
            var user = new SiteUsers
            {
                UserName = "mmd_sohrabi",
                FirstName = "mmd",
                LastName = "sohrabi",
                Password = PasswordHelper.EncodePasswordMd5("123456"),
                Email = "test@gmail.com"
            };
            _context.Users.Add(user);
            _context.SaveChanges();

            var UpdateUser = new UserViewModel
            {
                Id = user.Id,
                UserName = "mmd_so",
                FirstName = "mmdi",
                LastName = "so",
                Email = "testgm@gmail.com"
            };

            //Act
             await _userService.EditUser(UpdateUser);

            //Assert
            

            var update = await _context.Users.FindAsync(user.Id);
            Assert.Equal("mmd_sohrabi", UpdateUser.UserName);
            Assert.Equal("mmd", UpdateUser.FirstName);
            Assert.Equal("sohrabi", UpdateUser.LastName);
            Assert.Equal("test@gmail.com", UpdateUser.Email);

        }

       

        #endregion
    }
}
