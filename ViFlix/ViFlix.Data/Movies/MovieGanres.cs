using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Data.Context;

namespace ViFlix.Data.Movies
{
    public class MovieGanres: BaseEntitiies
    {
        public long GanresId { get; set; }
        public long MovieId { get; set; }

        # region Relations
        [ForeignKey("MovieId")]
        public Movie Movie { get; set; }

        [ForeignKey("GanresId")]
        public Ganres Ganre { get; set; }
    }
        #endregion
    
}
