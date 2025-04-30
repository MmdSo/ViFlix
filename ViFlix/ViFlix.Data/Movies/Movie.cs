using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Data.Context;

namespace ViFlix.Data.Movies
{
    public class Movie : BaseEntitiies
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Duration { get; set; }
        public long LanguageId { get; set; }
        public long GanreId { get; set; }
        public int Rating { get; set; }
        public string? Poster { get; set; }
        public string? DownloadLink { get; set; }
        public string? Trailer { get; set; }

        #region Relations 
        [ForeignKey("LanguageId")]
        public Language language { get; set; }

        [ForeignKey("GanreId")]
        public List<Ganres> ganre { get; set; }

        public List<Reviews> review { get; set; }
        #endregion
    }
}
