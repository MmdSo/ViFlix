using AutoMapper;
using FirstShop.Core.Security;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Core.Services.User.UserServices;
using ViFlix.Core.ViewModels.UsersViewModels;
using ViFlix.Data.Context;
using ViFlix.Data.Users;
using Xunit;

namespace ViflixUnitTests
{
    public class UserServiceTest
    {
        
        #region userNameExist
        [Fact]
        public void UserNameExist_WithExistUserTest_ReturnTrue()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "UserExistDb")
                .Options;

            using var context = new AppDbContext(options);
            context.Users.Add(new SiteUsers
            {
                UserName = "mmd_sohrabi",
                FirstName = "mmd",
                LastName = "sohrabi",
                Password = "123456",
                
            }) ;
            context.SaveChanges();

            var mockMapper = new Mock<IMapper>();

            var service = new UserService(context, mockMapper.Object);

            // Act
            var result = service.UserNameExist("mmd_sohrabi");

            // Assert
            Assert.True(result);
            
        }



        [Fact]
        public void UserNameExist_WithNonExistingUser_ReturnsFalse()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
               .UseInMemoryDatabase(databaseName: "UserExistDb")
               .Options;

            using var context = new AppDbContext(options);
            context.Users.Add(new SiteUsers
            {
                UserName = "mmd_sohrabi",
                FirstName = "mmd",
                LastName = "sohrabi",
                Password = "123456",

            });
            context.SaveChanges();

            var mockMapper = new Mock<IMapper>();

            var service = new UserService(context, mockMapper.Object);

            // Act
            var result = service.UserNameExist("unknown");

            // Assert
            Assert.False(result);
        }
        #endregion

        #region Register

        [Fact]
        public async Task Register_WithUniqUserName_ReturnsTrue()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
               .UseInMemoryDatabase(databaseName: "RegisterUniqueTest")
               .Options;

            using var context = new AppDbContext(options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RegisterViewModel, SiteUsers>();
            });

            var mapper = config.CreateMapper();

            var userService = new UserService(context, mapper);

            var registerModel = new RegisterViewModel
            {
                UserName = "mmdi",
                FirstName = "mmd",
                LastName = "so",
                Email = "mmd@gmailcom",
                Password = "123456",
                ConfirmPassword = "123456"
            };

            //Act

            var result =  await userService.Register(registerModel);

            //Assert 

            Assert.True(result > 0);
            Assert.Single(context.Users);
            Assert.Equal("mmdi", context.Users.First().UserName);
        }


        [Fact]
        public async Task Register_WithExistingUsername_ReturnsFalse()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "RegisterDuplicateTest")
                .Options;

            using var context = new AppDbContext(options);

            context.Users.Add(new SiteUsers {
                UserName = "mmdi",
                FirstName = "mmd",
                LastName = "so",
                Password = "123456"
               
            });
            context.SaveChanges();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RegisterViewModel, SiteUsers>();
            });
            var mapper = config.CreateMapper();

            var userService = new UserService(context, mapper);

            var registerModel = new RegisterViewModel
            {
                UserName = "mmdi",
                FirstName = "mmd",
                LastName = "so",
                Email = "mmd@gmailcom",
                Password = "123456",
                ConfirmPassword = "123456"
            };

            // Act
            var result = await userService.Register(registerModel);

            // Assert
            Assert.False(result > 0);
            Assert.Single(context.Users); 
        }
        #endregion

        #region Login

        [Fact]
        public void Login_WithValidCredential_ReturnUser()
        {
            //Arrange

            var option = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName : "LoginValidTest").Options;

            using var context = new AppDbContext(option);

            var hashedPassword = PasswordHelper.EncodePasswordMd5("123456");

            context.Users.Add(new SiteUsers
            {
                UserName = "mmdi",
                Password = hashedPassword,
                FirstName = "mmd",
                LastName = "sohrabi",
                Email = "mmd@gmail.com"
            });
            context.SaveChanges();

            var config = new MapperConfiguration(cng =>
            {
                cng.CreateMap< SiteUsers , LoginViewModel>();
            });
            var mapper = config.CreateMapper();

            var userService = new UserService(context , mapper);

            var loginViewmodel = new LoginViewModel
            {
                UserName = "mmdi",
                Password = "123456"
            };

            //Act
            var result = userService.Login(loginViewmodel);

            //Assert

            Assert.NotNull(result);
            Assert.Equal("mmdi", result.UserName);
        }

        [Fact]
        public void Login_WithInvalidCredential_ReturnNull()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "LoginInvalidTest")
                .Options;

            using var context = new AppDbContext(options);

            var hashedPassword = PasswordHelper.EncodePasswordMd5("123456");

            context.Users.Add(new SiteUsers
            {
                UserName = "mmdi",
                Password = hashedPassword,
                FirstName = "mmd",
                LastName = "sohrabi",
                Email = "mmd@gmail.com"
            });
            context.SaveChanges();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SiteUsers, LoginViewModel>();
            });
            var mapper = config.CreateMapper();

            var userService = new UserService(context, mapper);

            var loginModel = new LoginViewModel
            {
                UserName = "wrongUser",
                Password = "wrongPass"
            };

            //Act

            var result = userService.Login(loginModel);

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region GetUserNamebyUserId

        [Fact]
        public void GetUserNamebyUserId_WithExistUsername_ReTurnTrue()
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

            
        }

        #endregion
    }
}
