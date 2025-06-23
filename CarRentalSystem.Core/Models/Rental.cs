// CarRentalSystem.Core/Models/Rental.cs
using System;

namespace CarRentalSystem.Core.Models
{
    public class Rental
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CustomerId { get; set; }
        public Guid CarId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Navigation properties for Entity Framework
        public Customer Customer { get; set; } = default!; // 'default!' to suppress null warning, assumes EF loads it
        public Car Car { get; set; } = default!; // 'default!' to suppress null warning, assumes EF loads it

        public bool IsCompleted()
        {
            return DateTime.Now.Date > EndDate.Date;
        }
    }
}