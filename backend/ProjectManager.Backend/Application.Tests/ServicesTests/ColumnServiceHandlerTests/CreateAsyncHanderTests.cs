using Moq;
using AutoMapper;
using ProjectManager.Application.DTOs;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;
using ProjectManager.Application.Services;
using Xunit;

namespace Application.Tests.ServicesTests.ColumnServiceHandlerTests
{
    public class CreateAsyncHanderTests
    {
        private readonly Mock<IColumnRepository> _columnRepositoryMock;
        private readonly ColumnService _service;

        public CreateAsyncHanderTests()
        {
            _columnRepositoryMock = new Mock<IColumnRepository>();
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile()); 
            });
            var mapper = mockMapper.CreateMapper();
            _service = new ColumnService(_columnRepositoryMock.Object, mapper);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateColumn_WhenValidColumnDtoIsProvided()
        {
            var columnDto = new ColumnDto
            {
                ColumnName = "New Column"
            };
            
            await _service.CreateAsync(columnDto);
            
            _columnRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<Column>()), Times.Once); 
        }

        [Fact]
        public async Task CreateAsync_ShouldMapColumnDtoToColumnEntity()
        {
            var columnDto = new ColumnDto
            {
                ColumnName = "New Column"
            };
            
            await _service.CreateAsync(columnDto);
            
            _columnRepositoryMock.Verify(x => x.CreateAsync(It.Is<Column>(c => c.ColumnName == "New Column")), Times.Once); // Проверяем, что в репозиторий передан объект с правильным значением
        }
    }
}