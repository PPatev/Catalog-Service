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
    public class CategoryItemServiceTests
    {
        private Fixture _fixture;
        private IEnumerable<Category> _categoriesData;
        private Mock<ICategoriesRepository> _categoriesRepoMock;
        private CategoryItemService _itemService;


        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _categoriesData = TestHelper.CreateData(_fixture);
            _categoriesRepoMock = new Mock<ICategoriesRepository>();

            _itemService = new CategoryItemService(_categoriesRepoMock.Object);
        }

        [Test]
        public async Task GetCategoryItemsAsync_OnNonExistingCategory_ShouldReturnNull()
        {
            Category category = null;
            var categoryId = _fixture.Create<int>();
            var pageIndex = _fixture.Create<int>();
            var pageSize = _fixture.Create<int>();

            _categoriesRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(category);

            var result = await _itemService.GetCategoryItemsAsync(categoryId, pageIndex, pageSize);

            result.Should().BeNull();
            _categoriesRepoMock.Verify(x => x.GetByIdAsync(categoryId), Times.Once);
        }

        [Test]
        public async Task GetCategoryItemsAsync_OnExistingCategory_ShouldReturnCorrect()
        {
            var random = new Random();
            var categoryId = random.Next(1, 20);
            
            var category = _categoriesData.Single(x => x.Id == categoryId);
            var pageSize = random.Next(1, category.CategoryItems.Count);

            _categoriesRepoMock.Setup(x => x.GetByIdAsync(categoryId)).ReturnsAsync(category);

            var result = await _itemService.GetCategoryItemsAsync(categoryId, 0, pageSize);

            result.Should().NotBeNullOrEmpty().And.BeAssignableTo<IEnumerable<CategoryItemDetailsDto?>?>();
            result?.Count().Should().Be(pageSize);
            _categoriesRepoMock.Verify(x => x.GetByIdAsync(categoryId), Times.Once);
        }

        [Test]
        public async Task GetCategoryItemAsync_OnNonExistingCategory_ShouldReturnCorrect()
        {
            Category category = null;
            var categoryId = _fixture.Create<int>();

            _categoriesRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(category);

            var result = await _itemService.GetCategoryItemAsync(categoryId, _fixture.Create<int>());

            result.Should().NotBeNull().And.BeAssignableTo<CategoryItemDetailsDto>();
            result?.CategoryId.Should().BeNull();
            _categoriesRepoMock.Verify(x => x.GetByIdAsync(categoryId), Times.Once);
        }

        [Test]
        public async Task GetCategoryItemAsync_OnNonExistingItem_ShouldReturnCorrect()
        {
            var random = new Random();
            var categoryId = random.Next(1, 20);

            var category = _categoriesData.Single(x => x.Id == categoryId);
            _categoriesRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(category);
            var itemId = category.CategoryItems.Select(x => x.Id).OrderByDescending(x => x).First() + _fixture.Create<int>();

            var result = await _itemService.GetCategoryItemAsync(categoryId, itemId);

            result.Should().NotBeNull().And.BeAssignableTo<CategoryItemDetailsDto>();
            result?.CategoryId.Should().NotBeNull();
            result?.ItemId.Should().BeNull();
            _categoriesRepoMock.Verify(x => x.GetByIdAsync(categoryId), Times.Once);
        }

        [Test]
        public async Task GetCategoryItemAsync_OnExistingCategoryAndItem_ShouldReturnCorrect()
        {
            var random = new Random();
            var categoryId = random.Next(1, 20);

            var category = _categoriesData.Single(x => x.Id == categoryId);
            _categoriesRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(category);
            var item = category.CategoryItems.First();

            var result = await _itemService.GetCategoryItemAsync(categoryId, item.Id);

            result.Should().NotBeNull().And.BeAssignableTo<CategoryItemDetailsDto>();
            result?.CategoryId.Should().NotBeNull();
            result?.CategoryName.Should().Be(category.Name);
            result?.ItemId.Should().NotBeNull();
            result?.ItemName.Should().Be(item.Name);
            _categoriesRepoMock.Verify(x => x.GetByIdAsync(categoryId), Times.Once);
        }

        [Test]
        public async Task CreateCategoryItemAsync_OnNonExistingCategory_ShouldReturnNull()
        {
            Category category = null;
            var random = new Random();
            var categoryId = random.Next(21, 100);

            _categoriesRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(category);
            var createItemModel = new CreateItemModel { CategoryId = categoryId };

            var resultDto = await _itemService.CreateCategoryItemAsync(createItemModel);

            resultDto.Should().BeNull();
            
            _categoriesRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
            _categoriesRepoMock.Verify(x => x.SaveAsync(), Times.Never);
        }

        [Test]
        public async Task CreateCategoryItemAsync_OnExistingCategory_ShouldReturnCorrect()
        {
            var random = new Random();
            var categoryId = random.Next(1, 20);
            var itemName = _fixture.Create<string>();

            var category = _categoriesData.Single(x => x.Id == categoryId);
            _categoriesRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(category);
            var createItemModel = new CreateItemModel { CategoryId = category.Id, ItemName = itemName };

            var resultDto = await _itemService.CreateCategoryItemAsync(createItemModel);

            resultDto.Should().NotBeNull();
            resultDto?.CategoryId.Should().Be(category.Id);
            resultDto?.CategoryName.Should().Be(category.Name);
            resultDto?.ItemName.Should().Be(itemName);

            _categoriesRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
            _categoriesRepoMock.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateCategoryItemAsync_OnNonExistingCategory_ShouldReturnNull()
        {
            Category category = null;
            var random = new Random();
            var categoryId = random.Next(21, 100);

            _categoriesRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(category);
            var updateItemModel = new UpdateItemModel { CategoryId = categoryId };

            var resultDto = await _itemService.UpdateCategoryItemAsync(updateItemModel);

            resultDto.Should().NotBeNull();
            resultDto.CategoryId.Should().BeNull();

            _categoriesRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
            _categoriesRepoMock.Verify(x => x.SaveAsync(), Times.Never);
        }

        [Test]
        public async Task UpdateCategoryItemAsync_OnNonExistingItem_ShouldReturnNull()
        {
            var random = new Random();
            var categoryId = random.Next(1, 20);

            var category = _categoriesData.Single(x => x.Id == categoryId);
            _categoriesRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(category);
            var itemId = category.CategoryItems.Select(x => x.Id).OrderByDescending(x => x).First() + _fixture.Create<int>();
            var updateItemModel = new UpdateItemModel { CategoryId = categoryId, ItemId = itemId };

            var result = await _itemService.UpdateCategoryItemAsync(updateItemModel);

            result.Should().NotBeNull().And.BeAssignableTo<UpdateCategoryItemDto>();
            result?.CategoryId.Should().NotBeNull();
            result?.ItemId.Should().BeNull();
            _categoriesRepoMock.Verify(x => x.GetByIdAsync(categoryId), Times.Once);
            _categoriesRepoMock.Verify(x => x.SaveAsync(), Times.Never);
        }

        [Test]
        public async Task UpdateCategoryItemAsync_OnExistingCategoryAndItem_ShouldReturnCorrect()
        {
            var random = new Random();
            var categoryId = random.Next(1, 20);
            var itemName = _fixture.Create<string>();

            var category = _categoriesData.Single(x => x.Id == categoryId);
            _categoriesRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(category);
            var item = category.CategoryItems.First();
            var updateItemModel = new UpdateItemModel { CategoryId = category.Id, ItemId = item.Id, ItemName = itemName };

            var resultDto = await _itemService.UpdateCategoryItemAsync(updateItemModel);

            resultDto.Should().NotBeNull();
            resultDto?.CategoryId.Should().Be(category.Id);
            resultDto?.ItemId.Should().Be(item.Id);
            resultDto?.CategoryName.Should().Be(category.Name);
            resultDto?.ItemName.Should().Be(itemName);

            _categoriesRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
            _categoriesRepoMock.Verify(x => x.SaveAsync(), Times.Once);
        }


        [Test]
        public async Task DeleteCategoryItemAsync_OnNonExistingCategory_ShouldReturnNull()
        {
            Category category = null;
            var random = new Random();
            var categoryId = random.Next(21, 100);

            _categoriesRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(category);
            
            var resultDto = await _itemService.DeleteCategoryItemAsync(categoryId, _fixture.Create<int>());

            resultDto.Should().NotBeNull();
            resultDto.CategoryId.Should().BeNull();

            _categoriesRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
            _categoriesRepoMock.Verify(x => x.SaveAsync(), Times.Never);
        }

        [Test]
        public async Task DeleteCategoryItemAsync_OnNonExistingItem_ShouldReturnNull()
        {
            var random = new Random();
            var categoryId = random.Next(1, 20);

            var category = _categoriesData.Single(x => x.Id == categoryId);
            _categoriesRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(category);
            var itemId = category.CategoryItems.Select(x => x.Id).OrderByDescending(x => x).First() + _fixture.Create<int>();
            

            var result = await _itemService.DeleteCategoryItemAsync(categoryId, itemId);

            result.Should().NotBeNull().And.BeAssignableTo<DeleteCategoryItemDto>();
            result?.CategoryId.Should().NotBeNull();
            result?.ItemId.Should().BeNull();
            _categoriesRepoMock.Verify(x => x.GetByIdAsync(categoryId), Times.Once);
            _categoriesRepoMock.Verify(x => x.SaveAsync(), Times.Never);
        }

        [Test]
        public async Task DeleteCategoryItemAsync_OnExistingCategoryAndItem_ShouldReturnCorrect()
        {
            var random = new Random();
            var categoryId = random.Next(1, 20);
            var itemName = _fixture.Create<string>();

            var category = _categoriesData.Single(x => x.Id == categoryId);
            _categoriesRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(category);
            var item = category.CategoryItems.First();
            
            var resultDto = await _itemService.DeleteCategoryItemAsync(categoryId, item.Id);

            resultDto?.Should().NotBeNull().And.BeAssignableTo<DeleteCategoryItemDto>();
            resultDto?.CategoryId.Should().Be(category.Id);
            resultDto?.ItemId.Should().Be(item.Id);
            
            _categoriesRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
            _categoriesRepoMock.Verify(x => x.SaveAsync(), Times.Once);
        }
    }
}
