using AutoMapper;
using Moq;
using ProjectManager.Application.DTOs;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;
using ProjectManager.Application.Exceptions;
using ProjectManager.Application.Services;
using Xunit;

namespace Application.Tests.ServicesTests.BoardServiceHandlerTests
{
    public class GetByIdAsyncHandlerTests
    {
        private readonly Mock<IBoardRepository> _boardRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly BoardService _service;

        public GetByIdAsyncHandlerTests()
        {
            _boardRepositoryMock = new Mock<IBoardRepository>();
            _mapperMock = new Mock<IMapper>();

            _service = new BoardService(_boardRepositoryMock.Object, _mapperMock.Object, null, null, null);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnMappedBoardDto_WhenBoardExists()
        {
            var boardId = Guid.NewGuid();
            var board = new Board { BoardId = boardId, BoardName = "Board 1" };
            var boardDto = new BoardDto { BoardId = boardId, BoardName = "Board 1" };

            _boardRepositoryMock.Setup(x => x.GetByIdAsync(boardId)).ReturnsAsync(board);
            _mapperMock.Setup(x => x.Map<BoardDto>(board)).Returns(boardDto);
            
            var result = await _service.GetByIdAsync(boardId);
            
            Assert.NotNull(result);
            Assert.Equal(boardDto.BoardId, result.BoardId);
            Assert.Equal(boardDto.BoardName, result.BoardName);
            _boardRepositoryMock.Verify(x => x.GetByIdAsync(boardId), Times.Once);
            _mapperMock.Verify(x => x.Map<BoardDto>(board), Times.Once);
        }
        
    }
}
