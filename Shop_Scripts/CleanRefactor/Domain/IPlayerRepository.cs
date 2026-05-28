namespace CleanRefactor.Domain
{
    /// <summary>
    /// Persistence abstraction (a "port" in hexagonal-architecture terms).
    ///
    /// SOLID - Dependency Inversion: the DOMAIN declares what it needs (load and
    /// save a PlayerState) but does NOT know HOW it is stored. The concrete
    /// PlayerPrefs implementation lives in the Infrastructure layer.
    ///
    /// Benefits:
    ///  - PlayerPrefs is no longer called directly from business code.
    ///  - Tests can inject an in-memory fake (see InMemoryPlayerRepository),
    ///    so the use cases are testable without Unity.
    ///  - The storage backend can be swapped (JSON file, cloud save, SQLite)
    ///    without touching a single line of domain or application code.
    /// </summary>
    public interface IPlayerRepository
    {
        PlayerState Load();
        void Save(PlayerState player);

        /// <summary>
        /// Clears any persisted state so the next Load() returns the defaults.
        /// Used by the Bootstrap's debug "reset to default" option. Kept on the
        /// abstraction so the domain/application code stays unaware of HOW the
        /// data is stored (PlayerPrefs, JSON, etc.).
        /// </summary>
        void Reset();
    }
}
