namespace CleanRefactor.Domain
{
    /// <summary>
    /// Shield item rules:
    ///  - Cost: 150 coins (from config)
    ///  - Allowed only if the player has enough coins and fewer than MaxUses shields.
    ///  - On success: shield uses increase by 1.
    /// </summary>
    public sealed class ShieldItem : IShopItem
    {
        private readonly ShopItemConfig _config;

        public ShieldItem(ShopItemConfig config)
        {
            _config = config;
        }

        public ShopItemType Type => ShopItemType.Shield;
        public int Cost => _config.Cost;

        public PurchaseStatus CanPurchase(PlayerState player)
        {
            if (player.ShieldUses >= _config.MaxUses)
                return PurchaseStatus.MaxUsesReached;

            if (player.Coins < _config.Cost)
                return PurchaseStatus.NotEnoughCoins;

            return PurchaseStatus.Purchased;
        }

        public void ApplyPurchase(PlayerState player)
        {
            player.AddShieldUse();
        }
    }
}
