using Moq;
using AutoMapper;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;
using ProjectManager.Application.Services;
using Xunit;
using ProjectManager.Application.RequestsDTOs;
using ProjectManager.Domain.Interfaces.ExternalServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using ProjectManager.Application.DTOs;

namespace Application.Tests.ServicesTests.TicketServiceHandlerTests
{
    public class CreateTicketAsyncHandlerTests
    {
        private readonly Mock<ITicketRepository> _ticketRepositoryMock;
        private readonly Mock<IDropBoxClient> _dropBoxClientMock;
        private readonly Mock<ITicketTransitionRuleRepository> _ticketTransitionRuleRepositoryMock;
        private readonly TicketService _service;

        public CreateTicketAsyncHandlerTests()
        {
            _ticketRepositoryMock = new Mock<ITicketRepository>();
            _dropBoxClientMock = new Mock<IDropBoxClient>();
            _ticketTransitionRuleRepositoryMock = new Mock<ITicketTransitionRuleRepository>();
            
            var mockMapper = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile())); // Добавить свой профиль маппинга
            var mapper = mockMapper.CreateMapper();
            
            _service = new TicketService(
                _ticketRepositoryMock.Object,
                mapper,
                _dropBoxClientMock.Object,
                _ticketTransitionRuleRepositoryMock.Object
            );
        }

        [Fact]
        public async Task CreateTicketAsync_ShouldCreateTicketWithoutAttachments_WhenNoAttachmentsProvided()
        {
            var ticketRequest = new CreateTicketRequest
            {
                Description = "Test Ticket without attachments",
                Attachments = Array.Empty<IFormFile>()
            };
            
            var result = await _service.CreateTicketAsync(ticketRequest);
            
            Assert.NotNull(result);
            Assert.Equal(ticketRequest.Description, result.Description);
            Assert.Empty(result.Attachments); 
            _ticketRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Ticket>()), Times.Once);
            _dropBoxClientMock.Verify(client => client.UploadFileAsync(It.IsAny<IFormFile>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task CreateTicketAsync_ShouldCreateTicketWithAttachments_WhenAttachmentsProvided()
        {
            var ticketRequest = new CreateTicketRequest
            {
                Description = "Test Ticket with attachments",
                Attachments = new[] { new FormFile(null, 0, 0, "file1", "file1.txt") }
            };

            var uploadedUrl = "https://dropbox.com/file1";
            _dropBoxClientMock.Setup(client => client.UploadFileAsync(It.IsAny<IFormFile>(), It.IsAny<string>()))
                .ReturnsAsync(uploadedUrl);
            
            var result = await _service.CreateTicketAsync(ticketRequest);
            
            Assert.NotNull(result);
            Assert.Equal(ticketRequest.Description, result.Description);
            Assert.Single(result.Attachments);
            Assert.Equal(uploadedUrl, result.Attachments[0]); 
            _ticketRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Ticket>()), Times.Once);
            _dropBoxClientMock.Verify(client => client.UploadFileAsync(It.IsAny<IFormFile>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task CreateTicketAsync_ShouldThrowException_WhenDropboxUploadFails()
        {
            var ticketRequest = new CreateTicketRequest
            {
                Description = "Test Ticket with failed upload",
                Attachments = new[] { new FormFile(null, 0, 0, "file1", "file1.txt") }
            };

            _dropBoxClientMock.Setup(client => client.UploadFileAsync(It.IsAny<IFormFile>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Upload failed"));
            
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.CreateTicketAsync(ticketRequest));
            Assert.Equal("Upload failed", exception.Message);
            _ticketRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Ticket>()), Times.Never); // Ticket should not be created
            _dropBoxClientMock.Verify(client => client.UploadFileAsync(It.IsAny<IFormFile>(), It.IsAny<string>()), Times.Once);
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<CreateTicketRequest, Ticket>();
                CreateMap<UpdateTicketRequest, Ticket>();
                CreateMap<BoardDto, Board>();
            }
        }
    }
}
