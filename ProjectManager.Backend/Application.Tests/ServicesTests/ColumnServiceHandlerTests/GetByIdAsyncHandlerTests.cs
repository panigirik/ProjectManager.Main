using Moq;
using AutoMapper;
using ProjectManager.Application.DTOs;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;
using ProjectManager.Application.Services;
using Xunit;

namespace Application.Tests.ServicesTests.ColumnServiceHandlerTests
{
    public class GetByIdAsyncHandlerTests
    {
        private readonly Mock<IColumnRepository> _columnRepositoryMock;
        private readonly IMapper _mapper;
        private readonly ColumnService _service;

        public GetByIdAsyncHandlerTests()
        {
            _columnRepositoryMock = new Mock<IColumnRepository>();
            var mockMapper = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
            _mapper = mockMapper.CreateMapper(); 
            _service = new ColumnService(_columnRepositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnColumnDto_WhenColumnExists()
        {
            var columnId = Guid.NewGuid();
            var column = new Column { ColumnId = columnId, ColumnName = "Column 1" };

            _columnRepositoryMock.Setup(x => x.GetByIdAsync(columnId)).ReturnsAsync(column);
            
            var result = await _service.GetByIdAsync(columnId);
            
            Assert.NotNull(result);
            Assert.IsType<ColumnDto>(result);
            Assert.Equal(columnId, result.ColumnId);
            Assert.Equal("Column 1", result.ColumnName);
        }
        
    }


}
