using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Data.Context;
using ViFlix.Data.Users;

namespace ViFlix.Data.Movies
{
    public class Reviews : BaseEntitiies
    {
        public string Comment { get; set; }
        public long UserId { get; set; }
        public long MovieId { get; set; }

        #region Relations
        [ForeignKey("UserId")]
        public SiteUsers users { get; set; }

        [ForeignKey("MovieId")]
        public Movie movie { get; set; }
        #endregion
    }
}
