using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Core.ViewModels.BaseModels;

namespace ViFlix.Core.ViewModels.MoviesViewModel
{
    public class SeriesViewModel : BaseViewModels
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string? Poster { get; set; }
        public DateTime ReleaseDate { get; set; }
        public bool IsCompleted { get; set; }
        public string? Link { get; set; }
        public string? Trailer { get; set; }
        public long? GanreId { get; set; }
        public string? GanreTitle { get; set; }
        public string? LanguageTitle { get; set; }
        public long? LanguageId { get; set; }
        public long? SeasonsId { get; set; }
    }
}
