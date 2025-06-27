using System.ComponentModel.DataAnnotations;

namespace CarRentalSystem.Data.Models
{
    public class Car
    {
        [Key]
        public int Id { get; set; }
        public string CarType { get; set; } = string.Empty;
        public string CarModel { get; set; } = string.Empty;
        public DateTime NextServiceDate { get; set; }
    }
}
