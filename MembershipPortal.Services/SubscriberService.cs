using MembershipPortal.DTOs;
using MembershipPortal.IRepositories;
using MembershipPortal.IServices;
using MembershipPortal.Models;
using static MembershipPortal.DTOs.UserDTO;

namespace MembershipPortal.Services
{
    public class SubscriberService : ISubscriberService
    {
        private readonly ISubscriberRepository _subscriberRepository;

        public SubscriberService(ISubscriberRepository subscriberRepository)
        {
            _subscriberRepository = subscriberRepository;
        }
        public async Task<GetSubscriberDTO> CreateSubscriberAsync(CreateSubscriberDTO subscriberDTO)
        {
            try
            {
                var subscriber = await _subscriberRepository.CreateAsync(
                    new Subscriber()
                    {
                        FirstName = subscriberDTO.FirstName,
                        LastName = subscriberDTO.LastName,
                        ContactNumber = subscriberDTO.ContactNumber,
                        Email = subscriberDTO.Email,
                        GenderId = subscriberDTO.GenderId
                    });

                var getSubscriber = new GetSubscriberDTO
                                                    (subscriber.Id,
                                                        subscriber.FirstName,
                                                        subscriber.LastName,
                                                        subscriber.ContactNumber,
                                                        subscriber.Email,
                                                        subscriber.GenderId,
                                                        null);

                return getSubscriber;
            }
            catch (Exception)
            {
                // Console.WriteLine($"Error occurred in CreateSubscriberAsync: {ex.Message}");
                throw;

            }

        }

        public async Task<bool> DeleteSubscriberAsync(long id)
        {
            try
            {
                var subscriber = await _subscriberRepository.GetAsyncById(id);

                if (subscriber != null)
                {
                    return await _subscriberRepository.DeleteAsync(subscriber);
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Error occurred in DeleteSubscriberAsync: {ex.Message}");
                throw;
            }
            return false;
        }

        public async Task<GetSubscriberDTO> GetSubscriberAsync(long id)
        {
            try
            {
                var subscriber = await _subscriberRepository.GetSubscriberByIdAsync(id);

                if(subscriber != null)
                {
                    var getSubscriber = new GetSubscriberDTO
                                                    (   subscriber.Id,
                                                        subscriber.FirstName,
                                                        subscriber.LastName,
                                                        subscriber.ContactNumber,
                                                        subscriber.Email,
                                                        subscriber.GenderId,
                                                        subscriber.Gender.GenderName
                                                        );
                    return getSubscriber;
                }
                return null;
            }
            catch (Exception)
            {
                // Console.WriteLine($"Error occurred in GetSubscriberAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<SubscriberWithGenderViewModel>> GetSubscribersAsync()
        {
            try
            {
                var subscribers = await _subscriberRepository.GetAllSubscriberDataAsync();

                if(subscribers != null)
                {
                    return subscribers;
                }
                return null;
            }
            catch (Exception)
            {
                // Console.WriteLine($"Error occurred in GetSubscribersAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<GetSubscriberDTO>> SearchAsyncAll(string search)
        {
            try
            {
                var subscribers = await _subscriberRepository.SearchAsyncAll(search);

                if (subscribers != null)
                {
                    var subscribersDto = subscribers.Select(
                                                         subscriber =>
                                                         new GetSubscriberDTO
                                                                               (subscriber.Id,
                                                                                subscriber.FirstName,
                                                                                subscriber.LastName,
                                                                                subscriber.ContactNumber,
                                                                                subscriber.Email,
                                                                                subscriber.GenderId, subscriber.Gender.GenderName));
                    return subscribersDto;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return null;
        }

        public async Task<GetSubscriberDTO> UpdateSubscriberAsync(long id, UpdateSubscriberDTO subscriberDTO)
        {
            try
            {
                var oldSubscriber = await _subscriberRepository.GetSubscriberByIdAsync(id);

                if (oldSubscriber != null)
                {
                    oldSubscriber.FirstName = subscriberDTO.FirstName;
                    oldSubscriber.LastName = subscriberDTO.LastName;
                    oldSubscriber.ContactNumber = subscriberDTO.ContactNumber;
                    oldSubscriber.Email = subscriberDTO.Email;
                    oldSubscriber.GenderId = subscriberDTO.GenderId;

                    var subscriber = await _subscriberRepository.UpdateSubsciberDataAsync(id, oldSubscriber);

                    var subscriberDto = new GetSubscriberDTO
                                        (subscriber.Id,
                                            subscriber.FirstName,
                                            subscriber.LastName,
                                            subscriber.ContactNumber,
                                            subscriber.Email,
                                            subscriber.GenderId, subscriber.Gender.GenderName);

                    return subscriberDto;
                }
            }
            catch(Exception)
            {
                //Console.WriteLine($"Error occurred in UpdateSubscriberAsync: {ex.Message}");
                throw;
            }
            return null;
                    
        }

        public async Task<IEnumerable<GetSubscriberDTO>> GetAllSortedSubscribers(string? sortColumn, string? sortOrder)
        {
            try
            {
                var sortedSubscribersList = await _subscriberRepository.GetAllSortedSubscribers(sortColumn, sortOrder);
                if (sortedSubscribersList != null)
                {
                    var sortedSubscribersDTOList = sortedSubscribersList.Select(subscribers => new GetSubscriberDTO(
                            subscribers.Id, subscribers.FirstName, subscribers.LastName, subscribers.ContactNumber, subscribers.Email, subscribers.GenderId, subscribers.Gender.GenderName
                        ))
                        .ToList();
                    return sortedSubscribersDTOList;
                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<(IEnumerable<GetSubscriberDTO>, int)> GetAllPaginatedSubscriberAsync(int page, int pageSize, Subscriber subscriber)
        {
            var subscriberListAndTotalPages = await _subscriberRepository.GetAllPaginatedSubscriberAsync(page, pageSize, subscriber);
            var userDTOList = subscriberListAndTotalPages.Item1.Select(subscriber =>

                    new GetSubscriberDTO(
                            subscriber.Id,
                            subscriber.FirstName,
                            subscriber.LastName,
                            subscriber.Email,
                            subscriber.ContactNumber,
                            subscriber.GenderId,
                            null
                        )
                ).ToList();
            return (userDTOList, subscriberListAndTotalPages.Item2);
        }
    }
}
