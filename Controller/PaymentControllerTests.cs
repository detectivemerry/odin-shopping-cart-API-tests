using OdinShopping.Services;
using FakeItEasy;
using FluentAssertions;
using OdinShopping.Models;
using Microsoft.AspNetCore.Mvc;
using OdinShopping.Controllers;
using OdinShopping.Exceptions;

namespace OdinShopping.Tests.Controller
{
    public class PaymentControllerTests
    {
        private readonly IPaymentService _paymentService;
        private PaymentController ControllerUnderTest { get; }
        public PaymentControllerTests()
        {
            _paymentService = A.Fake<IPaymentService>();
            ControllerUnderTest = new PaymentController(_paymentService);
        }

        [Fact]
        public async void PaymentController_Add_ReturnsOk()
        {
            //Arrange
            PaymentDto paymentDto = new PaymentDto
            {
                PaymentType = "example",
                Amount = 10,
                CartId = 1,
            };

            Payment payment = new Payment
            {
                PaymentId = 1,
                PaymentType = "example",
                Amount = 10,
                TransactionDate = DateTime.UtcNow,
            };

            A.CallTo(() => _paymentService.AddPayment(paymentDto))
    .Returns(payment);

            //Action
            var result = await ControllerUnderTest.Add(paymentDto);

            //Assert
            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void PaymentController_Add_ReturnsBadRequest()
        {
            //Arrange
            PaymentDto paymentDto = new PaymentDto
            {
                PaymentType = "example",
                Amount = 10,
                CartId = 1,
            };

            A.CallTo(() => _paymentService.AddPayment(paymentDto))
            .Throws(new OdinShoppingException());

            //Action
            var result = await ControllerUnderTest.Add(paymentDto);

            //Assert
            result.Result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async void PaymentController_GetPaymentWithinDate_ReturnsOk()
        {
            //Arrange
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            List<Payment> payment = new List<Payment>();  

            A.CallTo(() => _paymentService.GetPaymentWithinDate(startDate, endDate))
            .Returns(payment);

            //Action
            var result = await ControllerUnderTest.GetPaymentWithinDate(startDate, endDate);

            //Assert
            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void PaymentController_GetPaymentWithinDate_ReturnsBadRequest()
        {
            //Arrange
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();

            A.CallTo(() => _paymentService.GetPaymentWithinDate(startDate, endDate))
            .Throws(new OdinShoppingException());

            //Action
            var result = await ControllerUnderTest.GetPaymentWithinDate(startDate, endDate);

            //Assert
            result.Result.Should().BeOfType<BadRequestResult>();
        }



    }
}
