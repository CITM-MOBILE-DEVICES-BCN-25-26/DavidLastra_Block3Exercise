using CleanRefactor.Application;
using CleanRefactor.Domain;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CleanRefactor.Presentation
{
    /// <summary>
    /// The Unity VIEW of the MVP pattern. It is a MonoBehaviour, but a PASSIVE
    /// one: it has NO business rules, NO PlayerPrefs, NO use-case logic.
    ///
    /// Its only responsibilities are:
    ///   - hold the scene references (Text, Button)
    ///   - forward button clicks AND hover events to the presenter
    ///   - render whatever the presenter tells it to render (coins, button
    ///     dim state, feedback text)
    ///
    /// Note on hover + disabled buttons: a Unity Button with interactable=false
    /// does NOT receive pointer events, so we keep the buttons interactable and
    /// instead DIM them visually when they can't be bought. The actual purchase
    /// rule still lives in the domain, so clicking a dimmed button simply gets
    /// rejected by the use case and the reason is shown — nothing is bought.
    /// </summary>
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

        /// <summary>
        /// Called by the Bootstrap to inject the presenter and wire up clicks
        /// and hover events.
        /// </summary>
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

        // Registers PointerEnter (show reason) and PointerExit (reset) on a button.
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

        // --- IShopView -----------------------------------------------------

        public void RenderStatus(ShopStatusDto status)
        {
            coinsText.text = "Coins: " + status.Coins;

            // Buttons stay interactable so they keep receiving hover events;
            // we communicate "can't buy" by dimming the color instead.
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
            // Always remove listeners to avoid dangling references.
            if (_presenter == null) return;
            bombButton.onClick.RemoveListener(_presenter.OnBuyBombRequested);
            shieldButton.onClick.RemoveListener(_presenter.OnBuyShieldRequested);
            doubleCoinsButton.onClick.RemoveListener(_presenter.OnBuyDoubleCoinsRequested);
        }
    }
}
