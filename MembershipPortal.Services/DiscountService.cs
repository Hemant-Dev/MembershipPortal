﻿using MembershipPortal.DTOs;
using MembershipPortal.IRepositories;
using MembershipPortal.IServices;
using MembershipPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace MembershipPortal.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IDiscountRepository _discountRepository;
        public DiscountService(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }
        public async Task<GetDiscountDTO> CreateDiscountAsync(CreateDiscountDTO discountDTO)
        {
            try
            {
                if (discountDTO == null) throw new ArgumentNullException(nameof(discountDTO));
                var discount = new Discount()
                {
                    DiscountCode = discountDTO.DiscountCode,
                    DiscountAmount = discountDTO.DiscountAmount,
                    IsDiscountInPercentage = discountDTO.IsDiscountInPercentage
                };
                var result = await _discountRepository.CreateAsync(discount);
                var newDiscoutDTO = new GetDiscountDTO(result.Id, result.DiscountCode, result.DiscountAmount, result.IsDiscountInPercentage);
                return newDiscoutDTO;
            }
            catch (Exception ex)
            {
               // Console.WriteLine($"Error occurred in CreateDiscountAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteDiscountAsync(long Id)
        {
            try
            {
                if (Id < 0) throw new ArgumentOutOfRangeException(nameof(Id));
                var discount = await _discountRepository.GetAsyncById(Id);
                return await _discountRepository.DeleteAsync(discount);
            }
            catch (Exception ex)
            {
               // Console.WriteLine($"Error occurred in DeleteDiscountAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<GetDiscountDTO> GetDiscountByIdAsync(long id)
        {
            try
            {
                var discount = await _discountRepository.GetAsyncById(id);
                if (discount != null)
                {
                    return new GetDiscountDTO(discount.Id, discount.DiscountCode, discount.DiscountAmount, discount.IsDiscountInPercentage);
                }
            }
            catch (Exception ex)
            {
               // Console.WriteLine($"Error occurred in GetDiscountByIdAsync: {ex.Message}");
                throw;
            }
                return null;
        }

        public async Task<IEnumerable<GetDiscountDTO>> GetDiscountsAsync()
        {
            try
            {
                var discountList = await _discountRepository.GetAsyncAll();
                if (discountList != null)
                {
                    var discountDTOList = discountList.Select(discount => new GetDiscountDTO(discount.Id, discount.DiscountCode, discount.DiscountAmount, discount.IsDiscountInPercentage));
                    return discountDTOList;
                }
                return null;
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Error occurred in GetDiscountsAsync: {ex.Message}");

                throw;
            }
        }

        public async Task<GetDiscountDTO> UpdateDiscountAsync(long Id, UpdateDiscountDTO discountDTO)
        {
            try
            {
                var oldDiscount = await _discountRepository.GetAsyncById(Id);
                if (oldDiscount != null)
                {
                    oldDiscount.DiscountCode = discountDTO.DiscountCode;
                    oldDiscount.DiscountAmount = discountDTO.DiscountAmount;
                    oldDiscount.IsDiscountInPercentage = discountDTO.IsDiscountInPercentage;
                    var discount = await _discountRepository.UpdateAsync(oldDiscount);
                    return new GetDiscountDTO(discount.Id, discount.DiscountCode, discount.DiscountAmount, discount.IsDiscountInPercentage);
                }
                return null;
            }
            catch (Exception ex)
            {
               // Console.WriteLine($"Error occurred in UpdateDiscountAsync: {ex.Message}");

                throw;
            }
        }

      
    }
}
