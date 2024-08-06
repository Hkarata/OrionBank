using OrionBank.Abstractions.Entities;

namespace OrionBank.Abstractions.GrainInterfaces
{
    [Alias("OrionBank.Abstractions.GrainInterfaces.IAccountGrain")]
    public interface IAccountGrain : IGrainWithStringKey
    {
        [Alias("ActivateAccount")]
        Task ActivateAccount();

        [Alias("DeactivateAccount")]
        Task DeactivateAccount();

        [Alias("SuspendAccount")]
        Task SuspendAccount();

        [Alias("TryGetAccount")]
        ValueTask<Account> TryGetAccount();

        [Alias("CreateOrUpdateAccount")]
        Task CreateOrUpdateAccount(AccountActions action ,Account accountDetails);

        [Transaction(TransactionOption.Join)]
        [Alias("Withdraw")]
        Task Withdraw(int amount);

        [Transaction(TransactionOption.Join)]
        [Alias("Deposit")]
        Task Deposit(int amount);

        [Transaction(TransactionOption.CreateOrJoin)]
        [Alias("GetBalance")]
        Task<double> GetBalance();
    }
}
