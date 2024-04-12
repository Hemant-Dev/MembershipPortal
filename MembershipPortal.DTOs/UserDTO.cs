using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MembershipPortal.DTOs
{
    public  class UserDTO
    {
        
        public record CreateUserDTO (
            [MaxLength(50)]
            [Required(ErrorMessage = "FirstName is Required")]
            string FirstName,
            [MaxLength(50)]
            [Required(ErrorMessage = "LastName is Required")]
            string LastName,
            [MaxLength(100)]
            [Required(ErrorMessage ="Email is Required")]
            string Email,
            [MaxLength(50)]
            [Required(ErrorMessage = "Password is Required")]
            string Password,
            string ContactNumber
            );

        public record UpdateUserDTO(
            
            long Id,
            [MaxLength(50)]
            [Required(ErrorMessage = "FirstName is Required")]
            string FirstName,
            [MaxLength(50)]
            [Required(ErrorMessage = "LastName is Required")]
            string LastName,
            [MaxLength(100)]
            [Required(ErrorMessage ="Email is Required")]
            string Email,
            [MaxLength(50)]
            [Required(ErrorMessage = "Password is Required")]
            string Password,
            string ContactNumber
            );

        public record GetUserDTO(
            long Id,
            string FirstName,
            string LastName,
            string Email,
            string Password,

            string ContactNumber

            );

    }
}
