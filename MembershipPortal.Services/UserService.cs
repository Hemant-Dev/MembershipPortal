using MembershipPortal.DTOs;
using MembershipPortal.IRepositories;
using MembershipPortal.IServices;
using MembershipPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MembershipPortal.DTOs.ProductDTO;
using static MembershipPortal.DTOs.UserDTO;

namespace MembershipPortal.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository repository)
        {
            userRepository = repository;
        }

        public async Task<GetUserDTO> CreateUserAsync(CreateUserDTO createUserDTO)
        {
            try
            {
                var user = await userRepository.CreateAsync(new User()
                {

                    FirstName = createUserDTO.FirstName,
                    LastName = createUserDTO.LastName,
                    Email = createUserDTO.Email,
                    ContactNumber = createUserDTO.ContactNumber,
                    Password = createUserDTO.Password,
                });

                return new GetUserDTO(
                    user.Id,
                    user.FirstName,
                    user.LastName,
                    user.Email,
                    user.Password,
                    user.ContactNumber
                    );
            }
            catch (Exception ex)
            {

                //Console.WriteLine(ex.Message);
                throw;

            }
        }

        public async Task<bool> DeleteUserAsync(long Id)
        {
            try
            {
                var user = await userRepository.GetAsyncById(Id);

                if (user != null)
                {
                    await userRepository.DeleteAsync(user);
                    return true;

                }
                return false;

            }
            catch (Exception)
            {
                // Console.WriteLine(ex.Message);
                throw;

            }
        }


        public async Task<GetUserDTO> GetUserAsync(long Id)
        {
            try
            {

                var user = await userRepository.GetAsyncById(Id);
                if (user != null)
                {
                    return new GetUserDTO(
                         user.Id,
                         user.FirstName,
                         user.LastName,
                         user.Email,
                         user.Password,
                         user.ContactNumber
                         );
                }
                return null;
            }
            catch (Exception)
            {

                //Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<GetUserDTO>> GetUsersAsync()
        {
            try
            {
                var users = await userRepository.GetAsyncAll();

                var userDto = users.Select(user => new GetUserDTO(
                             user.Id,
                             user.FirstName,
                             user.LastName,
                             user.Email,
                             user.Password,
                             user.ContactNumber
                             ));
                return userDto;
            }
            catch (Exception ex)
            {

                //  Console.WriteLine(ex.Message);
                throw;
            }
        }



        public async Task<GetUserDTO> UpdateUserAsync(long id, UpdateUserDTO updateUserDTO)
        {
            try
            {
                var oldUser = await userRepository.GetAsyncById(id);

                if (oldUser != null)
                {

                    oldUser.FirstName = updateUserDTO.FirstName;
                    oldUser.LastName = updateUserDTO.LastName;
                    oldUser.ContactNumber = updateUserDTO.ContactNumber;
                    oldUser.Email = updateUserDTO.Email;
                    oldUser.Password = updateUserDTO.Password;

                    var result = await userRepository.UpdateAsync(oldUser);

                    return new GetUserDTO(oldUser.Id,
                                 oldUser.FirstName,
                                 oldUser.LastName,
                                 oldUser.Email,
                                 oldUser.Password,
                                 oldUser.ContactNumber);

                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred in UpdateUserAsync: {ex.Message}");
                throw;
            }
        }
        public async Task<IEnumerable<GetUserDTO>> GetUserSearchAsync(string find)
        {
            var userList = await userRepository.GetUserSearchAsync(find);

            var userDto = userList.Select(
                user => new GetUserDTO(
                    user.Id,
                    user.FirstName,
                    user.LastName,
                    user.Email,
                    user.Password,
                    user.ContactNumber
                ));

            return userDto;
        }


        public async Task<IEnumerable<GetUserDTO>> GetUserAdvanceSearchAsync(GetUserDTO getUserDTO)
        {
            try
            {
                var userList = await userRepository.GetUserAdvanceSearchAsync(new User()
                {
                    Id = getUserDTO.Id,
                    FirstName = getUserDTO.FirstName,
                    LastName = getUserDTO.LastName,
                    ContactNumber = getUserDTO.ContactNumber,
                    Email = getUserDTO.Email,
                    Password = getUserDTO.Password
                  
                });


                var userDto = userList.Select(
                    user => new GetUserDTO(
                        user.Id,
                        user.FirstName,
                        user.LastName,
                        user.Email,
                        user.Password,
                        user.ContactNumber
                        ));
                return userDto;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }


        public async Task<IEnumerable<GetUserDTO>> GetAllSortedUsers(string? sortColumn, string? sortOrder)
        {
            try
            {
                var sortedUsersList = await userRepository.GetAllSortedUser(sortColumn, sortOrder);
                if (sortedUsersList != null)
                {
                    var sortedUsersDTOList = sortedUsersList.Select(user => new GetUserDTO(
                            user.Id, user.FirstName, user.LastName, user.Email, user.Password, user.ContactNumber
                        )).ToList();
                    return sortedUsersDTOList;
                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<(IEnumerable<GetUserDTO>, int)> GetAllPaginatedUserAsync(string? sortColumn, string? sordOrder, int page, int pageSize, User user)
        {
            var userListAndTotalPages = await userRepository.GetAllPaginatedUserAsync(sortColumn, sordOrder, page, pageSize, user);
            var userDTOList = userListAndTotalPages.Item1.Select(user =>

                    new GetUserDTO(
                            user.Id,
                            user.FirstName,
                            user.LastName,
                            user.Email, 
                            user.Password,
                            user.ContactNumber
                        )
                ).ToList();
            return (userDTOList, userListAndTotalPages.Item2);
        }
    }

    
}
