using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Core.ViewModels.BaseModels;

namespace ViFlix.Core.ViewModels.SubscriptionViewModels
{
    public class UserSubscriptionViewModel : BaseViewModels
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public long UserId { get; set; }
        public long PlanId { get; set; }
    }
}
