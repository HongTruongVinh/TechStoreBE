using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using TechStore.Common.Constants;
using TechStore.Common.Enums;
using TechStore.Data.Entities;
using TechStore.Data.Repositories.Interfaces;
using TechStore.Data.UnitOfWork;
using TechStore.Model.DTOs.Cart;
using TechStore.Service.Implementations;
using Xunit;

namespace TechStore.UnitTests.Services
{
    public class CartServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUow;
        private readonly Mock<ICartItemRepository> _mockCartItemRepo;
        private readonly Mock<IUserRepository> _mockUserRepo;
        private readonly Mock<IProductVariantOptionRepository> _mockProductVariantOptionRepo;
        private readonly CartService _cartService;

        public CartServiceTests()
        {
            _mockUow = new Mock<IUnitOfWork>();
            _mockCartItemRepo = new Mock<ICartItemRepository>();
            _mockUserRepo = new Mock<IUserRepository>();
            _mockProductVariantOptionRepo = new Mock<IProductVariantOptionRepository>();

            _mockUow.Setup(u => u.CartItems).Returns(_mockCartItemRepo.Object);
            _mockUow.Setup(u => u.Users).Returns(_mockUserRepo.Object);
            _mockUow.Setup(u => u.ProductVariantOptions).Returns(_mockProductVariantOptionRepo.Object);

            _cartService = new CartService(_mockUow.Object, null!);
        }

        #region Helper Methods

        private static User CreateDummyUser(string publicId = "USR-001", Guid? id = null)
        {
            return new User
            {
                Id = id ?? Guid.NewGuid(),
                PublicId = publicId,
                FirstName = "Vinh",
                LastName = "Nguyen",
                PhoneNumber = "0987654321",
                PasswordHash = "hashed_password",
                Address = "123 Main St",
                City = "Hanoi",
                District = "Cau Giay",
                RoleId = ERole.Customer,
                Status = EUserStatus.Active,
                CreatedAt = DateTime.UtcNow
            };
        }

        private static ProductVariantOption CreateDummyProductVariantOption(
            string publicId = "PVO-001",
            Guid? id = null,
            decimal price = 25000000m)
        {
            return new ProductVariantOption
            {
                Id = id ?? Guid.NewGuid(),
                PublicId = publicId,
                ProductVariantId = Guid.NewGuid(),
                Name = "Titan Tu Nhien",
                ImageUrl = "https://example.com/natural-titanium.jpg",
                Stock = 20,
                Price = price,
                ImportPrice = price * 0.85m,
                SoldCount = 5,
                CreatedAt = DateTime.UtcNow,
                ProductVariant = new ProductVariant
                {
                    Id = Guid.NewGuid(),
                    PublicId = "PV-001",
                    ProductId = Guid.NewGuid(),
                    Name = "256GB",
                    Description = "Phien ban 256GB",
                    Price = price,
                    ImportPrice = price * 0.85m,
                    CreatedAt = DateTime.UtcNow,
                    Product = new Product
                    {
                        Id = Guid.NewGuid(),
                        PublicId = "PROD-001",
                        CategoryId = Guid.NewGuid(),
                        CategoryPublicId = "CAT-001",
                        BrandId = Guid.NewGuid(),
                        BrandPublicId = "BRAND-001",
                        Name = "iPhone 15 Pro Max",
                        ShortDescription = "Flagship Apple 2023",
                        Description = "Chi tiet san pham iPhone 15 Pro Max",
                        Warranty = 12,
                        Slug = "iphone-15-pro-max",
                        Tags = new List<string> { "apple", "iphone", "flagship" },
                        MainImageUrl = "https://example.com/main-iphone.jpg",
                        GalleryImageUrls = new List<string> { "https://example.com/gallery1.jpg" },
                        IsFeatured = true,
                        SalePrice = price,
                        MinPrice = price,
                        MaxPrice = price,
                        CreatedAt = DateTime.UtcNow
                    }
                }
            };
        }

        private static CartItem CreateDummyCartItem(
            Guid userId,
            Guid productVariantOptionId,
            string publicId = "CART-001",
            int quantity = 1,
            decimal price = 25000000m)
        {
            return new CartItem
            {
                Id = Guid.NewGuid(),
                PublicId = publicId,
                UserId = userId,
                ProductVariantOptionId = productVariantOptionId,
                Quantity = quantity,
                TotalPrice = quantity * price,
                Discount = 0,
                CreatedAt = DateTime.UtcNow
            };
        }

        #endregion

        #region AddToCart Tests

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-10)]
        public async Task AddToCart_WhenQuantityLessThanOne_ShouldReturnBadRequest(int invalidQuantity)
        {
            // Arrange
            var userId = "USR-001";
            var model = new CartItemUpdateModel
            {
                ProductVariantOptionId = "PVO-001",
                Quantity = invalidQuantity
            };

            // Act
            var result = await _cartService.AddToCart(userId, model);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be(Messenger.BadRequest);
            result.Data.Should().BeNull();

            _mockProductVariantOptionRepo.Verify(r => r.GetProductVariantOptionDetailByPublicIdAsync(It.IsAny<string>()), Times.Never);
            _mockUserRepo.Verify(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
            _mockCartItemRepo.Verify(r => r.AddAsync(It.IsAny<CartItem>(), It.IsAny<CancellationToken>()), Times.Never);
            _mockUow.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task AddToCart_WhenProductVariantOptionNotFound_ShouldReturnNoExitData()
        {
            // Arrange
            var userId = "USR-001";
            var model = new CartItemUpdateModel
            {
                ProductVariantOptionId = "PVO-NONEXISTENT",
                Quantity = 1
            };
            var user = CreateDummyUser(userId);

            _mockProductVariantOptionRepo
                .Setup(r => r.GetProductVariantOptionDetailByPublicIdAsync(model.ProductVariantOptionId))
                .ReturnsAsync((ProductVariantOption?)null);
            _mockUserRepo
                .Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            // Act
            var result = await _cartService.AddToCart(userId, model);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be(Messenger.NoExitData);
            result.Data.Should().BeNull();

            _mockCartItemRepo.Verify(r => r.AddAsync(It.IsAny<CartItem>(), It.IsAny<CancellationToken>()), Times.Never);
            _mockUow.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task AddToCart_WhenUserNotFound_ShouldReturnNoExitData()
        {
            // Arrange
            var userId = "USR-NONEXISTENT";
            var model = new CartItemUpdateModel
            {
                ProductVariantOptionId = "PVO-001",
                Quantity = 1
            };
            var productVariantOption = CreateDummyProductVariantOption(model.ProductVariantOptionId);

            _mockProductVariantOptionRepo
                .Setup(r => r.GetProductVariantOptionDetailByPublicIdAsync(model.ProductVariantOptionId))
                .ReturnsAsync(productVariantOption);
            _mockUserRepo
                .Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _cartService.AddToCart(userId, model);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be(Messenger.NoExitData);
            result.Data.Should().BeNull();

            _mockCartItemRepo.Verify(r => r.AddAsync(It.IsAny<CartItem>(), It.IsAny<CancellationToken>()), Times.Never);
            _mockUow.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task AddToCart_WhenBothProductVariantOptionAndUserNotFound_ShouldReturnNoExitData()
        {
            // Arrange
            var userId = "USR-NONEXISTENT";
            var model = new CartItemUpdateModel
            {
                ProductVariantOptionId = "PVO-NONEXISTENT",
                Quantity = 1
            };

            _mockProductVariantOptionRepo
                .Setup(r => r.GetProductVariantOptionDetailByPublicIdAsync(model.ProductVariantOptionId))
                .ReturnsAsync((ProductVariantOption?)null);
            _mockUserRepo
                .Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _cartService.AddToCart(userId, model);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be(Messenger.NoExitData);
            result.Data.Should().BeNull();

            _mockCartItemRepo.Verify(r => r.AddAsync(It.IsAny<CartItem>(), It.IsAny<CancellationToken>()), Times.Never);
            _mockUow.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task AddToCart_WhenItemDoesNotExistAndCommitSucceeds_ShouldAddCartItemAndReturnSuccess()
        {
            // Arrange
            var userId = "USR-001";
            var model = new CartItemUpdateModel
            {
                ProductVariantOptionId = "PVO-001",
                Quantity = 2
            };
            var user = CreateDummyUser(userId);
            var productVariantOption = CreateDummyProductVariantOption(model.ProductVariantOptionId, price: 20000000m);

            _mockProductVariantOptionRepo
                .Setup(r => r.GetProductVariantOptionDetailByPublicIdAsync(model.ProductVariantOptionId))
                .ReturnsAsync(productVariantOption);
            _mockUserRepo
                .Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            _mockCartItemRepo
                .Setup(r => r.FindOneAsync(It.IsAny<Expression<Func<CartItem, bool>>>()))
                .ReturnsAsync((CartItem?)null);
            _mockUow
                .Setup(u => u.CommitAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            CartItem? capturedCartItem = null;
            _mockCartItemRepo
                .Setup(r => r.AddAsync(It.IsAny<CartItem>(), It.IsAny<CancellationToken>()))
                .Callback<CartItem, CancellationToken>((item, _) => capturedCartItem = item)
                .Returns(Task.CompletedTask);

            // Act
            var result = await _cartService.AddToCart(userId, model);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Message.Should().Be(Messenger.SuccessFull);
            result.Data.Should().NotBeNull();

            // Verify captured entity added to repository
            capturedCartItem.Should().NotBeNull();
            capturedCartItem!.UserId.Should().Be(user.Id);
            capturedCartItem.PublicId.Should().Be($"{userId}-{productVariantOption.PublicId}");
            capturedCartItem.ProductVariantOptionId.Should().Be(productVariantOption.Id);
            capturedCartItem.Quantity.Should().Be(model.Quantity);
            capturedCartItem.TotalPrice.Should().Be(model.Quantity * productVariantOption.Price);
            capturedCartItem.Discount.Should().Be(0);

            // Verify returned DTO
            result.Data!.Id.Should().Be($"{userId}-{productVariantOption.PublicId}");
            result.Data.ProductVariantOptionId.Should().Be(productVariantOption.PublicId);
            result.Data.Quantity.Should().Be(model.Quantity);
            result.Data.Price.Should().Be(productVariantOption.Price);
            result.Data.TotalPrice.Should().Be(model.Quantity * productVariantOption.Price);
            result.Data.ProductName.Should().Be(productVariantOption.ProductVariant.Product.Name);
            result.Data.VariantName.Should().Be(productVariantOption.ProductVariant.Name);
            result.Data.OptionName.Should().Be(productVariantOption.Name);
            result.Data.MainImageUrl.Should().Be(productVariantOption.ImageUrl);
            result.Data.Stock.Should().Be(productVariantOption.Stock);

            _mockCartItemRepo.Verify(r => r.AddAsync(It.IsAny<CartItem>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockUow.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task AddToCart_WhenItemDoesNotExistAndCommitFails_ShouldReturnSystemError()
        {
            // Arrange
            var userId = "USR-001";
            var model = new CartItemUpdateModel
            {
                ProductVariantOptionId = "PVO-001",
                Quantity = 3
            };
            var user = CreateDummyUser(userId);
            var productVariantOption = CreateDummyProductVariantOption(model.ProductVariantOptionId);

            _mockProductVariantOptionRepo
                .Setup(r => r.GetProductVariantOptionDetailByPublicIdAsync(model.ProductVariantOptionId))
                .ReturnsAsync(productVariantOption);
            _mockUserRepo
                .Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            _mockCartItemRepo
                .Setup(r => r.FindOneAsync(It.IsAny<Expression<Func<CartItem, bool>>>()))
                .ReturnsAsync((CartItem?)null);
            _mockUow
                .Setup(u => u.CommitAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(0); // Commit failed

            // Act
            var result = await _cartService.AddToCart(userId, model);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be(Messenger.SystemError);
            result.Data.Should().BeNull();

            _mockCartItemRepo.Verify(r => r.AddAsync(It.IsAny<CartItem>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockUow.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task AddToCart_WhenItemAlreadyExists_ShouldReturnExistingItemWithoutCallingAddOrCommit()
        {
            // Arrange
            var userId = "USR-001";
            var model = new CartItemUpdateModel
            {
                ProductVariantOptionId = "PVO-001",
                Quantity = 2
            };
            var user = CreateDummyUser(userId);
            var productVariantOption = CreateDummyProductVariantOption(model.ProductVariantOptionId, price: 15000000m);
            var existingCartItem = CreateDummyCartItem(
                userId: user.Id,
                productVariantOptionId: productVariantOption.Id,
                publicId: $"{userId}-{productVariantOption.PublicId}",
                quantity: 1,
                price: productVariantOption.Price);

            _mockProductVariantOptionRepo
                .Setup(r => r.GetProductVariantOptionDetailByPublicIdAsync(model.ProductVariantOptionId))
                .ReturnsAsync(productVariantOption);
            _mockUserRepo
                .Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            _mockCartItemRepo
                .Setup(r => r.FindOneAsync(It.IsAny<Expression<Func<CartItem, bool>>>()))
                .ReturnsAsync(existingCartItem);

            // Act
            var result = await _cartService.AddToCart(userId, model);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Message.Should().Be(Messenger.SuccessFull);
            result.Data.Should().NotBeNull();

            // Should reflect existing cart item values
            result.Data!.Id.Should().Be(existingCartItem.PublicId);
            result.Data.Quantity.Should().Be(existingCartItem.Quantity);
            result.Data.TotalPrice.Should().Be(productVariantOption.Price * existingCartItem.Quantity);
            result.Data.ProductVariantOptionId.Should().Be(productVariantOption.PublicId);

            _mockCartItemRepo.Verify(r => r.AddAsync(It.IsAny<CartItem>(), It.IsAny<CancellationToken>()), Times.Never);
            _mockUow.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        #endregion
    }
}
