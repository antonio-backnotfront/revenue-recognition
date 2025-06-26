namespace RevenueRecognition.Infrastructure.Repositories.UnitOfWork;

public interface IUnitOfWork
{
    public Task StartTransactionAsync(CancellationToken cancellationToken);
    public Task CommitTransactionAsync(CancellationToken cancellationToken);
    public Task RollbackTransactionAsync(CancellationToken cancellationToken);
}