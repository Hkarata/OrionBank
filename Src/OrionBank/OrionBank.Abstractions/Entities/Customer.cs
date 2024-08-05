namespace OrionBank.Abstractions.Entities
{
    [GenerateSerializer, Immutable]
    public sealed record class Customer
    {
        [Id(0)]
        public string Id { get; set; } = string.Empty;

        [Id(1)]
        public string FirstName { get; set; } = string.Empty;

        [Id(2)]
        public string LastName { get; set; } = string.Empty;

        [Id(3)]
        public string OtherName { get; set; } = string.Empty;

        [Id(4)]
        public string Email { get; set; } = string.Empty;

        [Id(5)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Id(6)]
        public string Address { get; set; } = string.Empty;

        [Id(7)]
        public bool IsDeleted { get; set; }
    }
}
