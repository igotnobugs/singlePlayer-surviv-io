[System.Serializable]
public class ItemStack 
{
    public Item itemStored;
    public int itemAmount;
    public int maxItemAmount;
    
    //Use addAmount to add
    public ItemStack(Item item, int maxAmount) {
        itemStored = item;
        maxItemAmount = maxAmount;
    }

    // Add to amount, return left over
    public int AddAmount(int count) {
        if (itemAmount >= maxItemAmount) return count;

        itemAmount += count;

        if (itemAmount >= maxItemAmount) {
            int leftOver = itemAmount - maxItemAmount;
            itemAmount = maxItemAmount;
            return leftOver;
        }

        return 0;
    }

    public int GetSpace() {
        return maxItemAmount - itemAmount;
    }
}
