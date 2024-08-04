using OrionBank.Abstractions.Entities;
using OrionBank.Abstractions.GrainInterfaces;

namespace OrionBank.Grains
{
    internal class CustomerGrain(
        [PersistentState(
            stateName: "Customer",
            storageName: "OrionBank")]
        IPersistentState<Customer> customer)
        : Grain, ICustomerGrain
    {
        private readonly IPersistentState<Customer> _customer = customer;


        public async Task CreateOrUpdateCustomer(Customer customerDetails) =>
            await (_customer.State.Id != customerDetails.Id
                ? Task.Run(async () =>
                {
                    _customer.State = customerDetails;
                    await _customer.WriteStateAsync();
                })
                : Task.CompletedTask);

        public async Task DeleteCustomer(string customerId) =>
             await (_customer.State.Id == customerId && !_customer.State.IsDeleted
                 ? Task.Run(async () =>
                 {
                     _customer.State.IsDeleted = true;
                     await _customer.WriteStateAsync();
                 })
                 : Task.CompletedTask);


        public async ValueTask<(bool IsExisting, Customer customerDetails)> TryGetCustomer(string customerId) =>
              await ValueTask.FromResult(
                  _customer.State.Id == customerId && !_customer.State.IsDeleted
                      ? (true, _customer.State)
                      : (false, default(Customer))
              );


    }
}
