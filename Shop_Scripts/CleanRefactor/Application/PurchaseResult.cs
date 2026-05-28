using CleanRefactor.Domain;

namespace CleanRefactor.Application
{
    public sealed class PurchaseResult
    {
        public ShopItemType Item { get; }
        public PurchaseStatus Status { get; }
        public bool Success => Status == PurchaseStatus.Purchased;
        public int CoinsAfterPurchase { get; }

        public PurchaseResult(ShopItemType item, PurchaseStatus status, int coinsAfterPurchase)
        {
            Item = item;
            Status = status;
            CoinsAfterPurchase = coinsAfterPurchase;
        }
    }
}
