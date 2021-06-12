using System.Threading.Tasks;

namespace ThinkBridge.Inventory.ApplicationContract
{
    public interface IInventoryApplication
    {
        Task<GetItemsResponse> GetAllItems();

        Task<GetItemByIdResponse> GetItemById(string itemId);

        Task<AddItemResponse> AddItem(AddItemRequest request);

        Task<UpdateItemResponse> UpdateItem(UpdateItemRequest request);

        Task<RemoveItemResponse> RemoveItem(string itemId);
    }
}
