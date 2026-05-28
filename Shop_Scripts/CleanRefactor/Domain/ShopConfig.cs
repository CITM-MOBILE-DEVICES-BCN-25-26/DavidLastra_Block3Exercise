namespace CleanRefactor.Domain
{
    public sealed class ShopItemConfig
    {
        public int Cost { get; }
        public int MaxUses { get; }
        public int RequiredLevel { get; }

        public ShopItemConfig(int cost, int maxUses, int requiredLevel = 0)
        {
            Cost = cost;
            MaxUses = maxUses;
            RequiredLevel = requiredLevel;
        }
    }

    public sealed class ShopConfig
    {
        public ShopItemConfig Bomb { get; }
        public ShopItemConfig Shield { get; }
        public ShopItemConfig DoubleCoins { get; }

        public ShopConfig(ShopItemConfig bomb, ShopItemConfig shield, ShopItemConfig doubleCoins)
        {
            Bomb = bomb;
            Shield = shield;
            DoubleCoins = doubleCoins;
        }
        public static ShopConfig Default()
        {
            return new ShopConfig(
                bomb:        new ShopItemConfig(cost: 100, maxUses: 3),
                shield:      new ShopItemConfig(cost: 150, maxUses: 2),
                doubleCoins: new ShopItemConfig(cost: 300, maxUses: 1, requiredLevel: 5));
        }
    }
}
