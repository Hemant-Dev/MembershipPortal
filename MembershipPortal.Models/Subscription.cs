using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MembershipPortal.Models
{
    [Table("Subscriptions")]
    public class Subscription
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [ForeignKey(nameof(Subscriber))]
        [Required (ErrorMessage = "Please enter valid subscriber Id")]
        public long SubscriberId { get; set; } 
        public virtual Subscriber Subscriber { get; set; }

        [ForeignKey(nameof(Product))]
        [Required(ErrorMessage = "Please enter valid product Id")]
        public long ProductId { get; set; }
        public virtual Product Product{ get; set; }

        public string ProductName { get; set; }

        public decimal ProductPrice { get; set; }

        [ForeignKey(nameof(Discount))]
        public long DiscountId { get; set; } = 1;
        public virtual Discount Discount { get; set; }
        public string DiscountCode { get; set; }
        public decimal DiscountAmount { get; set; }

        [Required(ErrorMessage = "Please enter valid Start Date")]
        public DateOnly StartDate { get; set; }

        [Required(ErrorMessage = "Please enter valid expiry Date")]
        public DateOnly ExpiryDate { get; set; }

        public decimal PriceAfterDiscount { get; set; }

        [ForeignKey(nameof(Tax))]
        public long TaxId { get; set; }
        public virtual Tax Tax { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }

        public decimal TotalTaxPercentage { get; set; }

        public decimal TaxAmount { get; set; }

        public decimal FinalAmount { get; set; }

    }
}
