using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Core.ViewModels.SubscriptionViewModels;
using ViFlix.Data.Repository;
using ViFlix.Data.Subscription;

namespace ViFlix.Core.Services.Subscription.UserSubscriptionService
{
    public interface IUserSubscriptionServices : IGenericRepository<UserSubscription>
    {
        IEnumerable<UserSubscriptionViewModel> GetAllsubscriber();
        UserSubscriptionViewModel GetSubscriberById(long? id);
        Task<long> AddSubscriber(UserSubscriptionViewModel subs);
        Task EditSubscriber(UserSubscriptionViewModel subs);
        Task DeleteSubscriber(long subsId);
        Task<UserSubscriptionViewModel> GetSubscriberByUserIdAsync(long userId);
        bool IsUserSubscribed(long userId);
    }
}
