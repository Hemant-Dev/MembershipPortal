using MembershipPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MembershipPortal.IRepositories
{
    public interface ISubscriberRepository : IRepository<Subscriber>
    {
        Task<IEnumerable<Subscriber>> SearchAsyncAll(string search);
        Task<(IEnumerable<Subscriber>, int)> GetAllPaginatedSubscriberAsync(int page, int pageSize, Subscriber subscriberObj);

       
    }
}
