using OrionBank.Abstractions.Entities;
using OrionBank.Abstractions.GrainInterfaces;
using Orleans.Concurrency;
using Orleans.Transactions.Abstractions;

namespace OrionBank.Grains
{
    [Reentrant]
    internal class AccountGrain
        (
            [PersistentState(
                stateName: "Account",
                storageName: "OrionBank")]
            IPersistentState<Account> account,
            [TransactionalState("AccountBalance")] ITransactionalState<Account> accountBalance
            ) 
        : Grain, IAccountGrain
    {
        Task IAccountGrain.ActivateAccount() =>
            UpdateAccountStatus(AccountStatus.Active);

        Task IAccountGrain.CreateOrUpdateAccount(AccountActions action, Account accountDetails) =>
            UpdateAccount(action,accountDetails);

        Task IAccountGrain.DeactivateAccount() =>
            UpdateAccountStatus(AccountStatus.InActive);

        public async Task SuspendAccount() =>
            await UpdateAccountStatus(AccountStatus.Suspended);

        Task IAccountGrain.Deposit(int amount) =>
            accountBalance.PerformUpdate(state => state.Balance += amount);

        Task<double> IAccountGrain.GetBalance() =>
            accountBalance.PerformRead(state => state.Balance);

        ValueTask<Account> IAccountGrain.TryGetAccount() =>
            ValueTask.FromResult(account.State);

        Task IAccountGrain.Withdraw(int amount) =>
            accountBalance.PerformUpdate(acc =>
            {
                if (acc.Balance >= amount)
                    acc.Balance -= amount;
                else
                    throw new InvalidOperationException("Insufficient funds");
            });
        
        private async Task UpdateAccount(AccountActions action, Account accountDetails)
        {
            account.State = accountDetails;
            await account.WriteStateAsync();
            
            var customerGrain = GrainFactory.GetGrain<ICustomerGrain>(accountDetails.CustomerId);
            await customerGrain.AddAccount(action, accountDetails.Id);
        }

        private async Task UpdateAccountStatus(AccountStatus status)
        {
            switch (status)
            {
                case AccountStatus.Active:
                    account.State.AccountStatus = AccountStatus.Active;
                    account.State.UpdatedAt = DateTime.UtcNow;
                    await  account.WriteStateAsync();
                    break;
                case AccountStatus.InActive:
                    account.State.AccountStatus = AccountStatus.InActive;
                    account.State.UpdatedAt = DateTime.UtcNow;
                    await  account.WriteStateAsync();
                    break;
                case AccountStatus.Suspended:
                    account.State.AccountStatus = AccountStatus.Suspended;
                    account.State.UpdatedAt = DateTime.UtcNow;
                    await  account.WriteStateAsync();
                    break;
                case AccountStatus.Withheld: 
                    account.State.AccountStatus = AccountStatus.Withheld;
                    account.State.UpdatedAt = DateTime.UtcNow;
                    await  account.WriteStateAsync();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }
    }
}
