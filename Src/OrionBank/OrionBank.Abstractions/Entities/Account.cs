using Orleans;

namespace OrionBank.Abstractions.Entities
{
    [GenerateSerializer]
    public sealed record class Account
    {
        public string Id { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public AccountStatus AccountStatus { get; set; }
        public decimal Balance { get; set; } = 5_000;
    }

    public enum AccountStatus
    {
        Active,
        InActive,
        Suspended,
        Withheld
    }
}
