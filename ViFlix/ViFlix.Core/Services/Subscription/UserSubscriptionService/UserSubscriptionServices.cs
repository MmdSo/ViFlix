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

namespace ViFlix.Core.Services.Subscription.UserSubscriptionService
{
    public class UserSubscriptionServices : GenericRepository<UserSubscription>, IUserSubscriptionServices
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public UserSubscriptionServices(AppDbContext context , IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<long> AddSubscriber(UserSubscriptionViewModel subs)
        {
            var sc = _mapper.Map<UserSubscriptionViewModel, UserSubscription>(subs);
            await AddEntity(sc);
            _context.SaveChanges();
            return subs.Id;
        }

        public async Task DeleteSubscriber(long subsID)
        {
            UserSubscriptionViewModel sub = GetSubscriberById(subsID);

            var sc = _mapper.Map<UserSubscriptionViewModel, UserSubscription>(sub);

            sc.IsDelete = true;

            EditEntity(sc);
            await SaveChanges();
        }

        public async Task EditSubscriber(UserSubscriptionViewModel subs)
        {
            var sc = _mapper.Map<UserSubscriptionViewModel, UserSubscription>(subs);
            EditEntity(sc);
            await SaveChanges();
        }

        public IEnumerable<UserSubscriptionViewModel> GetAllsubscriber()
        {
            var subs = _mapper.Map<IEnumerable<UserSubscription>, IEnumerable<UserSubscriptionViewModel>>(GetAll());
            return subs;
        }

        public UserSubscriptionViewModel GetSubscriberById(long? id)
        {
            var sub = _mapper.Map<UserSubscription, UserSubscriptionViewModel>(GetEntityById(id));
            return sub;
        }

        public async Task<UserSubscriptionViewModel> GetSubscriberByUserIdAsync(long userId)
        {
            var subs = _mapper.Map<IEnumerable<UserSubscription>, IEnumerable<UserSubscriptionViewModel>>(GetAll().Where(u => u.UserId == userId).ToList());
            return subs.FirstOrDefault();
        }

        public bool IsUserSubscribed(long userId)
        {
            var userSubs = GetAll(); 

            return userSubs.Any(u =>
                u.UserId == userId &&
                u.IsActive &&
                u.EndDate > DateTime.UtcNow
                );
        }
    }
}
