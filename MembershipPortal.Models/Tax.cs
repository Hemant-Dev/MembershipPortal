using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mail;
using System.Reflection.Metadata.Ecma335;

namespace MembershipPortal.Models
{
    [Table("Taxes")]
    //[Index(nameof(TaxName), IsUnique = true)]
    public class Tax
    {
        [Required(ErrorMessage = "Tax Id is required")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Required(ErrorMessage = "Tax Name is required")]
        public string TaxName { get; set; }
        [Required(ErrorMessage = "SGST is required")]
        public decimal SGST { get; set; }
        [Required(ErrorMessage = "CGST is required")]
        public decimal CGST { get; set; }
        public decimal TotalTax { get; set; }
    }
}
