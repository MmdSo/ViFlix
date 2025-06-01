using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Data.Context;

namespace ViFlix.Data.Movies
{
    public class Actors : BaseEntitiies
    {
        public string ActorFullName { get; set; }

        public List<MovieActors> MovieActors { get; set; }
    }
}
