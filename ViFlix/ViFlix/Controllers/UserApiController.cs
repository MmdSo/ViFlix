using AutoMapper;
using FirstShop.Core.Security;
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
            var existUser = _userServices.GetUserById(id);
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
            existUser.Password = user.Password;

           await _userServices.EditUser(existUser);

            return Ok();
        }

        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            var us = _userServices.GetUserById(id);

            if (us == null)
            {
                return NotFound(new { message = "User Not found !" });
            }

            await _userServices.DeleteUser(id);

            return Ok();
        }

        [HttpPut("ChangeUserEmail")]
        public async Task<IActionResult> ChangeUserEmail([FromForm]ChangeEmailViewModel email)
        {
            var mail = _userServices.ChangeEmail(email);
            return Ok(mail);
        }

        [HttpPut("ChangeUserPassword")]
        public async Task<IActionResult> ChangeUserPassord([FromForm] ChangePasswordViewModel pass)
        {
            var password = _userServices.ChangePassword(pass);
            return Ok(password);
        }

        [HttpPost("UserRegister")]
        public async Task<IActionResult> UserRegister([FromForm] RegisterViewModel register)
        {
            if(_userServices.UserNameExist(register.UserName))
            {
                return BadRequest("This username is already taken");
            }

            var user = await _userServices.Register(register);
            return Ok(user);
        }

        [HttpPost("UserLogin")]
        public IActionResult UserLogin([FromForm] LoginViewModel login)
        {
            string pass = PasswordHelper.EncodePasswordMd5(login.Password);
            if(login.Password != null && login.UserName != null)
            {
                var user = _userServices.Login(login);

                if(user == null)
                {
                    return Unauthorized();
                }
            }      
                return Unauthorized("username and password dosnt mathch !");
        }

        [HttpGet("UserProfilById")]
        public ActionResult<ProfileViewModel> UserProfilById(long id)
        {
            var prof = _userServices.GetUserByIdProfile(id);
            return prof;
        }

        [HttpGet("GetUserByIdPasswordFromApi/{id}")]
        public ActionResult<ChangePasswordViewModel> GetUserByIdPasswordFromApi(long id)
        {
            var pass = _userServices.GetUserByIdChangePaswword(id);
            return Ok(pass);
        }

        [HttpGet("GetUserByIdEmailFromApi/{id}")]
        public ActionResult<ChangeEmailViewModel> GetUserByIdEmailFromApi(long id)
        {
            var mail = _userServices.GetUserByIdChangeEmail(id);
            return Ok(mail);
        }
    }
    #endregion
}
