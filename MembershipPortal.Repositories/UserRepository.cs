using MembershipPortal.Data;
using MembershipPortal.IRepositories;
using MembershipPortal.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MembershipPortal.DTOs.UserDTO;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MembershipPortal.Repositories
{
    public class UserRepository :Repository<User>, IUserRepository
    {

        private readonly MembershipPortalDbContext _dbContext;
        public UserRepository(MembershipPortalDbContext dbContext) : base(dbContext) { 
       
            _dbContext = dbContext;
        }

       

        public async Task<IEnumerable<User>> GetUserSearchAsync(string find)
        {
            string keyword = find.ToLower();

            var filterlist = await _dbContext.Users
               .Where(m => m.FirstName.ToLower().Contains(keyword) ||
                m.LastName.ToLower().Contains(keyword) ||
                m.ContactNumber.Contains(keyword) ||
                m.Email.ToLower().Contains(keyword))
                .ToListAsync();

            return filterlist;
        }

        public async Task<IEnumerable<User>> GetUserAdvanceSearchAsync(User userobj)
        {
            var query = _dbContext.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(userobj.FirstName))
            {
                query = query.Where(user => userobj.FirstName == user.FirstName);
            }
            if (!string.IsNullOrWhiteSpace(userobj.LastName))
            {
                query = query.Where(user => userobj.LastName == user.LastName);
            }
            if (!string.IsNullOrWhiteSpace(userobj.ContactNumber))
            {
                query = query.Where(user => userobj.ContactNumber == user.ContactNumber);
            }
            if (!string.IsNullOrWhiteSpace(userobj.Email))
            {
                query = query.Where(user => userobj.Email == user.Email);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAllSortedUser(string? sortColumn, string? sortOrder) 
        {
            IQueryable<User> query = _dbContext.Users;
            if (!string.IsNullOrWhiteSpace(sortColumn) && !string.IsNullOrWhiteSpace(sortOrder))
            {
                // Determine the sort order based on sortOrder parameter
                bool isAscending = sortOrder.ToLower() == "asc";
                switch (sortColumn.ToLower())
                {
                    case "firstname":
                        query = isAscending ? query.OrderBy(s => s.FirstName) : query.OrderByDescending(s => s.FirstName);
                        break;
                    case "lastname":
                        query = isAscending ? query.OrderBy(s => s.LastName) : query.OrderByDescending(s => s.LastName);
                        break;
                    case "email":
                        query = isAscending ? query.OrderBy(s => s.Email) : query.OrderByDescending(s => s.Email);
                        break;
                    case "password":
                        query = isAscending ? query.OrderBy(s => s.Password) : query.OrderByDescending(s => s.Password);
                        break;
                    case "contactnumber":
                        query = isAscending ? query.OrderBy(s => s.ContactNumber) : query.OrderByDescending(s => s.ContactNumber);
                        break;
                    default:
                        query = query.OrderBy(s => s.Id);
                        break;
                }

            }

            return await query.ToListAsync();
       
        }

        public async Task<(IEnumerable<User>, int)> GetAllPaginatedUserAsync(int page, int pageSize, User userObj)
        {
            var query = _dbContext.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(userObj.FirstName))
            {
                query = query.Where(user => user.FirstName.Contains(userObj.FirstName));
            }
            if (!string.IsNullOrWhiteSpace(userObj.LastName))
            {
                query = query.Where(user => user.LastName.Contains(userObj.LastName));
            }
            if (!string.IsNullOrWhiteSpace(userObj.ContactNumber))
            {
                query = query.Where(user => user.ContactNumber.Contains(userObj.ContactNumber));
            }
            if (!string.IsNullOrWhiteSpace(userObj.Email))
            {
                query = query.Where(user => user.Email.Contains(userObj.Email));
            }

            int totalCount = query.Count();
            int totalPages = (int)(Math.Ceiling((decimal)totalCount / pageSize));

            query = query.Skip((page - 1) * pageSize).Take(pageSize);
            return (await query.ToListAsync(), totalPages);
        }
    }
}
