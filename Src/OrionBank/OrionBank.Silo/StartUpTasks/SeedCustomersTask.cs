using OrionBank.Abstractions.Entities;
using OrionBank.Abstractions.GrainInterfaces;
using OrionBank.Silo.Extensions;

namespace OrionBank.Silo.StartUpTasks
{
    public sealed class SeedCustomersTask : IStartupTask
    {
        private readonly IGrainFactory _grainFactory;

        public SeedCustomersTask(IGrainFactory grainFactory) =>
            _grainFactory = grainFactory;

        async Task IStartupTask.Execute(CancellationToken cancellationToken)
        {
            var faker = new Customer().GetBogusFaker();

            foreach (var customer in faker.GenerateLazy(50))
            {
                var customerGrain = _grainFactory.GetGrain<ICustomerGrain>(customer.Id);
                await customerGrain.CreateOrUpdateCustomer(customer);
                var customerManagerGrain = _grainFactory.GetGrain<ICustomerManagerGrain>(1.ToString());
                await customerManagerGrain.CreateOrUpdateCustomer(customer);
            }
        }
    }
}
