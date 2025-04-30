using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Data.Context;
using ViFlix.Data.Users;

namespace ViFlix.Data.Subscription
{
    public class UserSubscription : BaseEntitiies
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public long UserId { get; set; }
        public long PlanId { get; set; }

        #region Relation 
        [ForeignKey("UserId")]
        public SiteUsers user { get; set; }

        [ForeignKey("PlanId")]
        public SubscriptionPlan plan { get; set; }
        #endregion
    }
}
