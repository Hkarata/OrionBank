using OrionBank.Abstractions.Entities;
using OrionBank.Abstractions.GrainInterfaces;

namespace OrionBank.Grains
{
    internal class CustomerManagerGrain(
            [PersistentState(
                stateName: "Customers",
                storageName: "OrionBank")]
            IPersistentState<HashSet<string>> customerIds
            )
            : Grain, ICustomerManagerGrain
    {
        private readonly Dictionary<string, Customer> _customerCache = new();

        public override Task OnActivateAsync(CancellationToken cancellationToken) => 
            PopulateCustomerCacheAsync(cancellationToken);

        Task ICustomerManagerGrain.CreateOrUpdateCustomer(Customer customer)
        {
            customerIds.State.Add(customer.Id);

            return Task.FromResult(_customerCache[customer.Id] = customer);
        }

        Task<HashSet<Customer>> ICustomerManagerGrain.GetAllCustomer() =>
            Task.FromResult(_customerCache.Values.ToHashSet());

        private async Task PopulateCustomerCacheAsync(CancellationToken cancellationToken)
        {
            if (customerIds is not { State.Count: > 0 })
            {
                return;
            }

            await Parallel.ForEachAsync(
                customerIds.State, // Explicitly use the current task-scheduler.
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
