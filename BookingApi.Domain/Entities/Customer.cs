using BookingApi.Core.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace BookingApi.Domain.Entities
{
    public class Customer : BaseEntity
    {
        [Required]
        public string FirstName { get; private init; }

        [Required]
        public string LastName { get; private init; }

        [Required]
        public string Email { get; private init; }

        public Customer(string firstName, string lastName, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }
    }
}
