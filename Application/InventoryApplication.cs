using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using ThinkBridge.Inventory.ApplicationContract;
using ThinkBridge.Inventory.DomainModel;
using ThinkBridge.Inventory.PersistenceContract;

namespace ThinkBridge.Inventory.Application
{
    public class InventoryApplication: IInventoryApplication
    {
        private readonly IInventoryRepository _inventoryRepository;

        public InventoryApplication(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }
        public async Task<GetItemsResponse> GetAllItems()
        {
            try
            {
                var items = await _inventoryRepository.GetAllItems();
                var itemsDto = new List<ItemDto>();
                foreach (var item in items)
                {
                    itemsDto.Add(new ItemDto()
                    {
                        Id = item.Id.ToString(),
                        Name = item.Name,
                        Price = item.Price.ToString(CultureInfo.InvariantCulture),
                        Description = item.Description
                    });
                }

                return new GetItemsResponse()
                {
                    Items = itemsDto
                };
            }
            catch (Exception ex)
            {
                return new GetItemsResponse()
                {
                    ErrorMessage = ex.Message
                };
            }
            
        }

        public async Task<GetItemByIdResponse> GetItemById(string itemId)
        {
            try
            {
                if (string.IsNullOrEmpty(itemId) || !int.TryParse(itemId, out _))
                {
                    throw new Exception("Item id is not valid !");
                }
                var item = await _inventoryRepository.GetItemById(itemId);

                if (item == null)
                {
                    return new GetItemByIdResponse()
                    {
                        ErrorMessage = "The item with specified id could not be found!"
                    };
                }
                return new GetItemByIdResponse()
                {
                    Item = new ItemDto()
                    {
                        Id = item.Id.ToString(),
                        Name = item.Name,
                        Price = item.Price.ToString(CultureInfo.InvariantCulture),
                        Description = item.Description
                    }
                };
            }
            catch (Exception ex)
            {
                return new GetItemByIdResponse()
                {
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<AddItemResponse> AddItem(AddItemRequest request)
        {
            try
            {
                Validate(request.Item);
                var itemToBeAdded = new Item()
                {
                    Name = request.Item.Name,
                    Description = request.Item.Description,
                    Price = Convert.ToDecimal(request.Item.Price)
                };
                var addedItemId = await _inventoryRepository.AddItem(itemToBeAdded);
                return new AddItemResponse()
                {
                    AddedItemId = addedItemId
                };
            }
            catch (Exception ex)
            {
                return new AddItemResponse()
                {
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<UpdateItemResponse> UpdateItem(UpdateItemRequest request)
        {
            try
            {
                Validate(request.Item);
                var itemToBeUpdated = new Item()
                {
                    Id = Convert.ToInt32(request.Item.Id),
                    Name = request.Item.Name,
                    Description = request.Item.Description,
                    Price = Convert.ToDecimal(request.Item.Price)
                };
                var result = await _inventoryRepository.UpdateItem(itemToBeUpdated);
                return result ? new UpdateItemResponse() : 
                    new UpdateItemResponse()
                    {
                        ErrorMessage = "The item with the specified id could not be updated!"
                    };
            }
            catch (Exception ex)
            {
                return new UpdateItemResponse()
                {
                    ErrorMessage = ex.Message
                };
            }
        }

        private static void Validate(ItemDto item)
        {
            if (item == null ||
                string.IsNullOrEmpty(item.Name) ||
                string.IsNullOrEmpty(item.Price) ||
                string.IsNullOrEmpty(item.Description))
            {
                throw new Exception("Missing params in AddItemRequest !");
            }

            if (!decimal.TryParse(item.Price, out _))
            {
                throw new Exception("Item price is not valid !");
            }
        }

        public async Task<RemoveItemResponse> RemoveItem(string itemId)
        {
            try
            {
                if (string.IsNullOrEmpty(itemId) || !int.TryParse(itemId, out _))
                {
                    throw new Exception("Item id is not valid !");
                }

                var result = await _inventoryRepository.RemoveItem(itemId);
                return result ? new RemoveItemResponse():
                new RemoveItemResponse()
                {
                    ErrorMessage = "The item with the specified id could not be removed!"
                };
            }
            catch (Exception ex)
            {
                return new RemoveItemResponse()
                {
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}
