using MembershipPortal.DTOs;
using MembershipPortal.IRepositories;
using MembershipPortal.IServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MembershipPortal.Models;

namespace MembershipPortal.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;

        public SubscriptionService(ISubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }

        public async Task<GetSubscriptionDTO> CreateSubscriptionAsync(CreateSubscriptionDTO createSubscriptionDTO)
        {
            try
            {
                var subscription = await _subscriptionRepository.CreateSubscriptionAsync(createSubscriptionDTO);

                var subscriptionDTO = new GetSubscriptionDTO
                    (
                        subscription.Id, subscription.SubscriberId, subscription.ProductId, subscription.ProductName, subscription.ProductPrice,
                        subscription.DiscountId, subscription.DiscountCode, subscription.DiscountAmount, subscription.StartDate, subscription.ExpiryDate,
                        subscription.PriceAfterDiscount, subscription.TaxId, subscription.CGST, subscription.SGST, subscription.TotalTaxPercentage, subscription.TaxAmount,
                        subscription.FinalAmount
                    );
                return subscriptionDTO;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<GetSubscriptionDTO> UpdateSubscriptionAsync( long Id ,UpdateSubscriptionDTO updateSubscriptionDTO)
        {

            try
            {
                var oldSubscription = await _subscriptionRepository.GetAsyncById(Id);

                if (oldSubscription != null)
                {

                    var subscription = await _subscriptionRepository.UpdateSubscriptionAsync(Id, updateSubscriptionDTO);
                    var subscriptionDTO = new GetSubscriptionDTO
                        (

                            subscription.Id, subscription.SubscriberId, subscription.ProductId, subscription.ProductName, subscription.ProductPrice,
                            subscription.DiscountId, subscription.DiscountCode, subscription.DiscountAmount, subscription.StartDate, subscription.ExpiryDate,
                            subscription.PriceAfterDiscount, subscription.TaxId, subscription.CGST, subscription.SGST, subscription.TotalTaxPercentage, subscription.TaxAmount,
                            subscription.FinalAmount
                        );
                    return subscriptionDTO;
                }
            }
            catch (Exception ex)
            {
                // Console.WriteLine(ex.ToString());
                throw;
               
            }
            return null;
        }

        public async Task<bool> DeleteSubscriptionByIdAsync(long id)
        {
            try
            {
                var subscription = await _subscriptionRepository.GetAsyncById(id);

                if (subscription != null)
                {
                     var result = await _subscriptionRepository.DeleteAsync(subscription);
                    return result;
                }
                return false;
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.ToString());
                throw;
            }

        }

        public async Task<IEnumerable<GetSubscriptionDTO>> GetAllSubscriptionForeignAsync()
        {
            try
            {
                var getSubscriptionDTOList = await _subscriptionRepository.GetAllSubscriptionForeignAsync();

                if(getSubscriptionDTOList != null)
                {
                    return getSubscriptionDTOList;
                }
                
            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return null;
        }

        public async Task<GetSubscriptionDTO> GetSubscriptionByIdAsync(long id)
        {
            try
            {
                var subscription = await _subscriptionRepository.GetAsyncById(id);
                
                if (subscription != null)
                {
                    var subscriptionDTO = new GetSubscriptionDTO
                    (
                        subscription.Id, subscription.SubscriberId, subscription.ProductId, subscription.ProductName, subscription.ProductPrice,
                        subscription.DiscountId, subscription.DiscountCode, subscription.DiscountAmount, subscription.StartDate, subscription.ExpiryDate,
                        subscription.PriceAfterDiscount, subscription.TaxId, subscription.CGST, subscription.SGST, subscription.TotalTaxPercentage, subscription.TaxAmount,
                        subscription.FinalAmount
                    );
                    return subscriptionDTO;
                }
            }
            catch (Exception ex)
            {

                // Console.WriteLine(ex.ToString());
                throw;
            }
            return null;
        }

        public async Task<IEnumerable<GetSubscriptionDTO>> GetAllSubscriptionSearchAsync(string filter)
        {
            try
            {
                var subscriptionList = await _subscriptionRepository.GetAllSearchSubscriptionsAsync(filter);
                if (subscriptionList != null)
                {
                    var subscriptionDTOList = subscriptionList.Select(obj =>
                        new GetSubscriptionDTO(
                                obj.Id, obj.SubscriberId, obj.ProductId, obj.ProductName, obj.ProductPrice,
                                obj.DiscountId, obj.DiscountCode, obj.DiscountAmount, obj.StartDate, obj.ExpiryDate, obj.PriceAfterDiscount,
                                obj.TaxId, obj.CGST, obj.SGST, obj.TotalTaxPercentage, obj.TaxAmount, obj.FinalAmount
                            )
                    );

                    return subscriptionDTOList;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return null;
        }

        public async Task<IEnumerable<GetSubscriptionDTO>> GetAllSubscriptionAdvanceSearchAsync(GetSubscriptionDTO subscriptionDTO)
        {
            try
            {
                var subscriptionModel = new Subscription()
                {
                    Id = subscriptionDTO.Id,
                    SubscriberId = subscriptionDTO.SubscriberId,
                    ProductId = subscriptionDTO.ProductId,
                    ProductName = subscriptionDTO.ProductName,
                    ProductPrice = subscriptionDTO.ProductPrice,
                    StartDate = subscriptionDTO.StartDate,
                    ExpiryDate = subscriptionDTO.ExpiryDate,
                    DiscountId = subscriptionDTO.DiscountId,
                    DiscountCode = subscriptionDTO.DiscountCode,
                    DiscountAmount = subscriptionDTO.DiscountAmount,
                    PriceAfterDiscount = subscriptionDTO.PriceAfterDiscount,
                    TaxId = subscriptionDTO.TaxId,
                    CGST = subscriptionDTO.CGST,
                    SGST = subscriptionDTO.SGST,
                    TotalTaxPercentage = subscriptionDTO.TotalTaxPercentage,
                    TaxAmount = subscriptionDTO.TaxAmount,
                    FinalAmount = subscriptionDTO.FinalAmount,
                };
                var subscriptionList = await _subscriptionRepository.GetAllAdvanceSearchSubscriptionsAsync(subscriptionModel);

                if (subscriptionList != null)
                {
                    var subscriptionDTOList = subscriptionList.Select(obj => new GetSubscriptionDTO(
                            obj.Id, obj.SubscriberId, obj.ProductId, obj.ProductName,
                            obj.ProductPrice, obj.DiscountId, obj.DiscountCode, obj.DiscountAmount,
                            obj.StartDate, obj.ExpiryDate, obj.PriceAfterDiscount, obj.TaxId,
                            obj.CGST, obj.SGST, obj.TotalTaxPercentage, obj.TaxAmount, obj.FinalAmount
                        ));

                    return subscriptionDTOList;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return null;
        }

        public async Task<IEnumerable<GetSubscriptionDTO>> GetAllSortedSubscriptions(string? sortColumn, string? sortOrder)
        {
            try
            {
                var sortedSubscriptionsList = await _subscriptionRepository.GetAllSortedSubscriptions(sortColumn, sortOrder);
                if (sortedSubscriptionsList != null)
                {
                    var sortedSubscriptionsDTOList = sortedSubscriptionsList
                        .Select(subscriptions => new GetSubscriptionDTO(
                                subscriptions.Id, subscriptions.SubscriberId, subscriptions.ProductId, subscriptions.ProductName, subscriptions.ProductPrice, subscriptions.DiscountId, subscriptions.DiscountCode, subscriptions.DiscountAmount, subscriptions.StartDate, subscriptions.ExpiryDate, subscriptions.PriceAfterDiscount, subscriptions.TaxId, subscriptions.CGST, subscriptions.SGST, subscriptions.TotalTaxPercentage, subscriptions.TaxAmount, subscriptions.FinalAmount
                            )
                    ).ToList();
                    return sortedSubscriptionsDTOList;
                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
