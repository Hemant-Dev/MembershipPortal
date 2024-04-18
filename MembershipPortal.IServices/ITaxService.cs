using MembershipPortal.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MembershipPortal.DTOs.ProductDTO;

namespace MembershipPortal.IServices
{
    public interface ITaxService
    {
        Task<IEnumerable<GetTaxDTO>> GetTaxesAsync();
        Task<GetTaxDTO> GetTaxByIdAsync(long id);
        Task<GetTaxDTO> CreateTaxAsync(CreateTaxDTO taxDTO);
        Task<GetTaxDTO> UpdateTaxAsync(long Id, UpdateTaxDTO taxDTO);
        Task<bool> DeleteTaxAsync(long Id);

        Task<IEnumerable<GetTaxDTO>> GetAllSortedTax(string? sortColumn, string? sortOrder);
    }
}
