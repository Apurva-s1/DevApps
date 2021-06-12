using System.Collections.Generic;

namespace ThinkBridge.Inventory.ApplicationContract
{
    public class GetItemsResponse: InventoryServiceResponse
    {
        public List<ItemDto> Items { get; set; }
    }
}
