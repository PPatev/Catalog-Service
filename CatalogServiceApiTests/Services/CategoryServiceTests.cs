using AutoFixture;
using Catalog_Service.Data.Entities;
using Catalog_Service.Interfaces;
using Catalog_Service.Models.Crud;
using Catalog_Service.Models.Dtos;
using Catalog_Service.Services;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace CatalogServiceApiTests.Services
{
    [TestFixture]
    public class CategoryServiceTests
    {
        private Fixture _fixture;
        private IEnumerable<Category> _categoriesData;
        private Mock<ICategoriesRepository> _categoriesRepoMock;
        private CategoryService _categoryService;
        

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _categoriesData = TestHelper.CreateData(_fixture);
            _categoriesRepoMock = new Mock<ICategoriesRepository>();

            _categoryService = new CategoryService(_categoriesRepoMock.Object);
        }

        [Test]
        public async Task GetAllCategoriesAsync_ShouldReturnCorrect()
        {
            _categoriesRepoMock.Setup(x => x.GetAllAsync()).ReturnsAsync(_categoriesData);

            var resultDtos = await _categoryService.GetAllCategoriesAsync();

            resultDtos.Should().NotBeNullOrEmpty().And.BeAssignableTo<IEnumerable<CategoryDto>>();
            resultDtos.Count().Should().Be(_categoriesData.Count());
            foreach (var category in _categoriesData)
            {
                var expectedResult = resultDtos.SingleOrDefault(x => x.CategoryId == category.Id && x.CategoryName.Equals(category.Name));
                expectedResult.Should().NotBeNull();
            }
        }

        [Test]
        public async Task GetCategoryAsync_OnExistingCategory_ShouldReturnNotNull()
        {
            var random = new Random();
            var categoryId = random.Next(1, 20);
            var category = _categoriesData.Single(x => x.Id == categoryId);

            _categoriesRepoMock.Setup(x => x.GetByIdAsync(categoryId)).ReturnsAsync(category);

            var resultDto = await _categoryService.GetCategoryAsync(categoryId);

            resultDto.Should().NotBeNull().And.BeAssignableTo<CategoryDetailsDto?>();
            resultDto?.CategoryName.Should().Be(category?.Name);
            resultDto?.CategoryItems.Count.Should().Be(category?.CategoryItems.Count);
        }

        [Test]
        public async Task GetCategoryAsync_OnNonExistingCategory_ShouldReturnNull()
        {
            var random = new Random();
            var categoryId = random.Next(21, 100);
            var category = _categoriesData.SingleOrDefault(x => x.Id == categoryId);

            _categoriesRepoMock.Setup(x => x.GetByIdAsync(categoryId)).ReturnsAsync(category);

            var resultDto = await _categoryService.GetCategoryAsync(categoryId);

            resultDto.Should().BeNull();
        }

        [Test]
        public async Task CreateCategoryAsync_ShouldReturnCorrect()
        {
            var createCategoryModel = new CreateCategoryModel { CategoryName = _fixture.Create<string>() };

            var resultDto = await _categoryService.CreateCategoryAsync(createCategoryModel);

            resultDto.Should().NotBeNull().And.BeAssignableTo<CategoryDetailsDto>();
            resultDto.CategoryName.Should().Be(createCategoryModel.CategoryName);
            resultDto.CategoryItems.Should().NotBeNull().And.BeEmpty();
            _categoriesRepoMock.Verify(x => x.AddAsync(It.IsAny<Category>()), Times.Once);
            _categoriesRepoMock.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateCategoryAsync_OnExistingCategory_ShouldReturnCorrect()
        {
            var random = new Random();
            var categoryId = random.Next(1, 20);
            var category = _categoriesData.Single(x => x.Id == categoryId);
            var updateCategoryModel = new UpdateCategoryModel { Id = category.Id, CategoryName = category.Name};

            _categoriesRepoMock.Setup(x => x.GetByIdAsync(categoryId)).ReturnsAsync(category);

            var resultDto = await _categoryService.UpdateCategoryAsync(updateCategoryModel);

            resultDto.Should().NotBeNull().And.BeAssignableTo<UpdateCategoryDto>();
            resultDto.CategoryId.Should().Be(updateCategoryModel.Id);
            resultDto.CategoryName.Should().Be(updateCategoryModel.CategoryName);
            resultDto.CategoryItems.Should().NotBeNullOrEmpty();

            foreach (var item in category.CategoryItems)
            {
                var resultItem = resultDto.CategoryItems.SingleOrDefault(x => x.ItemId == item.Id && x.ItemName.Equals(item.Name));
                resultItem.Should().NotBeNull();
            }

            _categoriesRepoMock.Verify(x => x.GetByIdAsync(categoryId), Times.Once);
            _categoriesRepoMock.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateCategoryAsync_OnNonExistingCategory_ShouldReturnCorrect()
        {
            var random = new Random();
            var categoryId = random.Next(21, 100);
            var category = _categoriesData.SingleOrDefault(x => x.Id == categoryId);
            var updateCategoryModel = new UpdateCategoryModel { Id = categoryId, CategoryName = _fixture.Create<string>() };

            _categoriesRepoMock.Setup(x => x.GetByIdAsync(categoryId)).ReturnsAsync(category);

            var resultDto = await _categoryService.UpdateCategoryAsync(updateCategoryModel);

            resultDto.Should().NotBeNull().And.BeAssignableTo<UpdateCategoryDto>();
            resultDto.CategoryId.Should().BeNull();
            _categoriesRepoMock.Verify(x => x.GetByIdAsync(categoryId), Times.Once);
        }

        [Test]
        public async Task DeleteCategoryAsync_OnExistingCategory_ShouldReturnCorrect()
        {
            var random = new Random();
            var categoryId = random.Next(1, 20);
            var category = _categoriesData.Single(x => x.Id == categoryId);
            
            _categoriesRepoMock.Setup(x => x.GetByIdAsync(categoryId)).ReturnsAsync(category);

            var resultDto = await _categoryService.DeleteCategoryAsync(categoryId);

            resultDto.Should().NotBeNull().And.BeAssignableTo<DeleteCategoryDto>();
            resultDto.CategoryId.Should().Be(category.Id);
           
            _categoriesRepoMock.Verify(x => x.GetByIdAsync(categoryId), Times.Once);
            _categoriesRepoMock.Verify(x => x.Delete(category), Times.Once);
            _categoriesRepoMock.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteCategoryAsync_OnNonExistingCategory_ShouldReturnCorrect()
        {
            var random = new Random();
            var categoryId = random.Next(21, 100);
            var category = _categoriesData.SingleOrDefault(x => x.Id == categoryId);

            _categoriesRepoMock.Setup(x => x.GetByIdAsync(categoryId)).ReturnsAsync(category);

            var resultDto = await _categoryService.DeleteCategoryAsync(categoryId);

            resultDto.Should().NotBeNull().And.BeAssignableTo<DeleteCategoryDto>();
            resultDto.CategoryId.Should().BeNull();
            _categoriesRepoMock.Verify(x => x.GetByIdAsync(categoryId), Times.Once);
        }
    }
}
