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
        public DateTime ReleaseDate { get; set; }
        public bool IsCompleted { get; set; } 
        public string? Link { get; set; }
        public string? Trailer { get; set; }

        #region Relations
        public List<Reviews> reviews { get; set; }
        public List<Ganres> ganres { get; set; }
        public List<Seasons> season { get; set; }

        #endregion
    }
}
