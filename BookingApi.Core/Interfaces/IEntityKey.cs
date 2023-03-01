namespace BookingApi.Core.Interfaces
{
    public interface IEntityKey<out TKey> : IEntity where TKey : notnull
    {
        TKey Id { get; }
    }
}
