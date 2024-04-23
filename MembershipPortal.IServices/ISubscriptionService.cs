using MembershipPortal.DTOs;
using MembershipPortal.IRepositories;
using MembershipPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MembershipPortal.IServices
{
    public interface ISubscriptionService
    {
        public Task<IEnumerable<GetSubscriptionDTO>> GetAllSubscriptionForeignAsync();
        public Task<GetSubscriptionDTO> GetSubscriptionByIdAsync(long id);
        public Task<bool> DeleteSubscriptionByIdAsync(long id);
        public Task<GetSubscriptionDTO> CreateSubscriptionAsync(CreateSubscriptionDTO createSubscriptionDTO);

        public Task<GetSubscriptionDTO> UpdateSubscriptionAsync(long Id, UpdateSubscriptionDTO updateSubscriptionDTO);
        public Task<IEnumerable<GetSubscriptionDTO>> GetAllSubscriptionSearchAsync(string filter);
        public Task<IEnumerable<GetSubscriptionDTO>> GetAllSubscriptionAdvanceSearchAsync(GetSubscriptionDTO subscriptionDTO);
        public Task<IEnumerable<GetSubscriptionDTO>> GetAllSortedSubscriptions(string? sortColumn, string? sortOrder);

        public Task<(IEnumerable<GetSubscriptionDTO>, int)> GetAllPaginatedSubscriptionAsync(string? sortColumn, string? sortOrder, int page, int pageSize, Subscription subscription);
    }
}
