using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Data.Context;

namespace ViFlix.Data.Movies
{
    public class Seasons : BaseEntitiies
    {
        public int SeasonNumber { get; set; }
        public long SeriesId { get; set; }
        public string? HostUrl { get; set; }
        

        #region Relations
        [ForeignKey("SeriesId")]
        public Series serie { get; set; }
        #endregion
    }
}
