using BookingApi.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BookingApi.Core.Abstractions
{
    public class BaseEntity : IEntityKey<Guid>
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
