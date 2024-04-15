using MembershipPortal.DTOs;
using MembershipPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MembershipPortal.IRepositories
{
    public interface ISubscriptionRepository : IRepository<Subscription>
    {
        Task<IEnumerable<GetSubscriptionDTO>> GetAllSubscriptionForeignAsync();

        Task<Subscription> CreateSubscriptionAsync(CreateSubscriptionDTO subscriptionDTO);

        Task<Subscription> UpdateSubscriptionAsync(long Id, UpdateSubscriptionDTO updateSubscriptionDTO);

        Task<IEnumerable<Subscription>> GetAllSearchSubscriptionsAsync(string filter);
        Task<IEnumerable<Subscription>> GetAllAdvanceSearchSubscriptionsAsync(Subscription subscriptionObj);
    }
}
