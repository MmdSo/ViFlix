using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Core.ViewModels.BaseModels;

namespace ViFlix.Core.ViewModels.MoviesViewModel
{
    public class MovieViewModel : BaseViewModels
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ReleaseDate { get; set; }
        public string Duration { get; set; }
        public long LanguageId { get; set; }
        public long GanreId { get; set; }
        public string? GanreTitle { get; set; }
        public string? LanguageTitle { get; set; }
        public int Rating { get; set; }
        public string? Poster { get; set; }
        public string? DownloadLink { get; set; }
        public string? Trailer { get; set; }
        public string? Cast { get; set; }
        public string? Cuntry { get; set; }
        public string? Director { get; set; }
        public bool? IsDubed { get; set; }

        public List<DownloadLinksViewModel> DownloadLinks { get; set; }
    }
}
