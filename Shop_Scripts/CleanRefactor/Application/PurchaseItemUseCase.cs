using CleanRefactor.Domain;

namespace CleanRefactor.Application
{
    /// <summary>
    /// Application use case: orchestrates the flow of buying an item.
    ///
    /// It does NOT contain the per-item rules (those live in each IShopItem) and
    /// it does NOT know about Unity, UI or audio. Its single responsibility is
    /// the ORCHESTRATION:
    ///   1. load the player
    ///   2. ask the item if the purchase is valid
    ///   3. if valid: deduct coins, apply the item effect, persist
    ///   4. return a semantic PurchaseResult DTO
    ///
    /// Persistence rule (requirement / test case): the player is saved ONLY when
    /// the purchase succeeds. On failure nothing is mutated and nothing is saved.
    /// </summary>
    public sealed class PurchaseItemUseCase
    {
        private readonly ShopCatalog _catalog;
        private readonly IPlayerRepository _repository;

        public PurchaseItemUseCase(ShopCatalog catalog, IPlayerRepository repository)
        {
            _catalog = catalog;
            _repository = repository;
        }

        public PurchaseResult Execute(ShopItemType itemType)
        {
            PlayerState player = _repository.Load();
            IShopItem item = _catalog.Get(itemType);

            PurchaseStatus status = item.CanPurchase(player);
            if (status != PurchaseStatus.Purchased)
            {
                // Failure: no mutation, no save. Coins reported unchanged.
                return new PurchaseResult(itemType, status, player.Coins);
            }

            // Success: the coin-deduction rule is applied here ONCE, instead of
            // being duplicated inside every item (as in the original code).
            player.SpendCoins(item.Cost);
            item.ApplyPurchase(player);

            _repository.Save(player);

            return new PurchaseResult(itemType, PurchaseStatus.Purchased, player.Coins);
        }
    }
}
