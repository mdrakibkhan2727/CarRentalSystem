// CarRentalSystem.Application/Interfaces/IReportService.cs
using CarRentalSystem.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Interfaces
{
    public interface IReportService
    {
        Task<IEnumerable<UpcomingServiceDto>> GetUpcomingServicesAsync(DateTime today, int daysAhead = 14);
        Task<CarUtilizationReportDto> GetCarUtilizationStatisticsAsync(DateTime startDate, DateTime endDate);
    }
}