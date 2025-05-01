using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Core.ViewModels.BaseModels;

namespace ViFlix.Core.ViewModels.SubscriptionViewModels
{
    public class SuscriptionPlanViewModel : BaseViewModels
    {
        public string PlanName { get; set; }
        public string Description { get; set; }
        public Decimal Price { get; set; }
        public DateTime DurationDays { get; set; }
        public bool IsActive { get; set; }
    }
}
