namespace ThinkBridge.Inventory.ApplicationContract
{
    public class GetItemByIdResponse: InventoryServiceResponse
    {
        public ItemDto Item { get; set; }
    }
}
