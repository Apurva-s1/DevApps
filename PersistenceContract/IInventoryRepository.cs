using System.Collections.Generic;
using System.Threading.Tasks;
using ThinkBridge.Inventory.DomainModel;

namespace ThinkBridge.Inventory.PersistenceContract
{
    public interface IInventoryRepository
    {
        Task<List<Item>> GetAllItems();

        Task<Item> GetItemById(string itemId);

        Task<int> AddItem(Item request);

        Task<bool> UpdateItem(Item request);

        Task<bool> RemoveItem(string itemId);
    }
}
