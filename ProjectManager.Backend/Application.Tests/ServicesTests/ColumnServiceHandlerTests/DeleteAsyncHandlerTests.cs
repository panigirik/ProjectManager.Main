using Moq;
using ProjectManager.Domain.Interfaces;
using ProjectManager.Application.Services;
using Xunit;

namespace Application.Tests.ServicesTests.ColumnServiceHandlerTests
{
    public class DeleteAsyncHandlerTests
    {
        private readonly Mock<IColumnRepository> _columnRepositoryMock;
        private readonly ColumnService _service;

        public DeleteAsyncHandlerTests()
        {
            _columnRepositoryMock = new Mock<IColumnRepository>();
            _service = new ColumnService(_columnRepositoryMock.Object, null);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallDeleteAsyncOnRepository_WhenValidIdIsProvided()
        {
            var columnId = Guid.NewGuid();

            await _service.DeleteAsync(columnId);

          _columnRepositoryMock.Verify(x => x.DeleteAsync(It.Is<Guid>(id => id == columnId)), Times.Once);
        }
    }
}