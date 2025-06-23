// CarRentalSystem.Core/Models/Service.cs
using System;

namespace CarRentalSystem.Core.Models
{
    public class Service
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CarId { get; set; }
        public DateTime ScheduledDate { get; set; }
        public int DurationDays { get; set; } = 2; // Fixed as per requirements
        public bool IsCompleted { get; set; } = false;

        // Navigation property for Entity Framework
        public Car Car { get; set; } = default!;
    }
}