using AutoMapper;
using Moq;
using ProjectManager.Application.DTOs;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;
using ProjectManager.Application.Services;
using Xunit;

namespace Application.Tests.ServicesTests.BoardServiceHandlerTests
{
    public class CreateAsyncHandlerTests
    {
        private readonly Mock<IBoardRepository> _boardRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly BoardService _service;

        public CreateAsyncHandlerTests()
        {
            _boardRepositoryMock = new Mock<IBoardRepository>();
            _mapperMock = new Mock<IMapper>();

            _service = new BoardService(_boardRepositoryMock.Object, _mapperMock.Object, null, null, null);
        }

        [Fact]
        public async Task CreateAsync_ShouldMapAndCallRepositoryCreate_WhenBoardDtoIsValid()
        {
            var boardDto = new BoardDto { BoardId = Guid.NewGuid(), BoardName = "New Board" };
            var board = new Board { BoardId = boardDto.BoardId, BoardName = boardDto.BoardName };

            _mapperMock.Setup(x => x.Map<Board>(boardDto)).Returns(board);
            _boardRepositoryMock.Setup(x => x.CreateAsync(board)).Returns(Task.CompletedTask);
            
            await _service.CreateAsync(boardDto);
            
            _mapperMock.Verify(x => x.Map<Board>(boardDto), Times.Once);
            _boardRepositoryMock.Verify(x => x.CreateAsync(board), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowException_WhenRepositoryCreateFails()
        {
            var boardDto = new BoardDto { BoardId = Guid.NewGuid(), BoardName = "New Board" };
            var board = new Board { BoardId = boardDto.BoardId, BoardName = boardDto.BoardName };

            _mapperMock.Setup(x => x.Map<Board>(boardDto)).Returns(board);
            _boardRepositoryMock.Setup(x => x.CreateAsync(board)).ThrowsAsync(new Exception("Database error"));
            
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.CreateAsync(boardDto));
            Assert.Equal("Database error", exception.Message);
            _mapperMock.Verify(x => x.Map<Board>(boardDto), Times.Once);
            _boardRepositoryMock.Verify(x => x.CreateAsync(board), Times.Once);
        }
    }
}
