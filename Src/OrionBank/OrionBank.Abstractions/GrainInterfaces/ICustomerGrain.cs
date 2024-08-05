using OrionBank.Abstractions.Entities;

namespace OrionBank.Abstractions.GrainInterfaces;

[Alias("GrainInterfaces.ICustomerGrain")]
public interface ICustomerGrain : IGrainWithStringKey
{
    Task<Customer> GetCustomer();

    Task CreateOrUpdateCustomer(Customer customerDetails);

    Task DeleteCustomer();
}