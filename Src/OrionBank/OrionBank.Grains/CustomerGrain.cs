using OrionBank.Abstractions.Entities;
using OrionBank.Abstractions.GrainInterfaces;

namespace OrionBank.Grains
{
    public sealed class CustomerGrain(
        [PersistentState(
            stateName: "Customer",
            storageName: "OrionBank"
        )] IPersistentState<Customer> customer
        ) 
        : Grain, ICustomerGrain
    {
        private readonly IPersistentState<Customer> _customer = customer;

        public Task CreateOrUpdateCustomer(Customer customerDetails) =>
            Task.FromResult(_customer.State = customerDetails);

        public Task DeleteCustomer() =>
            Task.FromResult(_customer.State.IsDeleted = true);

        public Task<Customer> GetCustomer() =>
            Task.FromResult(_customer.State);
    }
}
