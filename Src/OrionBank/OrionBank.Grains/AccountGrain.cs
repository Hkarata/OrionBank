using OrionBank.Abstractions.Entities;
using OrionBank.Abstractions.GrainInterfaces;
using Orleans.Concurrency;
using Orleans.Transactions.Abstractions;

namespace OrionBank.Grains
{
    [Reentrant]
    public sealed class AccountGrain(
        [PersistentState(
            stateName: "Account",
            storageName: "OrionBank")]
        IPersistentState<Account> account,
        [TransactionalState("Account", "OrionBank")] ITransactionalState<Account> transactional
        )
        : Grain, IAccountGrain
    {
        private readonly IPersistentState<Account> _account = account;

        private readonly ITransactionalState<Account> _transactional = transactional;

        public async Task ActivateAccount(string accountId) =>
            await (_account.State.Id == accountId && _account.State.AccountStatus == AccountStatus.Active
                ? Task.Run(async () =>
                {
                    _account.State.AccountStatus = AccountStatus.Active;
                    await _account.WriteStateAsync();
                })
                : Task.CompletedTask);

        public Task CreateOrUpdateAccount(Account accountDetails)
        {
            throw new NotImplementedException();
        }

        public Task DeactivateAccount(string accountId) =>
            (_account.State.Id == accountId && !(_account.State.AccountStatus == AccountStatus.Active)
                ? Task.Run(async () =>
                {
                    _account.State.AccountStatus = AccountStatus.InActive;
                    await _account.WriteStateAsync();
                })
                : Task.CompletedTask);

        public Task Deposit(int amount) =>
            Task.Run(async () =>
            {
                _transactional.PerformUpdate(_account, account =>
                {
                    account.Balance += amount;
                    return Task.CompletedTask;
                });
            });

        public Task<int> GetBalance()
        {
            throw new NotImplementedException();
        }

        public ValueTask<(bool IsExisting, Account accountDetails)> TryGetAccount(string accountId)
        {
            throw new NotImplementedException();
        }

        public Task Withdraw(int amount)
        {
            throw new NotImplementedException();
        }
    }
}
