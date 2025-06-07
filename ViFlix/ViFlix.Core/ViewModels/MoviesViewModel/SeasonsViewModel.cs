using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Core.ViewModels.BaseModels;
using ViFlix.Data.Movies;

namespace ViFlix.Core.ViewModels.MoviesViewModel
{
    public class SeasonsViewModel 
    {
        public int SeasonNumber { get; set; }
        public long SeriesId { get; set; }
        public string? HostUrl { get; set; }
    }
}
