using OrionBank.Abstractions.Entities;
using OrionBank.Abstractions.GrainInterfaces;
using Orleans.Concurrency;

namespace OrionBank.Grains
{
    [Reentrant]
    public sealed class CustomerManagerGrain(
        [PersistentState(
            stateName: "Customer",
            storageName: "customer-details")]
        IPersistentState<HashSet<string>> state,
        IGrainFactory grainFactory)
        : Grain, ICustomerManagerGrain
    {
        private readonly Dictionary<string, Customer> _customerCache = new();

        public override Task OnActivateAsync(CancellationToken cancellationToken) =>
            PopulateCustomerCacheAsync(cancellationToken);

        public Task CreateOrUpdateCustomer(Customer customer)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCustomer(string customerId)
        {
            throw new NotImplementedException();
        }

        public Task<HashSet<Customer>> GetAllCustomer() =>
            Task.FromResult(_customerCache.Values.ToHashSet());

        private async Task PopulateCustomerCacheAsync(CancellationToken cancellationToken)
        {
            if (state is not { State.Count: > 0 })
            {
                return;
            }

            await Parallel.ForEachAsync(
                state.State, // Explicitly use the current task-scheduler.
                new ParallelOptions
                {
                    TaskScheduler = TaskScheduler.Current,
                    CancellationToken = cancellationToken,
                },
                async (id, _) =>
                {
                    var customerGrain = grainFactory.GetGrain<ICustomerGrain>(id);
                    var customerQuery = await customerGrain.TryGetCustomer(id);
                    if (customerQuery.IsExisting)
                    {
                        _customerCache[id] = customerQuery.customerDetails;
                    }
                });
        }
    }
}
