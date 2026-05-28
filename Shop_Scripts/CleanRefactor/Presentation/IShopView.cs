using CleanRefactor.Application;

namespace CleanRefactor.Presentation
{
    /// <summary>
    /// The View contract of the MVP pattern.
    ///
    /// The View is PASSIVE: it only renders what the presenter tells it and
    /// raises user-action events. It contains NO business rules and NO use-case
    /// calls. Implemented by the Unity MonoBehaviour (ShopView).
    ///
    /// Because the presenter depends on this interface and not on a concrete
    /// MonoBehaviour, the presenter can be unit-tested with a fake view.
    /// </summary>
    public interface IShopView
    {
        /// <summary>Renders coins and the enabled/disabled state of each button.</summary>
        void RenderStatus(ShopStatusDto status);

        /// <summary>Displays a feedback message produced by the presenter.</summary>
        void ShowFeedback(string message);
    }
}
