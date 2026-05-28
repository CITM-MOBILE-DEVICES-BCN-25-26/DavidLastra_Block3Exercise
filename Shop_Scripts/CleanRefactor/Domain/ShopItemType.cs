namespace CleanRefactor.Domain
{
    /// <summary>
    /// Identifies the items the shop can sell. Using an enum (instead of magic
    /// strings like "BombUses") keeps the code type-safe and refactor-friendly.
    /// </summary>
    public enum ShopItemType
    {
        Bomb,
        Shield,
        DoubleCoins
    }
}
