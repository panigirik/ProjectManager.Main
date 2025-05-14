using Moq;
using ProjectManager.Domain.Interfaces;
using ProjectManager.Application.Services;
using Xunit;

namespace Application.Tests.ServicesTests.BoardServiceHandlerTests
{
    public class DeleteAsyncHandlerTests
    {
        private readonly Mock<IBoardRepository> _boardRepositoryMock;
        private readonly BoardService _service;

        public DeleteAsyncHandlerTests()
        {
            _boardRepositoryMock = new Mock<IBoardRepository>();
            _service = new BoardService(_boardRepositoryMock.Object, null); 
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallRepositoryDelete_WhenBoardIdIsValid()
        {
            var boardId = Guid.NewGuid();
            _boardRepositoryMock.Setup(x => x.DeleteAsync(boardId)).Returns(Task.CompletedTask);
            
            await _service.DeleteAsync(boardId);
            
            _boardRepositoryMock.Verify(x => x.DeleteAsync(boardId), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowException_WhenRepositoryDeleteFails()
        {
            var boardId = Guid.NewGuid();
            _boardRepositoryMock.Setup(x => x.DeleteAsync(boardId)).ThrowsAsync(new Exception("Database error"));
            
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.DeleteAsync(boardId));
            Assert.Equal("Database error", exception.Message);

            _boardRepositoryMock.Verify(x => x.DeleteAsync(boardId), Times.Once);
        }
    }
}