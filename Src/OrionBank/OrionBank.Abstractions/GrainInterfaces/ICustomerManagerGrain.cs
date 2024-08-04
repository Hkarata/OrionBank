using OrionBank.Abstractions.Entities;
using Orleans;

namespace OrionBank.Abstractions.GrainInterfaces
{
    public interface ICustomerManagerGrain : IGrainWithStringKey
    {
        Task<HashSet<Customer>> GetAllCustomer();
        Task CreateOrUpdateCustomer(Customer customer);
    }
}
