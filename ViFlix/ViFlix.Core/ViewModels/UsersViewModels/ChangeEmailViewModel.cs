using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Core.ViewModels.BaseModels;

namespace ViFlix.Core.ViewModels.UsersViewModels
{
    public class ChangeEmailViewModel : BaseViewModels
    {
        public string CurrentEmail { get; set; }
        public string NewEmail { get; set; }
    }
}
