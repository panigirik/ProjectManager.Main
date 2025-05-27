using Moq;
using Xunit;
using ProjectManager.Application.Services;
using ProjectManager.Application.RequestsDTOs;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;
using ProjectManager.Domain.Interfaces.ExternalServices;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using ProjectManager.Domain.Enums;

namespace Application.Tests.ServicesTests.TicketServiceHandlerTests
{
    public class MoveToColumnAsyncHandlerTests
    {
        private readonly Mock<ITicketRepository> _ticketRepositoryMock;
        private readonly Mock<IDropBoxClient> _dropBoxClientMock;
        private readonly Mock<ITicketTransitionRuleRepository> _ticketTransitionRuleRepositoryMock;
        private readonly TicketService _service;

        public MoveToColumnAsyncHandlerTests()
        {
            _ticketRepositoryMock = new Mock<ITicketRepository>();
            _dropBoxClientMock = new Mock<IDropBoxClient>();
            _ticketTransitionRuleRepositoryMock = new Mock<ITicketTransitionRuleRepository>();

            var mockMapper = new MapperConfiguration(cfg => cfg.AddProfile(new GetAllAsyncHandlerTests.MappingProfile())); // Убедитесь, что у вас есть правильный MappingProfile
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
        public async Task MoveToColumnAsync_ShouldMoveTicket_WhenTransitionIsAllowed()
        {
            var request = new MoveTicketRequest
            {
                TicketId = Guid.NewGuid(),
                NewColumnId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                CommitLink = "http://commit-link.com",
                Attachments = new[] { new Mock<IFormFile>().Object }
            };

            var ticket = new Ticket
            {
                TicketId = request.TicketId,
                ColumnId = Guid.NewGuid(),
                Description = "Some description"
            };

            var rule = new TicketTransitionRule
            {
                IsAllowed = true,
                RequiredValidations = TransitionValidationType.CommitLink | TransitionValidationType.Attachment,
                UserId = request.UserId
            };

            _ticketRepositoryMock.Setup(repo => repo.GetByIdAsync(request.TicketId)).ReturnsAsync(ticket);
            _ticketTransitionRuleRepositoryMock.Setup(repo => repo.GetRuleForTicketAsync(ticket.TicketId, ticket.ColumnId, request.NewColumnId)).ReturnsAsync(rule);
            _dropBoxClientMock.Setup(client => client.UploadFileAsync(It.IsAny<IFormFile>(), It.IsAny<string>())).ReturnsAsync("uploadedUrl");
            
            await _service.MoveToColumnAsync(request);
            
            _ticketRepositoryMock.Verify(repo => repo.UpdateAsync(It.Is<Ticket>(t => t.ColumnId == request.NewColumnId)), Times.Once);
        }

        [Fact]
        public async Task MoveToColumnAsync_ShouldThrowException_WhenTicketNotFound()
        {
            var request = new MoveTicketRequest
            {
                TicketId = Guid.NewGuid(),
                NewColumnId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };

            _ticketRepositoryMock.Setup(repo => repo.GetByIdAsync(request.TicketId)).ReturnsAsync((Ticket)null);
            
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.MoveToColumnAsync(request));
            Assert.Equal("Ticket not found.", exception.Message);
        }

        [Fact]
        public async Task MoveToColumnAsync_ShouldThrowException_WhenTransitionIsNotAllowed()
        {
            var request = new MoveTicketRequest
            {
                TicketId = Guid.NewGuid(),
                NewColumnId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };

            var ticket = new Ticket
            {
                TicketId = request.TicketId,
                ColumnId = Guid.NewGuid(),
                Description = "Some description"
            };

            var rule = new TicketTransitionRule
            {
                IsAllowed = false,
                RequiredValidations = TransitionValidationType.None
            };

            _ticketRepositoryMock.Setup(repo => repo.GetByIdAsync(request.TicketId)).ReturnsAsync(ticket);
            _ticketTransitionRuleRepositoryMock.Setup(repo => repo.GetRuleForTicketAsync(ticket.TicketId, ticket.ColumnId, request.NewColumnId)).ReturnsAsync(rule);
            
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.MoveToColumnAsync(request));
            Assert.Equal("Transition is not allowed.", exception.Message);
        }

        [Fact]
        public async Task MoveToColumnAsync_ShouldThrowException_WhenValidationFails()
        {
            var request = new MoveTicketRequest
            {
                TicketId = Guid.NewGuid(),
                NewColumnId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };

            var ticket = new Ticket
            {
                TicketId = request.TicketId,
                ColumnId = Guid.NewGuid(),
                Description = "Some description"
            };

            var rule = new TicketTransitionRule
            {
                IsAllowed = true,
                RequiredValidations = TransitionValidationType.Attachment,
                UserId = request.UserId
            };

            _ticketRepositoryMock.Setup(repo => repo.GetByIdAsync(request.TicketId)).ReturnsAsync(ticket);
            _ticketTransitionRuleRepositoryMock.Setup(repo => repo.GetRuleForTicketAsync(ticket.TicketId, ticket.ColumnId, request.NewColumnId)).ReturnsAsync(rule);
            
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.MoveToColumnAsync(request));
            Assert.Equal("Transition requires an attachment.", exception.Message);
        }

        [Fact]
        public async Task MoveToColumnAsync_ShouldThrowException_WhenUserIsNotAssignedToTicket()
        {
            var request = new MoveTicketRequest
            {
                TicketId = Guid.NewGuid(),
                NewColumnId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };

            var ticket = new Ticket
            {
                TicketId = request.TicketId,
                ColumnId = Guid.NewGuid(),
                Description = "Some description"
            };

            var rule = new TicketTransitionRule
            {
                IsAllowed = true,
                RequiredValidations = TransitionValidationType.None,
                UserId = Guid.NewGuid()
            };

            _ticketRepositoryMock.Setup(repo => repo.GetByIdAsync(request.TicketId)).ReturnsAsync(ticket);
            _ticketTransitionRuleRepositoryMock.Setup(repo => repo.GetRuleForTicketAsync(ticket.TicketId, ticket.ColumnId, request.NewColumnId)).ReturnsAsync(rule);
            
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.MoveToColumnAsync(request));
            Assert.Equal("Only the assigned user can move this ticket.", exception.Message);
        }
    }
}
