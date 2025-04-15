using Moq;
using AutoMapper;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;
using ProjectManager.Application.Services;
using Xunit;
using ProjectManager.Application.RequestsDTOs;
using ProjectManager.Domain.Interfaces.ExternalServices;

namespace Application.Tests.ServicesTests.TicketServiceHandlerTests
{
    public class UpdateAsyncHandlerTests
    {
        private readonly Mock<ITicketRepository> _ticketRepositoryMock;
        private readonly Mock<IDropBoxClient> _dropBoxClientMock;
        private readonly Mock<ITicketTransitionRuleRepository> _ticketTransitionRuleRepositoryMock;
        private readonly TicketService _service;

        public UpdateAsyncHandlerTests()
        {
            _ticketRepositoryMock = new Mock<ITicketRepository>();
            _dropBoxClientMock = new Mock<IDropBoxClient>();
            _ticketTransitionRuleRepositoryMock = new Mock<ITicketTransitionRuleRepository>();
            
            var mockMapper = new MapperConfiguration(cfg => cfg.AddProfile(new GetAllAsyncHandlerTests.MappingProfile()));
            var mapper = mockMapper.CreateMapper();
            
            _service = new TicketService(
                _ticketRepositoryMock.Object,
                mapper,
                _dropBoxClientMock.Object,
                _ticketTransitionRuleRepositoryMock.Object
            );
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateTicket_WhenValidTicketRequestProvided()
        {
            var ticketRequest = new UpdateTicketRequest
            {
                TicketId = Guid.NewGuid(),
                Description = "Updated Description"
            };
            

            _ticketRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Ticket>())).Returns(Task.CompletedTask);
            
            await _service.UpdateAsync(ticketRequest);
            
            _ticketRepositoryMock.Verify(repo => repo.UpdateAsync(It.Is<Ticket>(t => t.TicketId == ticketRequest.TicketId && t.Description == ticketRequest.Description)), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenUpdateFails()
        {
            var ticketRequest = new UpdateTicketRequest
            {
                TicketId = Guid.NewGuid(),
                Description = "Updated Description"
            };
            

            _ticketRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Ticket>())).ThrowsAsync(new Exception("Update failed"));
            
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(ticketRequest));
            Assert.Equal("Update failed", exception.Message);
        }


    }
}
