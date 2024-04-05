using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Infrastructure.Interfaces;
using Moq;

namespace Tests;

public class AuthServiceTests
{
    private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IEncryptionService> _encryptionServiceMock;
        private Mock<ITokenService> _tokenServiceMock;
        private Mock<IActionService> _actionServiceMock;
        private AuthService _authService;

        public AuthServiceTests() 
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _encryptionServiceMock = new Mock<IEncryptionService>();
            _tokenServiceMock = new Mock<ITokenService>();
            _actionServiceMock = new Mock<IActionService>();

            _authService = new AuthService(
                                _userRepositoryMock.Object, 
                                _encryptionServiceMock.Object, 
                                _tokenServiceMock.Object,
                                _actionServiceMock.Object);
        }

        [Fact]
        public async Task RegisterAsync_NewUser_ReturnsSuccess()
        {
            // Arrange
            var registerDto = new RegisterDTO { Email = "test@example.com", Password = "testpassword" };
            byte[] passwordHash = new byte[] { }; 
            byte[] passwordSalt = new byte[] { };

            _userRepositoryMock.Setup(x => x.UserExists(registerDto.Email)).ReturnsAsync(false);
            _encryptionServiceMock.Setup(x => x.CreatePasswordHash(registerDto.Password, out passwordHash, out passwordSalt));
            _userRepositoryMock.Setup(x => x.AddUser(It.IsAny<User>())); 

            // Act
            var result = await _authService.RegisterAsync(registerDto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("User registered successfully.", result.Message);

            // Verify mocks
            _userRepositoryMock.Verify(x => x.UserExists(registerDto.Email), Times.Once);
            _encryptionServiceMock.Verify(x => x.CreatePasswordHash(registerDto.Password, out passwordHash, out passwordSalt), Times.Once);
            _userRepositoryMock.Verify(x => x.AddUser(It.IsAny<User>()), Times.Once); 
            _actionServiceMock.Verify(x => x.SendSuccessfulRegistrationEmail(registerDto.Email), Times.Once);
            _actionServiceMock.Verify(x => x.SendCreateUserMessage(It.IsAny<int>()), Times.Once); 
        }
}