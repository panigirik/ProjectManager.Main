using Moq;
using AutoMapper;
using ProjectManager.Application.DTOs;
using ProjectManager.Application.RequestsDTOs;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;
using ProjectManager.Application.Services;
using Xunit;

namespace Application.Tests.ServicesTests.ColumnServiceHandlerTests
{
    public class GetAllAsyncHandlerTests
    {
        private readonly Mock<IColumnRepository> _columnRepositoryMock;
        private readonly IMapper _mapper;
        private readonly ColumnService _service;

        public GetAllAsyncHandlerTests()
        {
            _columnRepositoryMock = new Mock<IColumnRepository>();
            var mockMapper = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
            _mapper = mockMapper.CreateMapper(); 
            _service = new ColumnService(_columnRepositoryMock.Object, null,  _mapper);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnListOfColumnDtos_WhenColumnsExist()
        {
            var columns = new List<Column>
            {
                new Column { ColumnId = Guid.NewGuid(), ColumnName = "Column 1" },
                new Column { ColumnId = Guid.NewGuid(), ColumnName = "Column 2" }
            };

            _columnRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(columns);
            
            var result = await _service.GetAllAsync();
            
            Assert.NotNull(result);
            Assert.IsType<List<ColumnDto>>(result);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoColumnsExist()
        {
            _columnRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<Column>());
            
            var result = await _service.GetAllAsync();
            
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Column, ColumnDto>();
            CreateMap<ColumnDto, Column>()
                .ForMember(dest => dest.ColumnName, opt
                    => opt.MapFrom(src => src.ColumnName));
            CreateMap<UpdateColumnRequest, Column>().ReverseMap();
        }
    }
}
