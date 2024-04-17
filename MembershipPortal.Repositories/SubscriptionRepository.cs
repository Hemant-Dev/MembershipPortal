using MembershipPortal.Data;
using MembershipPortal.DTOs;
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
    public class SubscriptionRepository : Repository<Subscription>, ISubscriptionRepository
    {
        private readonly MembershipPortalDbContext _dbContext;
        public SubscriptionRepository(MembershipPortalDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<GetSubscriptionDTO>> GetAllSubscriptionForeignAsync()
        {
            var getSubscription = await _dbContext.Subscriptions
                .Include(subscriber => subscriber.Subscriber)
                .Include(product => product.Product)
                .Include(discount => discount.Discount)
                .Include(tax => tax.Tax)
                .ToListAsync();


            var subscriptionDTOList = getSubscription.Select(
                subscription =>
                        new GetSubscriptionDTO(subscription.Id,
                                               subscription.SubscriberId,
                                               subscription.ProductId,
                                               subscription.Product.ProductName,
                                               subscription.Product.Price,
                                               subscription.DiscountId,
                                               subscription.Discount.DiscountCode,
                                               subscription.DiscountAmount,
                                               subscription.StartDate,
                                               subscription.ExpiryDate,
                                               subscription.PriceAfterDiscount,
                                               subscription.TaxId,
                                               subscription.Tax.CGST,
                                               subscription.Tax.SGST,
                                               subscription.Tax.TotalTax,
                                               subscription.TaxAmount,
                                               subscription.FinalAmount));

            return subscriptionDTOList;
        }

        public async Task<Subscription> CreateSubscriptionAsync(CreateSubscriptionDTO createSubscriptionDTO)
        {
            long discountId = createSubscriptionDTO.DiscountId == 0 ? 1 : createSubscriptionDTO.DiscountId;
            var product = await _dbContext.Products.FindAsync(createSubscriptionDTO.ProductId);

            
            var discount = await _dbContext.Discounts.FindAsync(discountId);
            var tax = await _dbContext.Taxes.FindAsync(createSubscriptionDTO.TaxId);

            decimal discountAmount = 0;
            decimal priceAfterDiscount = 0;

            if(discount != null)
            {
                if (discount.IsDiscountInPercentage)
                {
                    discountAmount = product.Price * discount.DiscountAmount / 100;
                }
                else
                {
                    discountAmount = discount.DiscountAmount;
                }
            }
            if (discountAmount < product.Price)
            {
                priceAfterDiscount = product.Price - discountAmount;
            }


            var taxAmount = priceAfterDiscount * tax.TotalTax / 100;

            var finalAmount = priceAfterDiscount + taxAmount;

            var subscription = new Subscription()
            {
                SubscriberId = createSubscriptionDTO.SubscriberId,
                ProductId = createSubscriptionDTO.ProductId,
                ProductName = product.ProductName,
                ProductPrice = product.Price,
                DiscountId = discountId,
                DiscountCode = discount.DiscountCode,
                DiscountAmount = discountAmount,
                StartDate = createSubscriptionDTO.StartDate,
                ExpiryDate = createSubscriptionDTO.ExpiryDate,
                PriceAfterDiscount = priceAfterDiscount,
                TaxId = createSubscriptionDTO.TaxId,
                SGST = tax.SGST,
                CGST = tax.CGST,
                TotalTaxPercentage = tax.TotalTax,
                TaxAmount = taxAmount,
                FinalAmount = finalAmount
            };

             await _dbContext.Subscriptions.AddAsync(subscription);
            
            
            await _dbContext.SaveChangesAsync();

            return subscription;

        }



        public async Task<Subscription> UpdateSubscriptionAsync(long Id ,UpdateSubscriptionDTO updateSubscriptionDTO)
        {
            var discount = await _dbContext.Discounts.FindAsync(updateSubscriptionDTO.DiscountId);
            var oldSubscription = await _dbContext.Subscriptions.FindAsync(Id);
            var product = await _dbContext.Products.FindAsync(updateSubscriptionDTO.ProductId);
            

            decimal discountAmount = 0;
            decimal priceAfterDiscount = 0;
            decimal taxAmount = 0;
            decimal finalAmount = 0;

            if (oldSubscription != null)
            {
                oldSubscription.SubscriberId = updateSubscriptionDTO.SubscriberId;
                if(oldSubscription.ProductId != updateSubscriptionDTO.ProductId)
                {
                    oldSubscription.ProductId = updateSubscriptionDTO.ProductId;
                    oldSubscription.ProductName = product.ProductName;
                    oldSubscription.ProductPrice = product.Price;

                    priceAfterDiscount = reCalculatingDiscount(oldSubscription, updateSubscriptionDTO, discount, product);
                    taxAmount = reCalculatingTax(oldSubscription);
                }
                else if(oldSubscription.DiscountId != updateSubscriptionDTO.DiscountId)
                {
                    priceAfterDiscount = reCalculatingDiscount(oldSubscription, updateSubscriptionDTO, discount, product);
                    taxAmount = reCalculatingTax(oldSubscription);
                }

                finalAmount = oldSubscription.PriceAfterDiscount + oldSubscription.TaxAmount;

                oldSubscription.FinalAmount = finalAmount;

                var result =  _dbContext.Subscriptions.Update(oldSubscription);
                await _dbContext.SaveChangesAsync();


                var subscription = new Subscription()
                {   
                    Id = oldSubscription.Id,
                    SubscriberId = oldSubscription.SubscriberId,
                    ProductId = oldSubscription.ProductId,
                    ProductName = oldSubscription.ProductName,
                    ProductPrice = oldSubscription.ProductPrice,
                    DiscountId = oldSubscription.DiscountId,
                    DiscountCode = oldSubscription.DiscountCode,
                    DiscountAmount = oldSubscription.DiscountAmount,
                    StartDate = oldSubscription.StartDate,
                    ExpiryDate = oldSubscription.ExpiryDate,
                    PriceAfterDiscount = oldSubscription.PriceAfterDiscount,
                    TaxId = oldSubscription.TaxId,
                    SGST = oldSubscription.SGST,
                    CGST = oldSubscription.CGST,
                    TotalTaxPercentage = oldSubscription.TotalTaxPercentage,
                    TaxAmount = oldSubscription.TaxAmount,
                    FinalAmount = oldSubscription.FinalAmount
                };
                return subscription;
            }

            return null;
        }



        private decimal reCalculatingDiscount(Subscription oldSubscription, UpdateSubscriptionDTO updateSubscriptionDTO,Discount discount, Product product)
        {
            if (discount == null) {
                oldSubscription.DiscountAmount = 0;
                oldSubscription.DiscountCode = "";
                oldSubscription.DiscountId = updateSubscriptionDTO.DiscountId;
                oldSubscription.PriceAfterDiscount = product.Price;
                return product.Price;
            }
            decimal discountAmount = 0;
            decimal priceAfterDiscount = 0;
            oldSubscription.DiscountId = updateSubscriptionDTO.DiscountId;
            oldSubscription.DiscountCode = discount.DiscountCode;
            // oldSubscription.DiscountAmount = discount.DiscountAmount;


            if (discount.IsDiscountInPercentage)
            {
                discountAmount = product.Price * discount.DiscountAmount / 100;

            }
            else
            {
                discountAmount = discount.DiscountAmount;
            }
            if (discountAmount >=  product.Price)
            {
                oldSubscription.DiscountAmount = 0;
                oldSubscription.PriceAfterDiscount = 0;
                return oldSubscription.PriceAfterDiscount;
            }
           

            oldSubscription.DiscountAmount = discountAmount;

            priceAfterDiscount = product.Price - discountAmount;

            oldSubscription.PriceAfterDiscount = priceAfterDiscount;

            return priceAfterDiscount;
        }

        private decimal reCalculatingTax(Subscription oldSubscription)
        {
            decimal taxAmount = 0;
            taxAmount = oldSubscription.PriceAfterDiscount * oldSubscription.TotalTaxPercentage / 100;

            oldSubscription.TaxAmount = taxAmount;
            return taxAmount;

        }

        public async Task<IEnumerable<Subscription>> GetAllSearchSubscriptionsAsync(string filter)
        {
            string keyword = filter.ToLower();

            var filterlist = await _dbContext.Subscriptions
                .Include(entity => entity.SubscriberId)
                .Include(entity => entity.TaxId)
                .Include(entity => entity.DiscountId)
                .ToListAsync();

            filterlist = filterlist.Where(
                                    m => m.CGST.ToString().Contains(keyword) ||
                                    m.SGST.ToString().ToLower().Contains(keyword) ||
                                    m.TotalTaxPercentage.ToString().Contains(keyword) ||
                                    m.Subscriber.FirstName.ToLower().Contains(keyword) ||
                                    m.Subscriber.LastName.ToLower().Contains(keyword) ||
                                    m.Product.ProductName.ToLower().Contains(keyword) || 
                                    m.Product.Price.ToString().Contains(keyword) ||
                                    m.Discount.DiscountAmount.ToString().Contains(keyword) ||
                                    m.DiscountCode.ToLower().Contains(keyword) ||
                                    m.StartDate.ToString().ToLower().Contains(keyword) ||
                                    m.ExpiryDate.ToString().ToLower().Contains(keyword) ||
                                    m.PriceAfterDiscount.ToString().Contains(keyword) ||
                                    m.FinalAmount.ToString().Contains(keyword) ||
                                    m.DiscountAmount.ToString().Contains(keyword) ||
                                    m.TaxAmount.ToString().Contains(keyword)
                                    )
                                    .ToList();

            return filterlist;
        }

        public async Task<IEnumerable<Subscription>> GetAllAdvanceSearchSubscriptionsAsync(Subscription subscriptionObj)
        {
            var query = _dbContext.Subscriptions.AsQueryable();

            if (!string.IsNullOrWhiteSpace(subscriptionObj.ProductName))
            {
                query = query.Where(subscription => subscription.ProductName == subscriptionObj.ProductName);
            }
            if (!string.IsNullOrWhiteSpace(subscriptionObj.ProductPrice.ToString()))
            {
                query = query.Where(subscription => subscription.ProductPrice == subscriptionObj.ProductPrice);
            }
            if (!string.IsNullOrWhiteSpace(subscriptionObj.DiscountCode))
            {
                query = query.Where(subscription => subscription.DiscountCode == subscriptionObj.DiscountCode);
            }
            if (!string.IsNullOrWhiteSpace(subscriptionObj.DiscountAmount.ToString()))
            {
                query = query.Where(subscription => subscription.DiscountAmount == subscriptionObj.DiscountAmount);
            }
            if (!string.IsNullOrWhiteSpace(subscriptionObj.StartDate.ToString()))
            {
                query = query.Where(subscription => subscription.StartDate == subscriptionObj.StartDate);
            }
            if (!string.IsNullOrWhiteSpace(subscriptionObj.ExpiryDate.ToString()))
            {
                query = query.Where(subscription => subscription.ExpiryDate == subscriptionObj.ExpiryDate);
            }
            if (!string.IsNullOrWhiteSpace(subscriptionObj.TaxAmount.ToString()))
            {
                query = query.Where(subscription => subscription.TaxAmount == subscriptionObj.TaxAmount);
            }
            if (!string.IsNullOrWhiteSpace(subscriptionObj.SGST.ToString()))
            {
                query = query.Where(subscription => subscription.SGST == subscriptionObj.SGST);
            } 
            if (!string.IsNullOrWhiteSpace(subscriptionObj.CGST.ToString()))
            {
                query = query.Where(subscription => subscription.CGST == subscriptionObj.CGST);
            }
            if (!string.IsNullOrWhiteSpace(subscriptionObj.TotalTaxPercentage.ToString()))
            {
                query = query.Where(subscription => subscription.TotalTaxPercentage == subscriptionObj.TotalTaxPercentage);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Subscription>> GetAllSortedSubscriptions(string? sortColumn, string? sortOrder)
        {
            IQueryable<Subscription> query = _dbContext.Subscriptions;
            if (!string.IsNullOrWhiteSpace(sortColumn) && !string.IsNullOrWhiteSpace(sortOrder))
            {
                // Determine the sort order based on sortOrder parameter
                bool isAscending = sortOrder.ToLower() == "asc";
                switch (sortColumn.ToLower())
                {
                    case "productname":
                        query = isAscending ? query.OrderBy(s => s.ProductName) : query.OrderByDescending(s => s.ProductName);
                        break;
                    case "productprice":
                        query = isAscending ? query.OrderBy(s => s.ProductPrice) : query.OrderByDescending(s => s.ProductPrice);
                        break;
                    case "discountcode":
                        query = isAscending ? query.OrderBy(s => s.DiscountCode) : query.OrderByDescending(s => s.DiscountCode);
                        break;
                    case "discountamount":
                        query = isAscending ? query.OrderBy(s => s.DiscountAmount) : query.OrderByDescending(s => s.DiscountAmount);
                        break;
                    case "priceafterdiscount":
                        query = isAscending ? query.OrderBy(s => s.PriceAfterDiscount) : query.OrderByDescending(s => s.PriceAfterDiscount);
                        break;
                    case "cgst":
                        query = isAscending ? query.OrderBy(s => s.CGST) : query.OrderByDescending(s => s.CGST);
                        break;
                    case "sgst":
                        query = isAscending ? query.OrderBy(s => s.SGST) : query.OrderByDescending(s => s.SGST);
                        break;
                    case "totaltaxpercentage":
                        query = isAscending ? query.OrderBy(s => s.TotalTaxPercentage) : query.OrderByDescending(s => s.TotalTaxPercentage);
                        break;
                    case "taxamount":
                        query = isAscending ? query.OrderBy(s => s.TaxAmount) : query.OrderByDescending(s => s.TaxAmount);
                        break;
                    case "finalamount":
                        query = isAscending ? query.OrderBy(s => s.FinalAmount) : query.OrderByDescending(s => s.FinalAmount);
                        break;
                    case "startdate":
                        query = isAscending ? query.OrderBy(s => s.StartDate) : query.OrderByDescending(s => s.StartDate);
                        break;
                    case "expirydate":
                        query = isAscending ? query.OrderBy(s => s.ExpiryDate) : query.OrderByDescending(s => s.ExpiryDate);
                        break;
                    default:
                        query = query.OrderBy(s => s.Id);
                        break;
                }

            }

            return await query.ToListAsync();
        }
    }
}
