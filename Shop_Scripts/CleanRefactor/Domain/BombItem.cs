namespace CleanRefactor.Domain
{
    /// <summary>
    /// Bomb item rules:
    ///  - Cost: 100 coins (from config)
    ///  - Allowed only if the player has enough coins and fewer than MaxUses bombs.
    ///  - On success: bomb uses increase by 1.
    /// </summary>
    public sealed class BombItem : IShopItem
    {
        private readonly ShopItemConfig _config;

        public BombItem(ShopItemConfig config)
        {
            _config = config;
        }

        public ShopItemType Type => ShopItemType.Bomb;
        public int Cost => _config.Cost;

        public PurchaseStatus CanPurchase(PlayerState player)
        {
            if (player.BombUses >= _config.MaxUses)
                return PurchaseStatus.MaxUsesReached;

            if (player.Coins < _config.Cost)
                return PurchaseStatus.NotEnoughCoins;

            return PurchaseStatus.Purchased;
        }

        public void ApplyPurchase(PlayerState player)
        {
            player.AddBombUse();
        }
    }
}
