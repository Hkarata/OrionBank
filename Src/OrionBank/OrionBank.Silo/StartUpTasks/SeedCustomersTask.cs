using OrionBank.Abstractions.Entities;
using OrionBank.Abstractions.GrainInterfaces;
using OrionBank.Silo.Extensions;

namespace OrionBank.Silo.StartUpTasks
{
    internal class SeedCustomersTask(IGrainFactory grainFactory) : IStartupTask
    {
        public async Task Execute(CancellationToken cancellationToken)
        {
            var faker = new Customer().GetBogusFaker();

            foreach (var customer in faker.GenerateLazy(50))
            {
                var customerGrain = grainFactory.GetGrain<ICustomerGrain>(customer.Id);
                await customerGrain.CreateOrUpdateCustomer(customer);
            }
        }
    }
}
