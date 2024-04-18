using MembershipPortal.Data;
using MembershipPortal.DTOs;
using MembershipPortal.IRepositories;
using MembershipPortal.IServices;
using MembershipPortal.Models;
using NuGet.Protocol.Plugins;
using System.Reflection;
using static MembershipPortal.DTOs.ProductDTO;

namespace MembershipPortal.Services
{
    public class GenderService : IGenderService
    {
        private readonly IGenderRepository _genderRepository;
        
        public GenderService(IGenderRepository genderRepository)
        {
            _genderRepository = genderRepository;
        }
        public async Task<GetGenderDTO> CreateGenderAsync(CreateGenderDTO genderDTO)
        {
            try
            {
                var gender =  await _genderRepository.CreateAsync(new Gender()
                {
                    GenderName = genderDTO.GenderName
                });

                var genderDto = new GetGenderDTO(gender.Id, gender.GenderName);

                return genderDto;
            }catch(Exception ex)
            {
                //Console.WriteLine($"Error occurred in CreateGenderAsync: {ex.Message}");
                throw;
            }
            return null;
        }

        public async Task<bool> DeleteGenderAsync(long id)
        {
            try
            {
                var gender = await _genderRepository.GetAsyncById(id);

                if(gender != null)
                {
                    return await _genderRepository.DeleteAsync(gender);
                }
            }catch(Exception ex)
            {
                // Console.WriteLine($"Error occurred in DeleteGenderAsync: {ex.Message}");
                throw;
            }
            return false;
        }

       

        public async Task<GetGenderDTO> GetGenderAsync(long id)
        {
            try
            {
                var gender = await _genderRepository.GetAsyncById(id);

                if(gender != null)
                {
                    var genderDto = new GetGenderDTO(gender.Id, gender.GenderName);
                    return genderDto;
                }

                
            }catch(Exception ex)
            {
                // Console.WriteLine($"Error occurred in GetGenderAsync: {ex.Message}");
                throw;
            }
            return null;
        }

        public async Task<IEnumerable<GetGenderDTO>> GetGendersAsync()
        {
            try
            {
                var genders = await _genderRepository.GetAsyncAll();

                var genderDto = genders.Select(gender => new GetGenderDTO(

                    gender.Id,
                    gender.GenderName
                ));

                return genderDto;
                
            }catch(Exception ex)
            {
                //Console.WriteLine($"Error occurred in GetGendersAsync: {ex.Message}");
                throw;
            }
            return null;
        }

        public async Task<IEnumerable<GetGenderDTO>> SearchGendersAsync(string search)
        {
            try
            {
                var genders = await _genderRepository.SearchAsyncAll(search);

                if(genders != null)
                {
                    var genderDto = genders.Select(gender => new GetGenderDTO(

                                gender.Id,
                                gender.GenderName
                    ));

                    return genderDto;
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return null;
        }

        public async Task<GetGenderDTO> UpdateGenderAsync(long id, UpdateGenderDTO genderDTO)
        {
            try
            {
                var oldGender = await _genderRepository.GetAsyncById(id);

                if(oldGender != null)
                {
                    oldGender.GenderName = genderDTO.GenderName;

                    await _genderRepository.UpdateAsync(oldGender); 

                   return new GetGenderDTO(oldGender.Id, oldGender.GenderName);
                }
            }catch(Exception ex)
            {
                //Console.WriteLine($"Error occurred in UpdateGenderAsync: {ex.Message}");
                throw;
            }
            return null;
        }

        public async  Task<(IEnumerable<GetGenderDTO>, int)> GetAllPaginatedGenderAsync(int page, int pageSize, Gender gender)
        {
            var genderListAndTotalPages = await _genderRepository.GetAllPaginatedGenderAsync(page, pageSize, gender);
            var genderDTOList = genderListAndTotalPages.Item1.Select(gender =>

                    new GetGenderDTO(
                            gender.Id,
                            gender.GenderName
                            
                        )
                ).ToList();
            return (genderDTOList, genderListAndTotalPages.Item2);
        }
    }
}
