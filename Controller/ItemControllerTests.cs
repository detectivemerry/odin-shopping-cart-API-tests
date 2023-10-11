using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using OdinShopping.Controllers;
using OdinShopping.Exceptions;
using OdinShopping.Migrations;
using OdinShopping.Models;
using OdinShopping.Services;

namespace OdinShopping.Tests.Controller
{
    public class ItemControllerTests
    {
        private readonly IItemService _itemService;
        private ItemController ControllerUnderTest { get; }
        public ItemControllerTests()
        {
            _itemService = A.Fake<IItemService>();
            ControllerUnderTest = new ItemController(_itemService);
        }

        [Fact]
        public async void ItemController_Get_ReturnOk()
        {
            //Arrange
            List<Item> items = new List<Item> {
                new Item{
                    ItemId = 1,
                    Author = "Test",
                    Name = "Testbook1",
                    Description = "TestDescription",
                    Category = new Category{ CategoryId = 1, CategoryName = "Python"},
                    Price = 11.11,
                    QuantityLeft = 10
                    },
                new Item{
                    ItemId = 2,
                    Author = "Test",
                    Name = "Testbook2",
                    Description = "TestDescription",
                    Category = new Category{ CategoryId = 1, CategoryName = "Python"},
                    Price = 11.11,
                    QuantityLeft = 10
                    },
            };
            A.CallTo(() => _itemService.GetAllIAvailableItems()).Returns(items);

            //Act
            var result = await ControllerUnderTest.Get();

            //Assert
            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void ItemController_Get_ReturnNotFound()
        {
            //Arrange
            List<Item> items = new List<Item>();
            A.CallTo(() => _itemService.GetAllIAvailableItems()).Returns(items);

            //Act
            var result = await ControllerUnderTest.Get();

            //Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async void ItemController_GetId_ReturnOk()
        {
            //Arrange
            Item item = new Item
                {
                    ItemId = 1,
                    Author = "Test",
                    Name = "Testbook1",
                    Description = "TestDescription",
                    Category = new Category { CategoryId = 1, CategoryName = "Python" },
                    Price = 11.11,
                    QuantityLeft = 10
                };
            int itemId = 1;
            A.CallTo(() => _itemService.GetItem(itemId)).Returns(item);

            //Act
            var result = await ControllerUnderTest.Get(itemId);

            //Assert
            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void ItemController_GetId_ReturnNotFound()
        {
            //Arrange
            Item item = new Item();
            int itemId = 1;

            A.CallTo(() => _itemService.GetItem(itemId))
                .Throws(new ItemNotFoundException(itemId));

            //Act
            var result = await ControllerUnderTest.Get(itemId);

            //Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async void ItemController_AddItem_ReturnOk()
        {
            //Arrange
            Item item = new Item
            {
                ItemId = 1,
                Author = "Test",
                Name = "Testbook1",
                Description = "TestDescription",
                Category = new Category { CategoryId = 1, CategoryName = "Python" },
                Price = 11.11,
                QuantityLeft = 10
            };

            A.CallTo(() => _itemService.AddItem(item)).Returns(item);

            //Act
            var result = await ControllerUnderTest.AddItem(item);

            //Assert
            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void ItemController_AddItem_ReturnBadRequest()
        {
            //Arrange
            Item item = new Item();

            A.CallTo(() => _itemService.AddItem(item))
                .Throws(new OdinShoppingException());

            //Act
            var result = await ControllerUnderTest.AddItem(item);

            //Assert
            result.Result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async void ItemController_UpdateItem_ReturnOk()
        {
            //Arrange
            ItemDto itemDto = new ItemDto
            {
                ItemId = 1,
                Author = "Test",
                Name = "Testbook1",
                Description = "TestDescription",
                Price = 11.11,
                QuantityLeft = 10
            };

            Item updatedItem = new Item
            {
                ItemId = 1,
                Author = "Test",
                Name = "Testbook1",
                Description = "TestDescription",
                Category = new Category { CategoryId = 1, CategoryName = "Python" },
                Price = 11.11,
                QuantityLeft = 10
            };

            A.CallTo(() => _itemService.UpdateItem(itemDto)).Returns(updatedItem);

            //Act
            var result = await ControllerUnderTest.UpdateItem(itemDto);

            //Assert
            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void ItemController_UpdateItem_ReturnNotFound()
        {
            //Arrange
            ItemDto itemDto = new ItemDto
            {
                ItemId = 1,
                Author = "Test",
                Name = "Testbook1",
                Description = "TestDescription",
                Price = 11.11,
                QuantityLeft = 10
            };

            A.CallTo(() => _itemService.UpdateItem(itemDto))
                .Throws(new ItemNotFoundException(itemDto.ItemId));

            //Act
            var result = await ControllerUnderTest.UpdateItem(itemDto);

            //Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async void ItemController_UpdateItem_ReturnBadRequest()
        {
            //Arrange
            ItemDto itemDto = new ItemDto
            {
                ItemId = 1,
                Author = "Test",
                Name = "Testbook1",
                Description = "TestDescription",
                Price = 11.11,
                QuantityLeft = 10
            };

            A.CallTo(() => _itemService.UpdateItem(itemDto))
                .Throws(new OdinShoppingException());

            //Act
            var result = await ControllerUnderTest.UpdateItem(itemDto);

            //Assert
            result.Result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async void ItemController_Delete_ReturnOk()
        {
            //Arrange
            int itemId = 1;
            A.CallTo(() => _itemService.DeleteItem(itemId)).Returns(true);

            //Act
            var result = await ControllerUnderTest.Delete(itemId);

            //Assert
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async void ItemController_Delete_ReturnNotFound()
        {
            //Arrange
            int itemId = 1;
            A.CallTo(() => _itemService.DeleteItem(itemId))
                .Throws(new ItemNotFoundException(itemId));

            //Act
            var result = await ControllerUnderTest.Delete(itemId);

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async void ItemController_Delete_ReturnBadRequest()
        {
            //Arrange
            int itemId = 1;
            A.CallTo(() => _itemService.DeleteItem(itemId))
                .Throws(new OdinShoppingException());

            //Act
            var result = await ControllerUnderTest.Delete(itemId);

            //Assert
            result.Should().BeOfType<BadRequestResult>();
        }
    }
}
