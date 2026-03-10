namespace _Main.Scripts.Items
{
    public class ItemData
    {
        public int id;
        public string itemName;
        public ItemType itemType;
    }
    
    public enum ItemType
    {
        InventoryItem,
    }
}