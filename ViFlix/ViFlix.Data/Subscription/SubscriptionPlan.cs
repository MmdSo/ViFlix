using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Data.Context;

namespace ViFlix.Data.Subscription
{
    public class SubscriptionPlan : BaseEntitiies
    {
        public string PlanName { get; set; }
        public string Description { get; set; }
        public Decimal Price { get; set; }
        public DateTime DurationDays { get; set; }
        public bool IsActive { get; set; }

        #region relations
        public List<UserSubscription> userSubs { get; set; }
        #endregion
    }
}
