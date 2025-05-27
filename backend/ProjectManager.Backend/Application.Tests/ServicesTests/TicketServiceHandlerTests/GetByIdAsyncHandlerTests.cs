using Moq;
using AutoMapper;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;
using ProjectManager.Application.Services;
using Xunit;
using ProjectManager.Application.DTOs;
using ProjectManager.Domain.Interfaces.ExternalServices;

namespace Application.Tests.ServicesTests.TicketServiceHandlerTests
{
    public class GetByIdAsyncHandlerTests
    {
        private readonly Mock<ITicketRepository> _ticketRepositoryMock;
        private readonly Mock<IDropBoxClient> _dropBoxClientMock;
        private readonly Mock<ITicketTransitionRuleRepository> _ticketTransitionRuleRepositoryMock;
        private readonly TicketService _service;

        public GetByIdAsyncHandlerTests()
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
                _ticketTransitionRuleRepositoryMock.Object,
                null
            );
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnTicketDto_WhenTicketExists()
        {
            var ticketId = Guid.NewGuid();
            var ticket = new Ticket 
            {
                TicketId = ticketId,
                ColumnId = Guid.NewGuid(),
                Description = "Test Ticket"
            };

            _ticketRepositoryMock.Setup(repo => repo.GetByIdAsync(ticketId)).ReturnsAsync(ticket);
            
            var result = await _service.GetByIdAsync(ticketId);
            
            Assert.NotNull(result);
            Assert.IsType<TicketDto>(result);
            Assert.Equal(ticket.TicketId, result.TicketId);
            Assert.Equal(ticket.Description, result.Description);
            _ticketRepositoryMock.Verify(x => x.GetByIdAsync(ticketId), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenTicketDoesNotExist()
        {
            var ticketId = Guid.NewGuid();
            _ticketRepositoryMock.Setup(repo => repo.GetByIdAsync(ticketId)).ReturnsAsync((Ticket)null);
            
            var result = await _service.GetByIdAsync(ticketId);
            
            Assert.Null(result);
            _ticketRepositoryMock.Verify(x => x.GetByIdAsync(ticketId), Times.Once);
        }


    }
}
