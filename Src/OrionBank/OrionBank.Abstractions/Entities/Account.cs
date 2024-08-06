namespace OrionBank.Abstractions.Entities
{
    [GenerateSerializer]
    [Alias("OrionBank.Abstractions.Entities.Account")]
    public sealed record class Account
    {
        [Id(0)]
        public string Id { get; set; } = string.Empty;
        [Id(1)]
        public string CustomerId { get; set; } = string.Empty;
        [Id(2)]
        public AccountStatus AccountStatus { get; set; }
        [Id(3)]
        public double Balance { get; set; } = 5_000;

        [Id(4)] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [Id(5)]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        [Id(6)]
        public bool IsDeleted { get; set; } = false;
    }

    public enum AccountStatus
    {
        Active,
        InActive,
        Suspended,
        Withheld
    }

    public enum AccountActions
    {
        Add,
        Delete,
    }
}
