using Moq;
using PasswordManager.Services.Services;
using PasswordManager.Services.Interfaces.Encryption;
using PasswordManager.Services.Interfaces.Decryption;
using PasswordManager.Persistence.Contexts;
using Microsoft.Extensions.Configuration;
using PasswordManager.Core.Dto.Requests.Auth;
using Microsoft.EntityFrameworkCore;

namespace PasswordManager.Services.Tests
{
    public sealed class AuthServiceTests
    {
        private readonly Mock<IEncryptionService> _mockEncryptionService;
        private readonly Mock<IDecryptionService> _mockDecryptionService;
        private readonly Mock<PasswordManagerDbContext> _mockDbContext;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _mockEncryptionService = new Mock<IEncryptionService>();
            _mockDecryptionService = new Mock<IDecryptionService>();

            var mockDbContextOptions = new DbContextOptionsBuilder<PasswordManagerDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _mockDbContext = new Mock<PasswordManagerDbContext>(mockDbContextOptions);

            _mockConfiguration = new Mock<IConfiguration>();

            _authService = new AuthService(
                _mockConfiguration.Object,
                _mockDbContext.Object,
                _mockEncryptionService.Object,
                _mockDecryptionService.Object
            );
        }

        [Fact]
        public async Task Register_ShouldReturnResponse_WhenUserIsSuccessfullyRegistered()
        {
            var options = new DbContextOptionsBuilder<PasswordManagerDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var dbContext = new PasswordManagerDbContext(options);

            var authService = new AuthService(
                _mockConfiguration.Object,
                dbContext,
                _mockEncryptionService.Object,
                _mockDecryptionService.Object
            );

            var registerDto = new RegisterDto
            {
                Username = "testuser",
                MasterPassword = "password123",
                MasterPasswordRepeated = "password123"
            };

            var salt = new byte[] { 1, 2, 3 };
            var hashedPassword = new byte[] { 10, 20, 30 };
            var derivedKey = new byte[] { 40, 50, 60 };
            var encryptedPrivateKey = new byte[] { 100, 110, 120 };
            var encryptedAESKey = new byte[] { 130, 140, 150 };

            byte[] capturedAesKey = null;

            _mockEncryptionService
                .Setup(e => e.HashPassword(It.IsAny<string>(), It.IsAny<byte[]>(), 100_000, 32))
                .Returns(hashedPassword);

            _mockEncryptionService
                .Setup(e => e.EncryptWithAES(It.IsAny<byte[]>(), It.IsAny<byte[]>()))
                .Callback<byte[], byte[]>((data, key) =>
                {
                    capturedAesKey = key;
                })
                .Returns((byte[] data, byte[] key) =>
                {
                    if (key.SequenceEqual(derivedKey)) return encryptedAESKey;
                    if (key.SequenceEqual(capturedAesKey)) return encryptedPrivateKey;
                    return null;
                });

            _mockDecryptionService
                .Setup(d => d.DeriveKeyFromPassword(It.IsAny<string>(), It.IsAny<byte[]>()))
                .Returns(derivedKey);

            var response = await authService.Register(registerDto, CancellationToken.None);

            Assert.NotNull(response);
            Assert.NotNull(response.Data);
            Assert.Equal("testuser", response.Data.Username);

            _mockEncryptionService.Verify(e => e.HashPassword(registerDto.MasterPassword, It.IsAny<byte[]>(), 100_000, 32), Times.Once);
            _mockEncryptionService.Verify(e => e.EncryptWithAES(It.IsAny<byte[]>(), derivedKey), Times.Once);

            Assert.NotNull(capturedAesKey);
            _mockEncryptionService.Verify(e => e.EncryptWithAES(It.IsAny<byte[]>(), capturedAesKey), Times.Once);
        }
    }
}