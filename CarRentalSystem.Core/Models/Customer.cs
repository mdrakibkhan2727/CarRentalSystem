// CarRentalSystem.Core/Models/Customer.cs
using System;
using System.Collections.Generic;

namespace CarRentalSystem.Core.Models
{
    public class Customer
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string CustomerIdString { get; set; } = string.Empty; // User-provided ID
        public string FullName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        // Navigation property for Entity Framework
        public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
    }
}