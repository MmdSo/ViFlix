using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Core.Services.User.UserServices;
using ViFlix.Data.Context;
using ViFlix.Data.Users;
using Xunit;

namespace ViflixUnitTests
{
    public class UserServiceTest
    {
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
    }
}
