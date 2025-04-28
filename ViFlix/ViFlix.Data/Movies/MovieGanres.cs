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
        public long GanreId { get; set; }
        public long MovieId { get; set; }

        # region Relations
        [ForeignKey("MovieId")]
        public List<Movie> movie { get; set; }

        [ForeignKey("GanreId")]
        public List<Ganres> ganres { get; set; }
        #endregion
    }
}
