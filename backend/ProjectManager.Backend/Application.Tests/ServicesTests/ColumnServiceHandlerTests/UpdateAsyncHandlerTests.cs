using Moq;
using AutoMapper;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;
using ProjectManager.Application.Services;
using ProjectManager.Application.RequestsDTOs;
using Xunit;

namespace Application.Tests.ServicesTests.ColumnServiceHandlerTests
{
    public class UpdateAsyncHandlerTests
    {
        private readonly Mock<IColumnRepository> _columnRepositoryMock;
        private readonly ColumnService _service;

        public UpdateAsyncHandlerTests()
        {
            _columnRepositoryMock = new Mock<IColumnRepository>();
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            var mapper = mockMapper.CreateMapper();
            _service = new ColumnService(_columnRepositoryMock.Object, null,  mapper);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateColumn_WhenValidUpdateColumnRequestIsProvided()
        {
            var updateColumnRequest = new UpdateColumnRequest
            {
                ColumnName = "Updated Column"
            };
            
            await _service.UpdateAsync(updateColumnRequest);
            
            _columnRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Column>()), Times.Once); 
        }

        [Fact]
        public async Task UpdateAsync_ShouldMapUpdateColumnRequestToColumnEntity()
        {
            var updateColumnRequest = new UpdateColumnRequest
            {
                ColumnName = "Updated Column"
            };
            
            await _service.UpdateAsync(updateColumnRequest);
            
            _columnRepositoryMock.Verify(x => x.UpdateAsync(It.Is<Column>(c => c.ColumnName == "Updated Column")), Times.Once); // Проверяем, что в репозиторий передан объект с правильным значением
        }
    }
}
