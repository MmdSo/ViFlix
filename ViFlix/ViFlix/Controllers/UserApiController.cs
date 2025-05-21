using AutoMapper;
using FirstShop.Core.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ViFlix.Core.Services.User.PermissionsServices;
using ViFlix.Core.Services.User.RolesServices;
using ViFlix.Core.Services.User.UserServices;
using ViFlix.Core.Tools;
using ViFlix.Core.ViewModels.UsersViewModels;
using ViFlix.Data.Users;

namespace ViFlix.Controllers
{
    #region User
    [Route("api/[controller]")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userServices;
        private readonly IConfiguration _config;

        public UserApiController(IMapper mapper, IUserService userServices , IConfiguration config)
        {
            _mapper = mapper;
            _userServices = userServices;
            _config = config;
        }

        public List<UserViewModel> userViewModel { get; set; }

        [HttpGet]
        public List<UserViewModel> GetUsers()
        {
            userViewModel = _userServices.GetAllUsers().ToList();
            return userViewModel;
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

        [Authorize]
        [HttpPost("AddUsers")]
        public async Task<long> AddUsers([FromForm]UserViewModel user )
        {
            var users = _userServices.AddUser(user);
            return await users;
        }

        [Authorize]
        [HttpPut("EditUser")]
        public async Task<IActionResult> EditUser([FromForm]UserViewModel user , long id)
        {
            var existUser = _userServices.GetUserById(id);
            string pass = PasswordHelper.EncodePasswordMd5(user.Password);
            if (existUser == null)
            {
                return NotFound("User not found !");
            }

            existUser.UserName = user.UserName;
            existUser.FirstName = user.FirstName;
            existUser.LastName = user.LastName;
            existUser.Email = user.Email;
            existUser.PhoneNumber = user.PhoneNumber;
            existUser.age = user.age;
            existUser.Password = pass;
            existUser.RoleId = user.RoleId;

           await _userServices.EditUser(existUser);

            return Ok();
        }

        [Authorize]
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

        [Authorize]
        [HttpPut("ChangeUserEmail")]
        public async Task<IActionResult> ChangeUserEmail([FromForm]ChangeEmailViewModel email)
        {
            var mail = _userServices.ChangeEmail(email);
            return Ok(mail);
        }

        [Authorize]
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
            
            if(login.Password != null && login.UserName != null)
            {
                var user = _userServices.Login(login);

                if(user == null)
                {
                    return Unauthorized();
                }
                return Ok();
            }      
                return Unauthorized("username and password dosnt mathch !");
        }

        [HttpPost("LoginJWT")]
        public IActionResult LoginJWT([FromQuery] LoginViewModel login)
        {
            string pass = PasswordHelper.EncodePasswordMd5(login.Password);
            if (login.UserName != null && pass != null)
            {
                var user = _userServices.Login(login);

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name ,login.UserName)
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(60),
                    Issuer = _config["JwtSettings:Issuer"],
                    Audience = _config["JwtSettings:Audience"],
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return Ok(new { Token = tokenString });
            }

            return Unauthorized("username and password dosnt mathch !");
        }

        [Authorize]
        [HttpGet("UserProfilById")]
        public ActionResult<ProfileViewModel> UserProfilById(long id)
        {
            var prof = _userServices.GetUserByIdProfile(id);
            return prof;
        }

        [Authorize]
        [HttpGet("GetUserByIdPasswordFromApi/{id}")]
        public ActionResult<ChangePasswordViewModel> GetUserByIdPasswordFromApi([FromForm] long id)
        {
            var pass = _userServices.GetUserByIdChangePaswword(id);
            return Ok(pass);
        }

        [Authorize]
        [HttpGet("GetUserByIdEmailFromApi/{id}")]
        public ActionResult<ChangeEmailViewModel> GetUserByIdEmailFromApi([FromForm] long id)
        {
            var mail = _userServices.GetUserByIdChangeEmail(id);
            return Ok(mail);
        }

        [Authorize]
        [HttpGet("GetUserIdByUsernameFromApi")]
        public ActionResult<UserViewModel> GetUserIdByUsernameFromApi([FromForm] string name)
        {
            var User = _userServices.GetUserIdByUserName(name);
            return Ok(name);
        }

        [Authorize]
        [HttpPut("EditProfile")]
        public async Task<IActionResult> EditProfile([FromForm] ProfileViewModel prof, long id, IFormFile AvatarImg)
        {
            var user = _userServices.GetUserById(id);
            if(user == null)
            {
                return NotFound("User not found!");
            }
            
            user.age = prof.age;
            user.PhoneNumber = prof.PhoneNumber;
            user.LastName = prof.LastName;
            user.FirstName = prof.FirstName;

            string fileName = NameGenerator.GenerateUniqCode() + Path.GetExtension(AvatarImg.FileName);
            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Images/Avatar", fileName);

            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await AvatarImg.CopyToAsync(stream);
            }


            prof.avatar = "/Images/Avatar" + fileName;

            var us = _mapper.Map<UserViewModel, ProfileViewModel>(user);
            await _userServices.EditProfile(us, AvatarImg);

            return Ok();
        }

        [Authorize]
        [HttpPost("AddRoleIdToUserFromApi")]
        public IActionResult AddRoleIdToUserFromApi([FromBody] List<long> roleId, [FromQuery] long userId)
        {
            if (roleId == null || !roleId.Any())
                return BadRequest("Dont get any Role!");

            _userServices.AddRoleToUser(roleId, userId);

            return Ok();
        }

        [Authorize]
        [HttpGet("GetUserRolesByUserIdFromApi")]
        public async Task<IActionResult> GetUserRolesByUserIdFromApi([FromForm]long id)
        {
            var user = _userServices.GetUserRolesByUserID(id);

            return Ok(user);
        }
    }
    #endregion

    #region Roles
    [Route("api/[controller]")]
    [ApiController]
    public class RoleApiController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRoleServices _roleServices;

        public RoleApiController(IMapper mapper, IRoleServices roleServices)
        {
            _mapper = mapper;
            _roleServices = roleServices;
        }

        public List<RoleViewModel> roleList { get; set; }

        [HttpGet]
        public List<RoleViewModel> GetRoles()
        {
            roleList = _roleServices.GetAllRoles().ToList();
            return roleList;
        }

        [HttpGet("{id}")]
        public ActionResult<RoleViewModel> GetRoleById(long id)
        {
            var role = _roleServices.GetRoleById(id);
            if (role == null)
                return NotFound(role);
            else
                return Ok(role);
        }

        [HttpPost]
        public long AddRole(long id)
        {
            return id;
        }

        [Authorize]
        [HttpPost("AddRole")]
        public async Task<long> AddRole([FromForm]RoleViewModel role)
        {
            return await _roleServices.AddRole(role);
            
        }

        [Authorize]
        [HttpPut("EditRoles")]
        public async Task<IActionResult> EditRoles(long id , [FromForm] RoleViewModel role)
        {
            var ro = _roleServices.GetRoleById(id);
            if(ro == null)
            {
                return NotFound("Role not Found!");
            }
            await _roleServices.EditRole(role);

            return Ok();
        }

        [Authorize]
        [HttpDelete("DeleteRole")]
        public async Task<IActionResult> DeleteRole(long id)
        {
            var ro = _roleServices.GetRoleById(id);

            if (ro == null)
            {
                return NotFound(new { message = " Role Not found !" });
            }
            await _roleServices.DeleteRole(id);

            return Ok();
        }

        [Authorize]
        [HttpGet("GetRolePermissinsByApi")]
        public async Task<IActionResult> GetRolePermissinsByApi([FromQuery]long roleId)
        {
            if(roleId == null)
            {
                return NotFound("Role not found!");
            }

             _roleServices.GetRolePermissions(roleId);

            return Ok(roleId);
        }

        [Authorize]
        [HttpPost("AddPermissionToRoleByApi")]
        public IActionResult AddRolePermissionByApi(long roleId, [FromQuery] List<long> permissions)
        {
            if (permissions == null || !permissions.Any())
                return BadRequest("Dont get any Permissions!");

            _roleServices.AddPermissionsToRole(roleId, permissions);

            return Ok();
        }

        [Authorize]
        [HttpPut("UpdatePermissionToRoleByApi")]
        public IActionResult UpdatePermissionToRoleByApi(long roleId, [FromQuery] List<long> permissions)
        {
            if (permissions == null || !permissions.Any())
                return BadRequest("Dont get any Permissions!");

            _roleServices.UpdatePermissionsRole(roleId, permissions);

            return Ok();
        }

        [Authorize]
        [HttpGet("CheckUserPermission")]
        public IActionResult CheckUserPermission([FromQuery] long permissionId, [FromQuery] string userName)
        {
            if (string.IsNullOrEmpty(userName))
                return BadRequest("Please Write an UserName !");

            bool hasPermission = _roleServices.CheckPermission(permissionId, userName);
            if (hasPermission)
            {
                return Ok();
            }
            else
            {
                return BadRequest("You dont have a permission !");
            }
        }
    }
    #endregion

    #region Permission
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionApiController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPermissionServices _permissionServices;

        public PermissionApiController (IMapper mapper , IPermissionServices permissionServices)
        {
            _mapper = mapper;
            _permissionServices = permissionServices;
        }

        public List<PermissionViewModel> permissionList { get; set; }

        [HttpGet]
        public List<PermissionViewModel> GetPermissions()
        {
            permissionList = _permissionServices.GetAllPermission().ToList();
            return permissionList;
        }

        [HttpGet("{id}")]
        public ActionResult<PermissionViewModel> GetPermissionById(long id)
        {
            var permission = _permissionServices.GetPermissionById(id);
            if (permission == null)
                return NotFound(permission);
            else
                return Ok(permission);
        }

        [HttpGet("CheckUserPermission")]
        public IActionResult CheckUserPermission([FromQuery] int permissionId, [FromQuery] string userName)
        {
            if (string.IsNullOrEmpty(userName))
                return BadRequest("Please Write an UserName !");

            bool hasPermission = _permissionServices.CheckPermission(permissionId, userName);
            if (hasPermission)
            {
                return Ok();
            }
            else
            {
                return BadRequest("You dont have a permission !");
            }
        }
    }
    #endregion
}
