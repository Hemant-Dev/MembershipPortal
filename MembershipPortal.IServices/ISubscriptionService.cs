﻿using MembershipPortal.DTOs;
using MembershipPortal.IRepositories;
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
    }
}
