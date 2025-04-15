using AutoMapper;
using ProjectManager.Domain.Entities;
using ProjectManager.Application.RequestsDTOs;
using ProjectManager.Application.DTOs;
using ProjectManager.Domain.Interfaces;
using ProjectManager.Application.Services;
using Xunit;
using Moq;
using ProjectManager.Domain.Interfaces.ExternalServices;

namespace Application.Tests.ServicesTests.TicketServiceHandlerTests
{
    public class GetAllAsyncHandlerTests
    {
        private readonly Mock<ITicketRepository> _ticketRepositoryMock;
        private readonly Mock<IDropBoxClient> _dropBoxClientMock;
        private readonly Mock<ITicketTransitionRuleRepository> _ticketTransitionRuleRepositoryMock;
        private readonly TicketService _service;

        public GetAllAsyncHandlerTests()
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
        public async Task GetAllAsync_ShouldReturnMappedTicketDtos_WhenTicketsExist()
        {
            var tickets = new List<Ticket>
            {
                new Ticket { TicketId = Guid.NewGuid(), ColumnId = Guid.NewGuid(), Description = "Test Ticket 1" },
                new Ticket { TicketId = Guid.NewGuid(), ColumnId = Guid.NewGuid(), Description = "Test Ticket 2" }
            };

            _ticketRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(tickets);
            
            var result = await _service.GetAllAsync();
            
            Assert.NotNull(result);
            Assert.IsType<List<GetTicketRequest>>(result);
            Assert.Equal(tickets.Count, result.Count());
            _ticketRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoTicketsExist()
        {
            var tickets = new List<Ticket>(); 

            _ticketRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(tickets);
            
            var result = await _service.GetAllAsync();
            
            Assert.NotNull(result);
            Assert.Empty(result);
            _ticketRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        }
        
        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Ticket, GetTicketRequest>()
                    .ForMember(dest => dest.TicketId, opt => opt.MapFrom(src => src.TicketId))
                    .ForMember(dest => dest.ColumnId, opt => opt.MapFrom(src => src.ColumnId))
                    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));
                
                CreateMap<Ticket, TicketDto>().ReverseMap();
                CreateMap<UpdateTicketRequest, Ticket>().ReverseMap();
            }
        }
    }
}
