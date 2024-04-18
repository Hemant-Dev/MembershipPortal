using MembershipPortal.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MembershipPortal.IServices
{
    public interface IGenderService
    {
        Task<GetGenderDTO> GetGenderAsync(long id);

        Task<IEnumerable<GetGenderDTO>> GetGendersAsync();
        Task<GetGenderDTO> UpdateGenderAsync(long id, UpdateGenderDTO genderDTO);
        Task<GetGenderDTO> CreateGenderAsync(CreateGenderDTO genderDTO);

        Task<bool> DeleteGenderAsync(long id);

        Task<IEnumerable<GetGenderDTO>> SearchGendersAsync(string search);
        Task<IEnumerable<GetGenderDTO>> GetAllSortedGender(string? sortColumn, string? sortOrder);
    }
}
