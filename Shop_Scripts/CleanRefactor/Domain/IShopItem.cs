namespace CleanRefactor.Domain
{
    /// <summary>
    /// Abstraction of a single sellable shop item and ITS OWN business rules.
    ///
    /// SOLID notes:
    ///  - Open/Closed: adding a new item (e.g. "ExtraLife") means writing a NEW
    ///    class that implements this interface. Existing items and the use case
    ///    are NOT modified.
    ///  - Single Responsibility: each implementation knows only how to validate
    ///    and apply ONE item. The original ShopManager mixed all three.
    ///  - Liskov: every implementation is interchangeable behind this contract.
    /// </summary>
    public interface IShopItem
    {
        ShopItemType Type { get; }

        /// <summary>The cost of this item, taken from configuration.</summary>
        int Cost { get; }

        /// <summary>
        /// Validates whether the given player can currently buy this item.
        /// Returns PurchaseStatus.Purchased if allowed, or the failing reason.
        /// This method MUST NOT mutate the player state.
        /// </summary>
        PurchaseStatus CanPurchase(PlayerState player);

        /// <summary>
        /// Applies the effects of a successful purchase to the player state
        /// (e.g. increment uses, grant ownership). Coin deduction is handled by
        /// the use case so the rule is not duplicated across items.
        /// </summary>
        void ApplyPurchase(PlayerState player);
    }
}
