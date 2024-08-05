using OrionBank.Abstractions.Entities;

namespace OrionBank.Abstractions.GrainInterfaces
{
    public interface IAccountGrain : IGrainWithStringKey
    {
        Task ActivateAccount();

        Task DeactivateAccount();

        ValueTask<Account> TryGetAccount(string accountId);

        Task CreateOrUpdateAccount(Account accountDetails);

        [Transaction(TransactionOption.Join)]
        Task Withdraw(int amount);

        [Transaction(TransactionOption.Join)]
        Task Deposit(int amount);

        [Transaction(TransactionOption.CreateOrJoin)]
        Task<Double> GetBalance();
    }
}
