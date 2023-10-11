using OdinShopping.Services;
using FakeItEasy;
using FluentAssertions;
using OdinShopping.Controllers;
using OdinShopping.Models;
using Microsoft.AspNetCore.Mvc;
using OdinShopping.Exceptions;

namespace OdinShopping.Tests.Controller
{
    public class CategoryControllerTests
    {
        private readonly ICategoryService _categoryService;
        private CategoryController ControllerUnderTest { get; }
        public CategoryControllerTests()
        {
            _categoryService = A.Fake<ICategoryService>();
            ControllerUnderTest = new CategoryController(_categoryService);
        }

        [Fact]
        public async void CategoryController_Get_ReturnsOk()
        {
            List<Category> categories = new List<Category> { 
                new Category{ CategoryId = 1, CategoryName = "Cat1" },
                new Category{ CategoryId = 2, CategoryName = "Cat2" },
            };

            A.CallTo(()=> _categoryService.GetCategory()).Returns(categories);

            var result = await ControllerUnderTest.Get();
            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void CategoryController_Get_ReturnsNotFound()
        {
            List<Category> categories = new List<Category>();

            A.CallTo(() => _categoryService.GetCategory()).Returns(categories);

            var result = await ControllerUnderTest.Get();
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async void CategoryController_Add_ReturnsOk()
        {
            string categoryName = "cat1";
            Category category = new Category{ CategoryId = 1, CategoryName = "cat1" };

            A.CallTo(() => _categoryService.AddCategory(categoryName)).Returns(category);

            var result = await ControllerUnderTest.Add(categoryName);
            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void CategoryController_Add_ReturnsBadRequest()
        {
            string categoryName = "cat1";

            A.CallTo(() => _categoryService.AddCategory(categoryName))
                .Throws(new OdinShoppingException());

            var result = await ControllerUnderTest.Add(categoryName);

            result.Result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async void CategoryController_Update_ReturnsOk()
        {
            string categoryName = "cat2";
            int categoryId = 2;
            Category category = new Category { CategoryId = categoryId, CategoryName = categoryName };
            
            A.CallTo(() => _categoryService.UpdateCategory(categoryName, categoryId))
                .Returns(category);

            var result = await ControllerUnderTest.Update(categoryName, categoryId);

            result.Result.Should().BeOfType<OkObjectResult>();

        }

        [Fact]
        public async void CategoryController_Update_ReturnsBadRequest()
        {
            string categoryName = "cat2";
            int categoryId = 2;

            A.CallTo(() => _categoryService.UpdateCategory(categoryName, categoryId))
                .Throws(new OdinShoppingException());

            var result = await ControllerUnderTest.Update(categoryName, categoryId);

            result.Result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async void CategoryController_Delete_ReturnsOk()
        {
            int categoryId = 2;
            string categoryName = "Test";
            Category category = new Category { CategoryId = categoryId, CategoryName = categoryName };

            A.CallTo(() => _categoryService.DeleteCategory(categoryId))
                .Returns(true);

            var result = await ControllerUnderTest.Delete(categoryId);

            result.Result.Should().BeOfType<OkResult>();

        }

        [Fact]
        public async void CategoryController_Delete_ReturnsNotFound()
        {
            int categoryId = 2;

            A.CallTo(() => _categoryService.DeleteCategory(categoryId))
                .Throws(new CategoryNotFoundException(categoryId));

            var result = await ControllerUnderTest.Delete(categoryId);

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async void CategoryController_Delete_ReturnsBadRequest()
        {
            int categoryId = 2;

            A.CallTo(() => _categoryService.DeleteCategory(categoryId))
                .Throws(new OdinShoppingException());

            var result = await ControllerUnderTest.Delete(categoryId);

            result.Result.Should().BeOfType<BadRequestResult>();
        }
    }
}
