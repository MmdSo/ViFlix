using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Core.ViewModels.BaseModels;

namespace ViFlix.Core.ViewModels.UsersViewModels
{
    public class WishListViewModel : BaseViewModels
    {
        public long UserId { get; set; }
        public long MovieId { get; set; }
    }
}
