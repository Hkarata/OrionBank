using OrionBank.Abstractions.Entities;
using OrionBank.Abstractions.GrainInterfaces;

namespace OrionBank.Grains
{
    internal class CustomerGrain(
        [PersistentState(
            stateName: "Customer",
            storageName: "OrionBank"
        )] IPersistentState<Customer> customer
        )
        : Grain, ICustomerGrain
    {
        Task<HashSet<Account>> ICustomerGrain.GetAccounts()
        {
            if (customer.State.Accounts is null)
            {
                return Task.FromResult(new HashSet<Account>());
            }
            
            var accounts = new HashSet<Account>();

            Parallel.ForEachAsync(
                customer.State.Accounts,
                new ParallelOptions
                {
                    TaskScheduler = TaskScheduler.Current
                },
                async (account, _) =>
                {
                    var accountGrain = GrainFactory.GetGrain<IAccountGrain>(account);
                    var acc = await accountGrain.TryGetAccount();
                    accounts.Add(acc);
                }
            );
            
            return Task.FromResult(accounts);
        }
        Task ICustomerGrain.AddAccount(AccountActions accountActions,string accountId) =>
            UpdateAccounts(accountActions, accountId);

        Task ICustomerGrain.CreateOrUpdateCustomer(Customer customerDetails) =>
            UpdateStateAsync(customerDetails);

        Task ICustomerGrain.DeleteCustomer() =>
            Task.FromResult(customer.State.IsDeleted = true);

        Task<Customer> ICustomerGrain.GetCustomer() =>
            Task.FromResult(customer.State);

        private async Task UpdateStateAsync(Customer customer1)
        {
            customer.State = customer1;
            await customer.WriteStateAsync();

            var customerManager = GrainFactory.GetGrain<ICustomerManagerGrain>(0.ToString());
            await customerManager.CreateOrUpdateCustomer(customer1);
        }

        private async Task UpdateAccounts(AccountActions action, string accountId)
        {
            customer.State.Accounts ??= [];
            
            switch (action)
            {
                case AccountActions.Add:
                    customer.State.Accounts.Add(accountId);
                    break;
                case AccountActions.Delete:
                    customer.State.Accounts.Remove(accountId);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }

            await customer.WriteStateAsync();
        }
    }
}
