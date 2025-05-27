using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Core.ViewModels.SubscriptionViewModels;
using ViFlix.Data.Repository;
using ViFlix.Data.Subscription;

namespace ViFlix.Core.Services.Subscription.SubscriptionPlanService
{
    public interface IsubscriptionPlanServices : IGenericRepository<SubscriptionPlan>
    {
        IEnumerable<SuscriptionPlanViewModel> GetAllPlans();
        SuscriptionPlanViewModel GetPlanById(long? id);
        Task<long> AddPlan(SuscriptionPlanViewModel plan);
        Task EditPlans(SuscriptionPlanViewModel plan);
        Task DeletePlans(long planId);
    }
}
