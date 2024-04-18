using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MembershipPortal.DTOs
{
    public record CreateSubscriberDTO(
                                        [Required(ErrorMessage = "Please enter first name")] [MaxLength(50)] string FirstName,
                                        [MaxLength(50)] string LastName,
                                        [Required(ErrorMessage = "Please enter Customer number")] string ContactNumber,
                                        [MaxLength(50)] [Required(ErrorMessage = "Please enter Customer Email")] string Email,
                                        [Required(ErrorMessage = "Please select valid gender")] long GenderId
        );

    public record UpdateSubscriberDTO(   long Id,
                                        [Required(ErrorMessage = "Please enter first name")][MaxLength(50)] string FirstName,
                                        [MaxLength(50)] string LastName,
                                        [Required(ErrorMessage = "Please enter Customer number")] string ContactNumber,
                                        [MaxLength(50)][Required(ErrorMessage = "Please enter Customer Email")] string Email,
                                        [Required(ErrorMessage = "Please select valid gender")] long GenderId
        );

    public record GetSubscriberDTO(     long Id,
                                        string FirstName,
                                        string LastName,
                                        string ContactNumber,
                                        string Email,
                                        long GenderId,
                                        string GenderName
        );

}

