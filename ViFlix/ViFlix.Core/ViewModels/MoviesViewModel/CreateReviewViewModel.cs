using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Core.ViewModels.BaseModels;

namespace ViFlix.Core.ViewModels.MoviesViewModel
{
    public class CreateReviewViewModel : BaseViewModels
    {
        public string Comment { get; set; }
        public long? MovieId { get; set; }
        public long? SeriesId { get; set; }
        public long? ParentReviewId { get; set; }
    }
}
