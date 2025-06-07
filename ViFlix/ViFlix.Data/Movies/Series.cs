using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Data.Context;

namespace ViFlix.Data.Movies
{
    public class Series : BaseEntitiies
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string? Poster { get; set; }  
        public string ReleaseDate { get; set; }
        public bool IsCompleted { get; set; } 
        public string? Link { get; set; }
        public string? Trailer { get; set; }
        public long? GanreId { get; set; }
        public long? LanguageId { get; set; }
        public long? SeasonsId { get; set; }
        public string? ActorsId { get; set; }
        public string? Country { get; set; }
        public string? DirectorId { get; set; }
        public bool? IsDubed { get; set; }

        #region Relations
        public ICollection<Reviews> Reviews { get; set; } = new List<Reviews>();
        public List<Ganres> ganre { get; set; }
        public List<Language> Language { get; set; }
        public ICollection<Seasons> Seasons { get; set; } = new List<Seasons>();

        #endregion
    }
}
