using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThinkBridge.Inventory.Application;
using ThinkBridge.Inventory.ApplicationContract;
using ThinkBridge.Inventory.DomainModel;
using ThinkBridge.Inventory.PersistenceContract;

namespace ThinkBridge.Inventory.UnitTest
{
    [TestClass, ExcludeFromCodeCoverage]
    public class ThinkBridgeApplicationUnitTest
    {
        private InventoryApplication _inventoryApplication;
        private Mock<IInventoryRepository> _mockInventoryRepositoryFactory;

        [TestInitialize]
        public void Initialize()
        {
            _mockInventoryRepositoryFactory = new Mock<IInventoryRepository>();
            _inventoryApplication =
                new InventoryApplication(_mockInventoryRepositoryFactory.Object);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public async Task GetAllItems_ItemsReturned()
        {
            // Arrange
            var mockItems = new List<Item>()
            {
                new Item()
                {
                    Id = 100,
                    Name = "Item1",
                    Price = new decimal(456.78),
                    Description = "Item1 description"
                }
            };
            _mockInventoryRepositoryFactory.Setup(x => x.GetAllItems())
                .ReturnsAsync(mockItems);
            // Act
            var itemList = await _inventoryApplication.GetAllItems()
                .ConfigureAwait(false);

            // Assert
            itemList.Should().NotBeNull();
            itemList.Items.Count.Should().BeGreaterThan(0);

        }

        [TestMethod]
        [TestCategory("Unit")]
        public async Task GetAllItems_ZeroItemsReturned()
        {
            // Arrange
            var items = new List<Item>();
            _mockInventoryRepositoryFactory.Setup(x => x.GetAllItems())
                .ReturnsAsync(items);
            // Act
            var itemList = await _inventoryApplication.GetAllItems()
                .ConfigureAwait(false);

            // Assert
            itemList.Should().NotBeNull();
            itemList.Items.Count.Should().Be(0);

        }

        [TestMethod]
        [TestCategory("Unit")]
        public async Task GetItemById_ItemReturned()
        {
            // Arrange
            var item = new Item()
            {
                Id = 100,
                Name = "Item1",
                Price = new decimal(456.78),
                Description = "Item1 description"
            };
            _mockInventoryRepositoryFactory.Setup(x => x.GetItemById(It.IsAny<string>()))
                .ReturnsAsync(item);
            // Act
            var getItemResult = await _inventoryApplication.GetItemById("100")
                .ConfigureAwait(false);

            // Assert
            getItemResult.Should().NotBeNull();
            getItemResult.Item.Should().NotBeNull();
            getItemResult.Item.Name.Should().NotBeNull();
            getItemResult.Item.Price.Should().NotBeNull();
            getItemResult.Item.Description.Should().NotBeNull();

        }

        [TestMethod]
        [TestCategory("Unit")]
        public async Task GetItemById_ItemNotFound()
        {
            // Arrange
            _mockInventoryRepositoryFactory.Setup(x => x.GetItemById(It.IsAny<string>()))
                .ReturnsAsync((Item) null);
            // Act
            var getItemResult = await _inventoryApplication.GetItemById("100")
                .ConfigureAwait(false);

            // Assert
            getItemResult.Should().NotBeNull();
            getItemResult.ErrorMessage.Should().NotBeEmpty();
            getItemResult.Item.Should().BeNull();

        }

        [TestMethod]
        [TestCategory("Unit")]
        public async Task RemoveItemById_ItemRemoved()
        {
            // Arrange
            _mockInventoryRepositoryFactory.Setup(x => x.RemoveItem(It.IsAny<string>())).ReturnsAsync(true);
            // Act
            var removeItemResult = await _inventoryApplication.RemoveItem("100")
                .ConfigureAwait(false);

            // Assert
            removeItemResult.Should().NotBeNull();

        }

        [TestMethod]
        [TestCategory("Unit")]
        public async Task RemoveItemById_ItemNotRemoved()
        {
            // Arrange
            _mockInventoryRepositoryFactory.Setup(x => x.RemoveItem(It.IsAny<string>())).ReturnsAsync(false);
            // Act
            var removeItemResult = await _inventoryApplication.RemoveItem("100")
                .ConfigureAwait(false);

            // Assert
            removeItemResult.Should().NotBeNull();
            removeItemResult.ErrorMessage.Should().NotBeEmpty();

        }

        [TestMethod]
        [TestCategory("Unit")]
        public async Task AddItem_ItemAdded()
        {
            // Arrange
            
            var addItemRequest = new AddItemRequest()
            {
                Item = new ItemDto()
                {
                    Name = "Item1",
                    Price = "456.78",
                    Description = "Item1 description"
                }
            };
            _mockInventoryRepositoryFactory.Setup(x => x.AddItem(It.IsAny<Item>()))
                .ReturnsAsync(100);
            // Act
            var addItemResult = await _inventoryApplication.AddItem(addItemRequest)
                .ConfigureAwait(false);

            // Assert
            addItemResult.Should().NotBeNull();
            addItemResult.AddedItemId.Should().BeGreaterThan(0);

        }

        [TestMethod]
        [TestCategory("Unit")]
        public async Task AddItem_ItemNotAdded()
        {
            // Arrange
            var item = new Item()
            {
                Name = "Item1"
            };
            _mockInventoryRepositoryFactory.Setup(x => x.AddItem(It.IsAny<Item>()))
                .ReturnsAsync(item.Id);
            // Act
            var addItemResult = await _inventoryApplication.AddItem(It.IsAny<AddItemRequest>())
                .ConfigureAwait(false);

            // Assert
            addItemResult.Should().NotBeNull();
            addItemResult.ErrorMessage.Should().NotBeEmpty();

        }

        [TestMethod]
        [TestCategory("Unit")]
        public async Task UpdateItem_ItemUpdated()
        {
            // Arrange
            var updateItemRequest = new UpdateItemRequest()
            {
                Item = new ItemDto()
                {
                    Id = "100",
                    Name = "Item1",
                    Price = "456.78",
                    Description = "Item1 description"
                }
            };
            _mockInventoryRepositoryFactory.Setup(x => x.UpdateItem(It.IsAny<Item>())).ReturnsAsync(true);
            // Act
            var removeItemResult = await _inventoryApplication.UpdateItem(updateItemRequest)
                .ConfigureAwait(false);

            // Assert
            removeItemResult.Should().NotBeNull();
            removeItemResult.ErrorMessage.Should().BeNull();

        }

        [TestMethod]
        [TestCategory("Unit")]
        public async Task UpdateItem_ItemNotUpdated()
        {
            // Arrange
            var updateItemRequest = new UpdateItemRequest()
            {
                Item = new ItemDto()
                {
                    Id = "100",
                    Name = "Item1 updated"
                }
            };
            _mockInventoryRepositoryFactory.Setup(x => x.UpdateItem(It.IsAny<Item>()))
                .ReturnsAsync(false);
            // Act
            var updateItemResponse = await _inventoryApplication.UpdateItem(updateItemRequest)
                .ConfigureAwait(false);

            // Assert
            updateItemResponse.Should().NotBeNull();
            updateItemResponse.ErrorMessage.Should().NotBeEmpty();

        }
    }
}
