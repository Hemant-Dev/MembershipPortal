using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;

namespace MembershipPortal.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessage = "FirstName is Required")]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(50)]
        [Required(ErrorMessage = "LastName is Required")]
        public string LastName { get; set; } = string.Empty;


        [Required(ErrorMessage ="Email is Required")]

        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is Required")]
        [MaxLength(50)]
        public string Password { get; set; } = string.Empty;

        public string ContactNumber { get; set; } = string.Empty;


    }
}
