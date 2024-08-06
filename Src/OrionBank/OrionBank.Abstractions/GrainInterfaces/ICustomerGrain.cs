using OrionBank.Abstractions.Entities;

namespace OrionBank.Abstractions.GrainInterfaces;

[Alias("GrainInterfaces.ICustomerGrain")]
public interface ICustomerGrain : IGrainWithStringKey
{
    [Alias("GetCustomer")]
    Task<Customer> GetCustomer();

    [Alias("CreateOrUpdateCustomer")]
    Task CreateOrUpdateCustomer(Customer customerDetails);

    [Alias("DeleteCustomer")]
    Task DeleteCustomer();

    [Alias("GetAccounts")]
    Task<HashSet<Account>> GetAccounts();

    [Alias("AddAccount")]
    Task AddAccount(AccountActions accountActions,string accountId);
}