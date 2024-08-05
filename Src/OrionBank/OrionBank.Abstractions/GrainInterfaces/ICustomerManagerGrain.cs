using OrionBank.Abstractions.Entities;

namespace OrionBank.Abstractions.GrainInterfaces
{
    public interface ICustomerManagerGrain : IGrainWithStringKey
    {
        Task<HashSet<Customer>> GetAllCustomer();
        Task CreateOrUpdateCustomer(Customer customer);
    }
}
