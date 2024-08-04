using OrionBank.Abstractions.Entities;
using Orleans;

namespace OrionBank.Abstractions.GrainInterfaces;

public interface ICustomerGrain : IGrainWithStringKey
{
    Task<Customer> GetCustomer();

    Task CreateOrUpdateCustomer(Customer customerDetails);

    Task DeleteCustomer();
}