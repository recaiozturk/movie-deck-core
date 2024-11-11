namespace MovieDeck.Repository.Shared
{
    public interface IUnitOfWork
    {
        Task<int> CommitAsync();
    }
}
