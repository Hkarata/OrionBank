using OrionBank.Abstractions.Entities;
using Orleans;

namespace OrionBank.Abstractions.GrainInterfaces
{
    public interface IAccountGrain : IGrainWithStringKey
    {
        Task ActivateAccount(string accountId);
        Task DeactivateAccount(string accountId);
        ValueTask<(bool IsExisting, Account accountDetails)> TryGetAccount(string accountId);
        Task CreateOrUpdateAccount(Account accountDetails);

        [Transaction(TransactionOption.Join)]
        Task Withdraw(int amount);

        [Transaction(TransactionOption.Join)]
        Task Deposit(int amount);

        [Transaction(TransactionOption.CreateOrJoin)]
        Task<int> GetBalance();
    }
}
