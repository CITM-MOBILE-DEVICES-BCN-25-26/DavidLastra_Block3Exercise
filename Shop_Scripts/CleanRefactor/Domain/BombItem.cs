namespace CleanRefactor.Domain
{
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
            {
                return PurchaseStatus.MaxUsesReached;
            }

            if (player.Coins < _config.Cost)
            {
                return PurchaseStatus.NotEnoughCoins;
            }

            return PurchaseStatus.Purchased;
        }

        public void ApplyPurchase(PlayerState player)
        {
            player.AddBombUse();
        }
    }
}
