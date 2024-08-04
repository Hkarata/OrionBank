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

        Task ICustomerGrain.CreateOrUpdateCustomer(Customer customerDetails) =>
            UpdateStateAsync(customerDetails);

        Task ICustomerGrain.DeleteCustomer() =>
            Task.FromResult(_customer.State.IsDeleted = true);

        Task<Customer> ICustomerGrain.GetCustomer() =>
            Task.FromResult(_customer.State);

        private async Task UpdateStateAsync(Customer customer)
        {
            _customer.State = customer;
            await _customer.WriteStateAsync();

            var customerManager = GrainFactory.GetGrain<ICustomerManagerGrain>(0.ToString());
            await customerManager.CreateOrUpdateCustomer(customer);
        }
    }
}
