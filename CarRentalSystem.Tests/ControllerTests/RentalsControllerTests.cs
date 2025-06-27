using CarRentalSystem.Api.Controllers;
using CarRentalSystem.Business.Repositories.IRepository;
using Moq;

namespace CarRentalSystem.Tests.Controllers
{
    public class RentalsControllerTests
    {
        private readonly Mock<IRentalRepository> _mockRepo;
        private readonly RentalsController _controller;

        public RentalsControllerTests()
        {
            _mockRepo = new Mock<IRentalRepository>();
            _controller = new RentalsController(_mockRepo.Object);
        }

      
    }
}
