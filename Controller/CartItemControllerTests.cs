using OdinShopping.Services;
using FakeItEasy;
using FluentAssertions;
using OdinShopping.Controllers;
using OdinShopping.Models;
using Microsoft.AspNetCore.Mvc;
using OdinShopping.Exceptions;

namespace OdinShopping.Tests.Controller
{
    public class CartItemControllerTests
    {
        private readonly ICartItemService _cartItemService;
        private CartItemController ControllerUnderTest;
        public CartItemControllerTests()
        {
            _cartItemService = A.Fake<ICartItemService>();
            ControllerUnderTest = new CartItemController(_cartItemService);
        }

        [Fact]
        public async void CartItemController_AddCartItem_ReturnsOk()
        {
            //Arrange
            CartItemDto cartItemDto = new CartItemDto
            {
                CartItemId = 1,
                ItemId = 1,
                Quantity = 10
            };

            A.CallTo(() => _cartItemService.AddCartItem(cartItemDto))
                .Returns(cartItemDto);

            //Act
            var result = await ControllerUnderTest.AddCartItem(cartItemDto);

            //Assert
            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void CartItemController_AddCartItem_ReturnsBadRequest()
        {

            CartItemDto cartItemDto = new CartItemDto
            {
                CartItemId = 1,
                ItemId = 1,
                Quantity = 10
            };

            A.CallTo(() => _cartItemService.AddCartItem(cartItemDto))
                .Throws(new OdinShoppingException());

            //Act
            var result = await ControllerUnderTest.AddCartItem(cartItemDto);

            //Assert
            result.Result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async void CartItemController_UpdateCartItem_ReturnsOk()
        {
            //Arrange
            CartItemDto cartItemDto = new CartItemDto
            {
                CartItemId = 1,
                ItemId = 1,
                Quantity = 10
            };

            A.CallTo(() => _cartItemService.UpdateCartItem(cartItemDto))
    .Returns(cartItemDto);

            //Action
            var result = await ControllerUnderTest.Update(cartItemDto);

            //Assert
            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void CartItemController_UpdateCartItem_ReturnsBadRequest()
        {
            //Arrange
            CartItemDto cartItemDto = new CartItemDto
            {
                CartItemId = 1,
                ItemId = 1,
                Quantity = 10
            };

            A.CallTo(() => _cartItemService.UpdateCartItem(cartItemDto))
    .Throws(new OdinShoppingException());

            //Action
            var result = await ControllerUnderTest.Update(cartItemDto);

            //Assert
            result.Result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async void CartItemController_Delete_ReturnsOk()
        {
            //Arrange
            int cartItemId = 1;

            A.CallTo(() => _cartItemService.DeleteCartItem(cartItemId))
                .Returns(true);

            //Action
            var result = await ControllerUnderTest.Delete(cartItemId);

            //Assert
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async void CartItemController_Delete_ReturnsBadRequest()
        {
            //Arrange
            int cartItemId = 1;

            A.CallTo(() => _cartItemService.DeleteCartItem(cartItemId))
                .Throws(new OdinShoppingException());

            //Action
            var result = await ControllerUnderTest.Delete(cartItemId);

            //Assert
            result.Should().BeOfType<BadRequestResult>();
        }
    }
}
