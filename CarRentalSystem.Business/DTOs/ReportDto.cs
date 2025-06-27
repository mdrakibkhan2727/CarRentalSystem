namespace CarRentalSystem.Business.DTOs
{
    public class UpcomingServiceDto
    {
        public int CarId { get; set; }
        public string Model { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public DateTime ServiceDate { get; set; }
    }

    public class CarUtilizationDto
    {
        public string Type { get; set; } = string.Empty;
        public int TotalRentals { get; set; }
        public double Percentage { get; set; }
    }

    public class CarUtilizationReportDto
    {
        public string MostRentedCarType { get; set; } = string.Empty;
        public double UtilizationPercentage { get; set; }
        public List<CarUtilizationDto> CarTypeBreakdown { get; set; } = new List<CarUtilizationDto>();
        public int TotalRentalsOverall { get; set; }
    }
}