namespace CleanRefactor.Domain
{
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
            if (player.HasDoubleCoins)
            {
                return PurchaseStatus.AlreadyOwned;
            }

            if (player.PlayerLevel < _config.RequiredLevel)
            {
                return PurchaseStatus.RequiredLevelNotReached;
            }

            if (player.Coins < _config.Cost)
            {
                return PurchaseStatus.NotEnoughCoins;
            }

            return PurchaseStatus.Purchased;
        }

        public void ApplyPurchase(PlayerState player)
        {
            player.GrantDoubleCoins();
        }
    }
}
