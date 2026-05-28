namespace CleanRefactor.Domain
{
    /// <summary>
    /// Double Coins item rules:
    ///  - Cost: 300 coins (from config)
    ///  - Requires player level 5 or higher.
    ///  - Can only be purchased once.
    ///  - On success: HasDoubleCoins becomes true.
    /// </summary>
    public sealed class DoubleCoinsItem : IShopItem
    {
        private readonly ShopItemConfig _config;

        public DoubleCoinsItem(ShopItemConfig config)
        {
            _config = config;
        }

        public ShopItemType Type => ShopItemType.DoubleCoins;
        public int Cost => _config.Cost;

        public PurchaseStatus CanPurchase(PlayerState player)
        {
            // Ownership is checked first: an already-owned upgrade can never be
            // bought again regardless of coins or level.
            if (player.HasDoubleCoins)
                return PurchaseStatus.AlreadyOwned;

            if (player.PlayerLevel < _config.RequiredLevel)
                return PurchaseStatus.RequiredLevelNotReached;

            if (player.Coins < _config.Cost)
                return PurchaseStatus.NotEnoughCoins;

            return PurchaseStatus.Purchased;
        }

        public void ApplyPurchase(PlayerState player)
        {
            player.GrantDoubleCoins();
        }
    }
}
