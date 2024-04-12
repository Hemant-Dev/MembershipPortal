using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MembershipPortal.DTOs
{
    class InitialValueAccess()
    {
        long _DiscountId = 1;
    }
    public record CreateSubscriptionDTO(
        
            [Required(ErrorMessage = "Please enter valid subscriber Id")] long SubscriberId,
            [Required(ErrorMessage = "Please enter valid product Id")] long ProductId,
            long DiscountId,
            long TaxId,
            DateTime StartDate,
            DateTime ExpiryDate

        );
    //[Required(ErrorMessage = "Please enter valid Start Date")] 
    //[Required(ErrorMessage = "Please enter valid Expiry Date")]
    public record UpdateSubscriptionDTO(
            [Required(ErrorMessage ="Please enter subscriptionId")] long Id,
           // [Required(ErrorMessage = "Please enter valid subscriber Id")] long SubscriberId,
            [Required(ErrorMessage = "Please enter valid product Id")] long ProductId,
            long DiscountId,
            [Required(ErrorMessage = "Please enter valid Start Date")] DateTime StartDate,
            [Required(ErrorMessage = "Please enter valid Expiry Date")] DateTime ExpiryDate
        );

    public record GetSubscriptionDTO(
            long Id, long SubscriberId, 
            long ProductId, string ProductName,
            decimal ProductPrice, long DiscountId, 
            string DiscountCode, decimal DiscountAmount,
            DateTime StartDate, DateTime ExpiryDate,
            decimal PriceAfterDiscount,long TaxId, 
            decimal CGST, decimal SGST, 
            decimal TotalTaxPercentage, decimal TaxAmount,
            decimal FinalAmount
        );
}
