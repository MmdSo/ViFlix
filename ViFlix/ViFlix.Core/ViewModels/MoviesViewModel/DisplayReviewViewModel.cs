using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Core.ViewModels.BaseModels;

namespace ViFlix.Core.ViewModels.MoviesViewModel
{
    public class DisplayReviewViewModel : BaseViewModels
    {
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UserName { get; set; } 
        public string? Avatar { get; set; }
        public bool IsApproved { get; set; }
        public List<DisplayReviewViewModel> Replies { get; set; } = new List<DisplayReviewViewModel>();

    }
}
