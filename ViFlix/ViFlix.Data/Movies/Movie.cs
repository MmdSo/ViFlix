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
        public string ReleaseDate { get; set; }
        public string Duration { get; set; }
        public long LanguageId { get; set; }
        public long ActorsId { get; set; }
        public long GanreId { get; set; }
        public long DirectorId { get; set; }
        public int Rating { get; set; }
        public string? Poster { get; set; }
        public string? Trailer { get; set; }
        public string? Country { get; set; }
        public bool? IsDubed { get; set; }

        #region Relations 
        public List<Language> language { get; set; }
        public List<Director> director { get; set; }
        public List<Actors> Actors { get; set; }

        public List<MovieGanres> moviesGanres { get; set; }

        public List<Reviews> review { get; set; }
        public ICollection<DownloadLink> DownloadLinks { get; set; } = new List<DownloadLink>();
        #endregion
    }
}
