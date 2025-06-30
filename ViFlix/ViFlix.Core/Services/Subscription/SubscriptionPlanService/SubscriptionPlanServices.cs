using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Core.ViewModels.SubscriptionViewModels;
using ViFlix.Data.Context;
using ViFlix.Data.Repository;
using ViFlix.Data.Subscription;

namespace ViFlix.Core.Services.Subscription.SubscriptionPlanService
{
    public class SubscriptionPlanServices : GenericRepository<SubscriptionPlan>, IsubscriptionPlanServices
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public SubscriptionPlanServices(AppDbContext context , IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<long> AddPlan(SuscriptionPlanViewModel plan)
        {
            var sp = _mapper.Map<SuscriptionPlanViewModel, SubscriptionPlan>(plan);
            await AddEntity(sp);
            await SaveChanges();
            return plan.Id;
        }

        public bool CheckIfPlanExists(long id)
        {
            return GetAll().Any(p => p.Id == id);
        }

        public async Task DeletePlans(long planId)
        {
            SuscriptionPlanViewModel plan = GetPlanById(planId);

            var sb = _mapper.Map<SuscriptionPlanViewModel, SubscriptionPlan>(plan);

            sb.IsDelete = true;

            EditEntity(sb);
            await SaveChanges();
        }

        public async Task EditPlans(SuscriptionPlanViewModel plan)
        {
            var sp = _mapper.Map<SuscriptionPlanViewModel, SubscriptionPlan>(plan);
            EditEntity(sp);
            await SaveChanges();
        }

        public IEnumerable<SuscriptionPlanViewModel> GetActivePlans()
        {
            var plan = _mapper.Map<IEnumerable<SubscriptionPlan> , IEnumerable<SuscriptionPlanViewModel>>(GetAll().Where(p => p.IsActive).ToList());
            return plan;
        }

        public IEnumerable<SuscriptionPlanViewModel> GetAllPlans()
        {
            var plans = _mapper.Map<IEnumerable<SubscriptionPlan>, IEnumerable<SuscriptionPlanViewModel>>(GetAll());
            return plans;
        }

        public SuscriptionPlanViewModel GetPlanById(long? id)
        {
            var plan = _mapper.Map<SubscriptionPlan, SuscriptionPlanViewModel>(GetEntityById(id));
            return plan;
        }
    }
}
