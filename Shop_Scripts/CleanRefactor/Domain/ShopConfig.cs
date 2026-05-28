namespace CleanRefactor.Domain
{
    /// <summary>
    /// Plain C# configuration object. Holds the cost and the constraints of a
    /// single shop item.
    ///
    /// IMPORTANT: this is NOT a ScriptableObject and uses no [SerializeField].
    /// The domain must not depend on Unity serialization. The Bootstrap
    /// (composition root) is the only place allowed to read serialized fields
    /// and build these plain objects from them.
    /// </summary>
    public sealed class ShopItemConfig
    {
        public int Cost { get; }

        /// <summary>Maximum amount of uses/ownership allowed (e.g. 3 for Bomb).</summary>
        public int MaxUses { get; }

        /// <summary>Minimum player level required to buy (0 if no requirement).</summary>
        public int RequiredLevel { get; }

        public ShopItemConfig(int cost, int maxUses, int requiredLevel = 0)
        {
            Cost = cost;
            MaxUses = maxUses;
            RequiredLevel = requiredLevel;
        }
    }

    /// <summary>
    /// Aggregates the configuration of every item the shop sells.
    /// Provides safe, sensible defaults that match the functional requirements.
    /// </summary>
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

        /// <summary>Default configuration as described in the functional requirements.</summary>
        public static ShopConfig Default()
        {
            return new ShopConfig(
                bomb:        new ShopItemConfig(cost: 100, maxUses: 3),
                shield:      new ShopItemConfig(cost: 150, maxUses: 2),
                doubleCoins: new ShopItemConfig(cost: 300, maxUses: 1, requiredLevel: 5));
        }
    }
}
