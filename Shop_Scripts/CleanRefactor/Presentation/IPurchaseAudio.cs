namespace CleanRefactor.Presentation
{
    /// <summary>
    /// Audio abstraction for the presentation layer.
    ///
    /// Requirement #8: "Audio playback must not be part of the domain or use
    /// cases." So audio is a PRESENTATION concern. The presenter depends on this
    /// interface; the concrete AudioSource implementation lives in the View
    /// layer. This also keeps the presenter testable (a silent fake can be
    /// injected).
    /// </summary>
    public interface IPurchaseAudio
    {
        void PlayPurchaseSound();
    }
}
