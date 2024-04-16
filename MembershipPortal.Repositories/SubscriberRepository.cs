using MembershipPortal.Data;
using MembershipPortal.IRepositories;
using MembershipPortal.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public async Task<IEnumerable<Subscriber>> GetAllSubscriberDataAsync()
        {
            var subscriberList = await _dbContext.Subscribers
                .Include(s => s.Gender)
                .Select(subscriber =>
                    new Subscriber()
                    {
                        Id = subscriber.Id,
                        FirstName = subscriber.FirstName,
                        LastName = subscriber.LastName,
                        ContactNumber = subscriber.ContactNumber,
                        Email = subscriber.Email,
                        GenderId = subscriber.GenderId,
                        GenderName = subscriber.Gender.GenderName
                    }

                )
                .ToListAsync();
            return subscriberList != null ? subscriberList : null;
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
    }
}
