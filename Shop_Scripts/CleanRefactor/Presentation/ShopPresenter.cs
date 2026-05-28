using CleanRefactor.Application;
using CleanRefactor.Domain;

namespace CleanRefactor.Presentation
{
    public sealed class ShopPresenter
    {
        private readonly IShopView _view;
        private readonly PurchaseItemUseCase _purchaseItem;
        private readonly GetShopStatusUseCase _getShopStatus;
        private readonly PurchaseFeedbackPresenter _feedback;
        private readonly IPurchaseAudio _audio;

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

        public void Initialize()
        {
            RefreshStatus();
            _view.ShowFeedback("Select an item to buy.");
        }

        public void OnBuyBombRequested()        => HandlePurchase(ShopItemType.Bomb);
        public void OnBuyShieldRequested()      => HandlePurchase(ShopItemType.Shield);
        public void OnBuyDoubleCoinsRequested() => HandlePurchase(ShopItemType.DoubleCoins);
        public void OnBombHovered()        => HandleHover(ShopItemType.Bomb);
        public void OnShieldHovered()      => HandleHover(ShopItemType.Shield);
        public void OnDoubleCoinsHovered() => HandleHover(ShopItemType.DoubleCoins);
        public void OnHoverExit()
        {
            _view.ShowFeedback("Select an item to buy.");
        }

        private void HandlePurchase(ShopItemType item)
        {
            PurchaseResult result = _purchaseItem.Execute(item);

            _view.ShowFeedback(_feedback.BuildMessage(result.Item, result.Status));

            if (result.Success)
            {
                _audio.PlayPurchaseSound();
            }
                
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
