using AutoMapper;
using FirstShop.Core.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
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
            // Arrange
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

            
            _context.Entry(user).State = EntityState.Detached;

            var updateUser = new UserViewModel
            {
                Id = user.Id,
                UserName = "mmd_sohrabi",
                FirstName = "mmdi",
                LastName = "so",
                Email = "testgm@gmail.com",
                Password = "123456"
            };

            // Act
            await _userService.EditUser(updateUser);

            // Assert
            var updatedUser = await _context.Users.FindAsync(user.Id);
            Assert.NotNull(updatedUser);
            Assert.Equal("mmd_sohrabi", updatedUser.UserName);
            Assert.Equal("mmdi", updatedUser.FirstName);
            Assert.Equal("so", updatedUser.LastName);
            Assert.Equal("testgm@gmail.com", updatedUser.Email);

        }

        #endregion

        #region ChangePassword

        [Fact]
        public async Task ChangePassword_WithValidCurrentPassword_ChangesPassword()
        {
            // Arrange
            var oldPassword = "123456";
            var newPassword = "newpass123";

            var initialUser = new UserViewModel
            {
                Id = 1,
                UserName = "testuser",
                FirstName = "Ali",
                LastName = "Rezai",
                age = 25,
                PhoneNumber = "09120000000",
                Password = PasswordHelper.EncodePasswordMd5(oldPassword),
                avatar = "user.png"
            };

            var mappedUser = _mapper.Map<SiteUsers>(initialUser);
            _context.Users.Add(mappedUser);
            _context.SaveChanges();

            _context.ChangeTracker.Clear();

            var changePasswordVM = new ChangePasswordViewModel
            {
                CurrentPassword = oldPassword,
                NewPassword = newPassword,
                ConfirmPassword = newPassword
            };

            // Act
            await _userService.ChangePassword(changePasswordVM, mappedUser.Id);

            // Assert
            var updated = _context.Users.FirstOrDefault(u => u.Id == mappedUser.Id);
            Assert.NotNull(updated);
            Assert.Equal(PasswordHelper.EncodePasswordMd5(newPassword), updated.Password);
        }



        #endregion

        #region ChangeEmail

        [Fact]
        public async Task ChangeEmail_WithValidData_returnTrue()
        {
            //Arrange
            var NewEmail = "NewEmail@gmail.com";
            var OldEmail = "OldEmail@gmail.com";

            var User = new UserViewModel
            {
                Id = 1,
                UserName = "testuser",
                FirstName = "Ali",
                LastName = "Rezai",
                age = 25,
                PhoneNumber = "09120000000",
                Password = PasswordHelper.EncodePasswordMd5("123456"),
                avatar = "user.png",
                Email = OldEmail
            };

            var mapperUser = _mapper.Map<SiteUsers>(User);
            _context.Users.Add(mapperUser);
            _context.SaveChanges();

            _context.ChangeTracker.Clear();

            var ChangeEmailVM = new ChangeEmailViewModel
            {
                CurrentEmail = OldEmail,
                NewEmail = NewEmail 
            };

            //Act
            await _userService.ChangeEmail(ChangeEmailVM, mapperUser.Id);

            //Assert
            var updated = _context.Users.FirstOrDefault(u => u.Id == mapperUser.Id);
            Assert.NotNull(updated);
        }

        #endregion

        #region DeleteUser 

        [Fact]
        public async Task DeleteUser_WithValidUserId_SetsIsDeleteTrue()
        {
            // Arrange
            var user = new SiteUsers
            {
                Id = 1,
                UserName = "testuser",
                Password = PasswordHelper.EncodePasswordMd5("123456"),
                IsDelete = false,
                FirstName = "Test",
                LastName = "User"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _context.ChangeTracker.Clear();

            var service = new UserService(_context, _mapper);

            // Act
            await service.DeleteUser(user.Id);

            // Assert
            var deletedUser = await _context.Users.FindAsync(user.Id);
            Assert.True(deletedUser.IsDelete);
        }


        #endregion
    }
}

