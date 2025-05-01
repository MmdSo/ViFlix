using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Core.ViewModels.BaseModels;

namespace ViFlix.Core.ViewModels.UsersViewModels
{
    public class ForgetPasswordViewModel :BaseViewModels
    {
        public string UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
    }
}
