using OdinShopping.Controllers;
using OdinShopping.Services;
using FakeItEasy;
using FluentAssertions;
using OdinShopping.Models;
using OdinShopping.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace OdinShopping.Tests.Controller
{
    public class CartControllerTests
    {
        private readonly ICartService _cartService;
        private CartController ControllerUnderTest { get; }
        public CartControllerTests()
        {
            _cartService = A.Fake<ICartService>();
            ControllerUnderTest = new CartController(_cartService);
        }

        [Fact]
        public async void CartController_Get_ReturnsOk()
        {
            //Arrange
            Cart cart = new Cart
            {
                CartId = 1,
                CartItems = new List<CartItem> { new CartItem() },
                UserId = 1,
                User = new User(),
                Payment = null
            };

            A.CallTo(() => _cartService.GetCartWithCartItemsAndItems())
                .Returns(cart);

            //Act
            var result = await ControllerUnderTest.Get();

            //Asset
            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void CartController_Get_ReturnsBadRequest()
        {
            //Arrange
            A.CallTo(() => _cartService.GetCartWithCartItemsAndItems())
                .Throws(new OdinShoppingException());

            //Act
            var result = await ControllerUnderTest.Get();

            //Assert
            result.Result.Should().BeOfType<BadRequestResult>();
        }
    }
}
