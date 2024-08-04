using OrionBank.Abstractions.Entities;
using OrionBank.Abstractions.GrainInterfaces;

namespace OrionBank.Grains
{
    internal class AccountGrain : Grain, IAccountGrain
    {
        Task IAccountGrain.ActivateAccount()
        {
            throw new NotImplementedException();
        }

        Task IAccountGrain.CreateOrUpdateAccount(Account accountDetails)
        {
            throw new NotImplementedException();
        }

        Task IAccountGrain.DeactivateAccount()
        {
            throw new NotImplementedException();
        }

        Task IAccountGrain.Deposit(int amount)
        {
            throw new NotImplementedException();
        }

        Task<double> IAccountGrain.GetBalance()
        {
            throw new NotImplementedException();
        }

        ValueTask<Account> IAccountGrain.TryGetAccount(string accountId)
        {
            throw new NotImplementedException();
        }

        Task IAccountGrain.Withdraw(int amount)
        {
            throw new NotImplementedException();
        }
    }
}
