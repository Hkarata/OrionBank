using OrionBank.Abstractions.Entities;
using OrionBank.Abstractions.GrainInterfaces;

namespace OrionBank.Grains
{
    public sealed class AccountGrain : Grain, IAccountGrain
    {
        public Task ActivateAccount()
        {
            throw new NotImplementedException();
        }

        public Task CreateOrUpdateAccount(Account accountDetails)
        {
            throw new NotImplementedException();
        }

        public Task DeactivateAccount()
        {
            throw new NotImplementedException();
        }

        public Task Deposit(int amount)
        {
            throw new NotImplementedException();
        }

        public Task<double> GetBalance()
        {
            throw new NotImplementedException();
        }

        public ValueTask<Account> TryGetAccount(string accountId)
        {
            throw new NotImplementedException();
        }

        public Task Withdraw(int amount)
        {
            throw new NotImplementedException();
        }
    }
}
