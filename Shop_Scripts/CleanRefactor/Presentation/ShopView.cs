using CleanRefactor.Application;
using CleanRefactor.Domain;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CleanRefactor.Presentation
{
    public sealed class ShopView : MonoBehaviour, IShopView
    {
        [Header("UI - Texts")]
        [SerializeField] private Text coinsText;
        [SerializeField] private Text feedbackText;

        [Header("UI - Buttons")]
        [SerializeField] private Button bombButton;
        [SerializeField] private Button shieldButton;
        [SerializeField] private Button doubleCoinsButton;

        [Header("Dim colors (buyable / not buyable)")]
        [SerializeField] private Color buyableColor = Color.white;
        [SerializeField] private Color notBuyableColor = new Color(0.6f, 0.6f, 0.6f, 1f);

        private ShopPresenter _presenter;

        public void Bind(ShopPresenter presenter)
        {
            _presenter = presenter;

            bombButton.onClick.AddListener(_presenter.OnBuyBombRequested);
            shieldButton.onClick.AddListener(_presenter.OnBuyShieldRequested);
            doubleCoinsButton.onClick.AddListener(_presenter.OnBuyDoubleCoinsRequested);

            AddHover(bombButton,        _presenter.OnBombHovered);
            AddHover(shieldButton,      _presenter.OnShieldHovered);
            AddHover(doubleCoinsButton, _presenter.OnDoubleCoinsHovered);

            _presenter.Initialize();
        }

        private void AddHover(Button button, System.Action onEnter)
        {
            EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
            if (trigger == null)
                trigger = button.gameObject.AddComponent<EventTrigger>();

            var enter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
            enter.callback.AddListener(_ => onEnter());
            trigger.triggers.Add(enter);

            var exit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
            exit.callback.AddListener(_ => _presenter.OnHoverExit());
            trigger.triggers.Add(exit);
        }

        public void RenderStatus(ShopStatusDto status)
        {
            coinsText.text = "Coins: " + status.Coins;

            Dim(bombButton,        status.CanBuyBomb);
            Dim(shieldButton,      status.CanBuyShield);
            Dim(doubleCoinsButton, status.CanBuyDoubleCoins);
        }

        private void Dim(Button button, bool buyable)
        {
            var image = button.GetComponent<Image>();
            if (image != null)
                image.color = buyable ? buyableColor : notBuyableColor;
        }

        public void ShowFeedback(string message)
        {
            feedbackText.text = message;
        }

        private void OnDestroy()
        {
            if (_presenter == null) return;
            bombButton.onClick.RemoveListener(_presenter.OnBuyBombRequested);
            shieldButton.onClick.RemoveListener(_presenter.OnBuyShieldRequested);
            doubleCoinsButton.onClick.RemoveListener(_presenter.OnBuyDoubleCoinsRequested);
        }
    }
}
