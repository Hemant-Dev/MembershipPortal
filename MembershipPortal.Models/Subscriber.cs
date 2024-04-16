using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MembershipPortal.Models
{
    [Table("Subscribers")]
    public class Subscriber : SubscriberWithGenderViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required(ErrorMessage = "Please enter first name")]
        [MaxLength(50)]                      
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }


        [Required(ErrorMessage = "Please enter Customer number")]
        public string ContactNumber { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessage = "Please enter Customer Email")]
        public string Email { get; set; }

        [ForeignKey("Gender")]
        [Required(ErrorMessage ="Please select valid gender")]
        public long GenderId { get; set;}

        public virtual Gender Gender { get; set; }
    }
}
