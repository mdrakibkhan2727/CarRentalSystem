namespace CarRentalSystem.Business.DTOs
{
    public class CarResponseDto
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public DateTime LastServiceDate { get; set; }
    }

    public class AvailableCarDto
    {
        public int CarId { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public DateTime LastServiceDate { get; set; }
    }
}