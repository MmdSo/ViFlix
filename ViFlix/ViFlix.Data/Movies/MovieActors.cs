using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Data.Context;

namespace ViFlix.Data.Movies
{
    public class MovieActors :BaseEntitiies
    {
        public long ActorId { get; set; }
        public long MovieId { get; set; }

        # region Relations
        [ForeignKey("MovieId")]
        public Movie Movie { get; set; }

        [ForeignKey("ActorId")]
        public Actors Actor { get; set; }
        #endregion
    }
}
