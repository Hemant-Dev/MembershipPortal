using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MembershipPortal.DTOs
{
    public  class ProductDTO
    {

        public record CreateProductDTO(
            string ProductName,
            [Required(ErrorMessage = "Price is Required")]
            [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
            decimal Price

            );

        public record UpdateProductDTO(
           long Id,
           string ProductName,
           [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
           [Required(ErrorMessage = "Price is Required")]
            decimal Price

           );


        public record GetProductDTO(
           long Id,
           string ProductName,
           decimal Price

          );

    }
}
