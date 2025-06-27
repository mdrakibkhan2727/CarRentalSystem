using System.ComponentModel.DataAnnotations;

namespace CarRentalSystem.Business.DTOs.Rental
{
    public class RegisterRentalDto
    {
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string CarType { get; set; } = string.Empty;
        public string CarModel { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
