using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Core.ViewModels.BaseModels;

namespace ViFlix.Core.ViewModels.UsersViewModels
{
    public class UserViewModel : BaseViewModels
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? age { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? avatar { get; set; }
        public long RoleId { get; set; }
    }
}
