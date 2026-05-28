using CleanRefactor.Application;
using CleanRefactor.Domain;

namespace CleanRefactor.Presentation
{
    /// <summary>
    /// The PRESENTER of the MVP pattern.
    ///
    /// It is plain C# — NO MonoBehaviour, NO Unity types — so it can be unit
    /// tested. It is the middle layer that:
    ///   - receives user actions forwarded by the View (OnBuy...Requested)
    ///   - calls the application use cases
    ///   - translates results into View updates (status + feedback text)
    ///   - triggers audio feedback (a presentation concern)
    ///
    /// MVP roles:
    ///   Model  -> use cases + domain (PurchaseItemUseCase, GetShopStatusUseCase)
    ///   View   -> IShopView (the passive Unity MonoBehaviour)
    ///   Presenter -> this class (no UI framework, no business rules)
    /// </summary>
    public sealed class ShopPresenter
    {
        private readonly IShopView _view;
        private readonly PurchaseItemUseCase _purchaseItem;
        private readonly GetShopStatusUseCase _getShopStatus;
        private readonly PurchaseFeedbackPresenter _feedback;
        private readonly IPurchaseAudio _audio;

        // Cached so hover handlers can explain WHY an item is not buyable
        // without recomputing. Updated on every RefreshStatus().
        private ShopStatusDto _lastStatus;

        public ShopPresenter(
            IShopView view,
            PurchaseItemUseCase purchaseItem,
            GetShopStatusUseCase getShopStatus,
            PurchaseFeedbackPresenter feedback,
            IPurchaseAudio audio)
        {
            _view = view;
            _purchaseItem = purchaseItem;
            _getShopStatus = getShopStatus;
            _feedback = feedback;
            _audio = audio;
        }

        /// <summary>Called once when the shop screen opens.</summary>
        public void Initialize()
        {
            RefreshStatus();
            _view.ShowFeedback("Select an item to buy.");
        }

        // --- User actions forwarded by the View ---------------------------

        public void OnBuyBombRequested()        => HandlePurchase(ShopItemType.Bomb);
        public void OnBuyShieldRequested()      => HandlePurchase(ShopItemType.Shield);
        public void OnBuyDoubleCoinsRequested() => HandlePurchase(ShopItemType.DoubleCoins);

        // --- Hover actions forwarded by the View --------------------------
        // When the pointer enters a button, show why it can (or cannot) be bought.

        public void OnBombHovered()        => HandleHover(ShopItemType.Bomb);
        public void OnShieldHovered()      => HandleHover(ShopItemType.Shield);
        public void OnDoubleCoinsHovered() => HandleHover(ShopItemType.DoubleCoins);

        /// <summary>Called when the pointer leaves a button (resets the prompt).</summary>
        public void OnHoverExit()
        {
            _view.ShowFeedback("Select an item to buy.");
        }

        // --- Internal flow -------------------------------------------------

        private void HandlePurchase(ShopItemType item)
        {
            PurchaseResult result = _purchaseItem.Execute(item);

            _view.ShowFeedback(_feedback.BuildMessage(result.Item, result.Status));

            if (result.Success)
                _audio.PlayPurchaseSound();

            // Refresh button states / coins after every attempt.
            RefreshStatus();
        }

        private void HandleHover(ShopItemType item)
        {
            if (_lastStatus == null) return;

            PurchaseStatus status = StatusFor(item);
            _view.ShowFeedback(_feedback.BuildHoverMessage(item, status));
        }

        private PurchaseStatus StatusFor(ShopItemType item)
        {
            switch (item)
            {
                case ShopItemType.Bomb:        return _lastStatus.BombStatus;
                case ShopItemType.Shield:      return _lastStatus.ShieldStatus;
                case ShopItemType.DoubleCoins: return _lastStatus.DoubleCoinsStatus;
                default:                       return PurchaseStatus.Purchased;
            }
        }

        private void RefreshStatus()
        {
            _lastStatus = _getShopStatus.Execute();
            _view.RenderStatus(_lastStatus);
        }
    }
}
