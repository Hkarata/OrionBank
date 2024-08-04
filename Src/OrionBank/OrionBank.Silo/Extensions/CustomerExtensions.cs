using Bogus;
using OrionBank.Abstractions.Entities;

namespace OrionBank.Silo.Extensions
{
    internal static class CustomerExtensions
    {
        internal static Faker<Customer> GetBogusFaker(this Customer customer) =>
            new Faker<Customer>()
                .StrictMode(true)
                .RuleFor(c => c.Id, (f, c) => f.Random.Number(1, 100_000).ToString())
                .RuleFor(c => c.FirstName, (f, c) => f.Name.FirstName())
                .RuleFor(c => c.LastName, (f, c) => f.Name.LastName())
                .RuleFor(c => c.OtherName, (f, c) => f.Name.LastName())
                .RuleFor(c => c.Email, (f, c) => f.Internet.Email())
                .RuleFor(c => c.PhoneNumber, (f, c) => f.Phone.PhoneNumber())
                .RuleFor(c => c.Address, (f, c) => f.Address.FullAddress())
                .RuleFor(c => c.IsDeleted, (f, c) => false);

        internal static bool MatchesFilter(this Customer customer, string? filter)
        {
            if (filter is null or { Length: 0 })
            {
                return true;
            }

            if (customer is not null)
            {
                return customer.FirstName.Contains(filter, StringComparison.OrdinalIgnoreCase)
                    || customer.LastName.Contains(filter, StringComparison.OrdinalIgnoreCase)
                    || customer.Email.Contains(filter, StringComparison.OrdinalIgnoreCase)
                    || customer.PhoneNumber.Contains(filter, StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }
    }
}
