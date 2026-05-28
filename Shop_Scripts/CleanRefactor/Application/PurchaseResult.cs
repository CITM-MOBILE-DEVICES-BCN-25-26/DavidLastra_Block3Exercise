using CleanRefactor.Domain;

namespace CleanRefactor.Application
{
    /// <summary>
    /// DTO returned by the PurchaseItemUseCase.
    ///
    /// It carries a SEMANTIC status plus the data the presentation layer needs
    /// to refresh the screen — but no final UI text. This decouples the
    /// presentation layer from the domain model: the View never receives a
    /// PlayerState or an IShopItem, only this plain result.
    /// </summary>
    public sealed class PurchaseResult
    {
        public ShopItemType Item { get; }
        public PurchaseStatus Status { get; }

        /// <summary>True only when Status == Purchased.</summary>
        public bool Success => Status == PurchaseStatus.Purchased;

        /// <summary>Player coins AFTER the operation (unchanged if it failed).</summary>
        public int CoinsAfterPurchase { get; }

        public PurchaseResult(ShopItemType item, PurchaseStatus status, int coinsAfterPurchase)
        {
            Item = item;
            Status = status;
            CoinsAfterPurchase = coinsAfterPurchase;
        }
    }
}
