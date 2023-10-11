using Microsoft.Extensions.Configuration;
using OdinShopping.Data;
using OdinShopping.Services;
using FakeItEasy;
using FluentAssertions;
using OdinShopping.Controllers;
using Microsoft.AspNetCore.Mvc;
using OdinShopping.Exceptions;

namespace OdinShopping.Tests.Controller
{
    public class AuthControllerTests
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        AuthController ControllerUnderTest;

        public AuthControllerTests()
        {
            _configuration = A.Fake<IConfiguration>();
            _userService = A.Fake<IUserService>();
            ControllerUnderTest = new AuthController(_context, _configuration, _userService);
        }

        [Fact]
        public void AuthController_Get_ReturnsOk()
        {
            //Arrange
            string username = "validusername";
            A.CallTo(() => _userService.GetUserName()).Returns(username);

            //Action
            var result = ControllerUnderTest.GetMe();

            //Assert
            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void AuthController_Get_ReturnsBadRequest()
        {
            //Arrange
            A.CallTo(() => _userService.GetUserName()).Throws(new OdinShoppingException());

            //Action
            var result = ControllerUnderTest.GetMe();

            //Assert
            result.Result.Should().BeOfType<BadRequestResult>();
        }
    }
}
