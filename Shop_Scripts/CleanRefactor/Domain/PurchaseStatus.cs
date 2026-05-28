namespace CleanRefactor.Domain
{
    /// <summary>
    /// Semantic result of a purchase attempt.
    ///
    /// The domain/application layers return THIS, never a final UI string like
    /// "Not enough coins for Bomb". Translating a status into human text is a
    /// presentation responsibility (see PurchaseFeedbackPresenter).
    /// </summary>
    public enum PurchaseStatus
    {
        Purchased,
        NotEnoughCoins,
        MaxUsesReached,
        RequiredLevelNotReached,
        AlreadyOwned
    }
}
