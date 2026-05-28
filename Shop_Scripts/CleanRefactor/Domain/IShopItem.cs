namespace CleanRefactor.Domain
{
    public interface IShopItem
    {
        ShopItemType Type { get; }
        int Cost { get; }
        PurchaseStatus CanPurchase(PlayerState player);
        void ApplyPurchase(PlayerState player);
    }
}
