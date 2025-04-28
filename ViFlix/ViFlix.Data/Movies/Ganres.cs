using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Data.Context;

namespace ViFlix.Data.Movies
{
    public class Ganres : BaseEntitiies
    {
        public string Title { get; set; }

        #region Relations 
        public List<Movie> movies { get; set; }
        #endregion
    }
}
