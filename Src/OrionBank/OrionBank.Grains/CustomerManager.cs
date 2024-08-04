using OrionBank.Abstractions.Entities;
using OrionBank.Abstractions.GrainInterfaces;

namespace OrionBank.Grains
{
    public sealed class CustomerManager(
            [PersistentState(
                stateName: "Customers",
                storageName: "OrionBank")]
            IPersistentState<HashSet<string>> customerIds
            )
            : Grain, ICustomerManagerGrain
    {

        private readonly IPersistentState<HashSet<string>> _customerIds = customerIds;

        private readonly Dictionary<string, Customer> _customerCache = new();

        public override Task OnActivateAsync(CancellationToken cancellationToken) => 
            PopulateCustomerCacheAsync(cancellationToken);

        public Task CreateOrUpdateCustomer(Customer customer)
        {
            if (!_customerIds.State.Contains(customer.Id))
            {
                _customerIds.State.Add(customer.Id);
            }

            return Task.FromResult(_customerCache[customer.Id] = customer);
        }

        public Task<HashSet<Customer>> GetAllCustomer()
        {
            throw new NotImplementedException();
        }

        private async Task PopulateCustomerCacheAsync(CancellationToken cancellationToken)
        {
            if (_customerIds is not { State.Count: > 0 })
            {
                return;
            }

            await Parallel.ForEachAsync(
                _customerIds.State, // Explicitly use the current task-scheduler.
                new ParallelOptions
                {
                    TaskScheduler = TaskScheduler.Current,
                    CancellationToken = cancellationToken,
                },
                async (id, _) =>
                {
                    var customerGrain = GrainFactory.GetGrain<ICustomerGrain>(id);
                    _customerCache[id] = await customerGrain.GetCustomer();
                });
        }

    }
}
