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
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public long UserId { get; set; }
        public long? MovieId { get; set; }
        public long? SeriesId { get; set; }
        public bool IsApproved { get; set; }
        public long? ParentReviewId { get; set; }

        #region Relations
        [ForeignKey("UserId")]
        public SiteUsers users { get; set; }

        [ForeignKey("MovieId")]
        public Movie movie { get; set; }

        [ForeignKey("SeriesId")]
        public Series series { get; set; }

        [ForeignKey("ParentReviewId")]
        public virtual Reviews? ParentReview { get; set; }

        public virtual ICollection<Reviews> Replies { get; set; } = new List<Reviews>();
        #endregion
    }
}
