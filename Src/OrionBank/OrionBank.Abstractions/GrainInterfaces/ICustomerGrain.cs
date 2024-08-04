using OrionBank.Abstractions.Entities;
using Orleans;

namespace OrionBank.Abstractions.GrainInterfaces;

public interface ICustomerGrain : IGrainWithStringKey
{
    ValueTask<(bool IsExisting, Customer customerDetails)> TryGetCustomer(string customerId);

    Task CreateOrUpdateCustomer(Customer customerDetails);

    Task DeleteCustomer(string customerId);
}