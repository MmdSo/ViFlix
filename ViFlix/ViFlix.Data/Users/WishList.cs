using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Data.Context;
using ViFlix.Data.Movies;

namespace ViFlix.Data.Users
{
    public class WishList : BaseEntitiies
    {
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
