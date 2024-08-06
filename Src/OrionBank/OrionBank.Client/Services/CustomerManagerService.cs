using OrionBank.Abstractions.Entities;
using OrionBank.Abstractions.GrainInterfaces;

namespace OrionBank.Client.Services
{
    public class CustomerManagerService
        (
        IHttpContextAccessor httpContextAccessor,
        IClusterClient clusterClient
        )
        : BaseClusterService(clusterClient, httpContextAccessor)
    {

        public Task<HashSet<Customer>> GetAllCustomers() =>
            TryUseGrain<ICustomerManagerGrain, Task<HashSet<Customer>>>(
                customerManager => customerManager.GetAllCustomer(),
                "1",
                () => Task.FromResult(new HashSet<Customer>()));

    }
}
