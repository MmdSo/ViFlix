using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ViFlix.Core.Services.User.UserServices;
using ViFlix.Core.Tools;
using ViFlix.Core.ViewModels.UsersViewModels;

namespace ViFlix.Controllers
{
    #region User
    [Route("api/[controller]")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userServices;

        public UserApiController(IMapper mapper, IUserService userServices)
        {
            _mapper = mapper;
            _userServices = userServices;
        }

        public List<UserViewModel> userViewModel { get; set; }

        [HttpGet]
        public List<UserViewModel> GetUsers()
        {
            var users = _userServices.GetAllUsers().ToList();
            return users;
        }

        [HttpGet("{Id}")]
        public ActionResult<UserViewModel> GetUserById(long Id)
        {
            var user = _userServices.GetUserById(Id);
            if(user == null)
                return NotFound(user);
            else
                return Ok(user);
        }

        [HttpPost]
        public long AddUserFromApi(long id)
        {
            return id;
        }

        [HttpPost("AddUsers")]
        public async Task<long> AddUsers([FromForm]UserViewModel user )
        {
            

            var users = _userServices.AddUser(user);
            return await users;
        }

        [HttpPut("EditUser")]
        public async Task<IActionResult> EditUser([FromForm]UserViewModel user , long id)
        {
            var existUser = _userServices.GetEntityById(id);
            if(existUser == null)
            {
                return NotFound("User not found !");
            }

            existUser.UserName = user.UserName;
            existUser.FirstName = user.FirstName;
            existUser.LastName = user.LastName;
            existUser.Email = user.Email;
            existUser.PhoneNumber = user.PhoneNumber;
            existUser.age = user.age;
            existUser.role = existUser.role;
            existUser.Password = user.Password;

            await _userServices.EditUser(existUser);

            return Ok();
        }
    }
    #endregion
}
