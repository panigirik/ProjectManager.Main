using AutoMapper;
using Moq;
using ProjectManager.Application.DTOs;
using ProjectManager.Application.Services;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;
using Xunit;

namespace Application.Tests.ServicesTests.BoardServiceHandlerTests
{
    public class GetAllAsyncHandlerTests
    {
        private readonly Mock<IBoardRepository> _boardRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly BoardService _service;

        public GetAllAsyncHandlerTests()
        {
            _boardRepositoryMock = new Mock<IBoardRepository>();
            _mapperMock = new Mock<IMapper>();

            _service = new BoardService(_boardRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnMappedBoardDtos_WhenBoardsExist()
        {
            var boards = new List<Board>
            {
                new Board { BoardId = Guid.NewGuid(), BoardName = "Board 1" },
                new Board { BoardId = Guid.NewGuid(), BoardName = "Board 2" }
            };

            var boardDtos = new List<BoardDto>
            {
                new BoardDto { BoardId = boards[0].BoardId, BoardName = boards[0].BoardName },
                new BoardDto { BoardId = boards[1].BoardId, BoardName = boards[1].BoardName }
            };

            _boardRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(boards);
            _mapperMock.Setup(x => x.Map<IEnumerable<BoardDto>>(boards)).Returns(boardDtos);
            
            var result = await _service.GetAllAsync();
            
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal(boardDtos[0].BoardName, result.ElementAt(0).BoardName);
            Assert.Equal(boardDtos[1].BoardName, result.ElementAt(1).BoardName);
            _boardRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
            _mapperMock.Verify(x => x.Map<IEnumerable<BoardDto>>(boards), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoBoardsExist()
        {
            var boards = new List<Board>();

            _boardRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(boards);
            _mapperMock.Setup(x => x.Map<IEnumerable<BoardDto>>(boards)).Returns(new List<BoardDto>());
            
            var result = await _service.GetAllAsync();
            
            Assert.NotNull(result);
            Assert.Empty(result);
            _boardRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
            _mapperMock.Verify(x => x.Map<IEnumerable<BoardDto>>(boards), Times.Once);
        }
    }
}
