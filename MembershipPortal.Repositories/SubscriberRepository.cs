using MembershipPortal.Data;
using MembershipPortal.IRepositories;
using MembershipPortal.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MembershipPortal.Repositories
{
    public class SubscriberRepository : Repository<Subscriber>, ISubscriberRepository
    {
        private readonly MembershipPortalDbContext _dbContext;

        public SubscriberRepository(MembershipPortalDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

       
        public async Task<IEnumerable<Subscriber>> SearchAsyncAll(string search)
        {
            var keyword = search.ToLower();

            var subscriber = await _dbContext.Subscribers
                .Where(subscriber => subscriber.FirstName.ToLower() == keyword || 
                                     subscriber.LastName.ToLower() == keyword ||
                                     subscriber.Email.ToLower() == keyword ||
                                     subscriber.ContactNumber.ToLower() == keyword || 
                                     subscriber.Gender.GenderName.ToLower() == keyword)
                .ToListAsync();

            return subscriber;
        }

        public async Task<(IEnumerable<Subscriber>, int)> GetAllPaginatedSubscriberAsync(int page, int pageSize, Subscriber subscriberObj)
        {
            var query = _dbContext.Subscribers.AsQueryable();



            if (!string.IsNullOrWhiteSpace(subscriberObj.FirstName))
            {
                query = query.Where(subscriber => subscriber.FirstName.Contains(subscriberObj.FirstName));
            }
            if (!string.IsNullOrWhiteSpace(subscriberObj.LastName))
            {
                query = query.Where(subscriber => subscriber.LastName.Contains(subscriber.LastName));
            }
            if (!string.IsNullOrWhiteSpace(subscriberObj.Email))
            {
                query = query.Where(subscriber => subscriber.Email.Contains(subscriber.Email));
            }
            if (!string.IsNullOrWhiteSpace(subscriberObj.ContactNumber))
            {
                query = query.Where(subscriber => subscriber.ContactNumber.Contains(subscriber.ContactNumber));
            }

            int totalCount = query.Count();
            int totalPages = (int)(Math.Ceiling((decimal)totalCount / pageSize));

            query = query.Skip((page - 1) * pageSize).Take(pageSize);
            return (await query.ToListAsync(), totalPages);
        }

    }



}
