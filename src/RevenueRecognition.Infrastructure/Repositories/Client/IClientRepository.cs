namespace RevenueRecognition.Infrastructure.Repositories.Client;

using Models.Client;

public interface IClientRepository
{
    public Task<bool> ExistsByEmailAsync(
        string email,
        CancellationToken cancellationToken
    );

    public Task<bool> ExistsByPhoneNumberAsync(
        string phoneNumber,
        CancellationToken cancellationToken
    );

    public Task<bool> ExistsIndividualByPeselAsync(
        string pesel,
        CancellationToken cancellationToken
    );

    public Task<bool> ExistsIndividualByClientIdAsync(
        int clientId,
        CancellationToken cancellationToken
    );

    public Task<bool> ExistsCompanyByKrsAsync(
        string krs,
        CancellationToken cancellationToken
    );

    public Task<bool> ExistsCompanyByClientIdAsync(
        int clientId,
        CancellationToken cancellationToken
    );

    public Task<List<Client>> GetClientsAsync(
        CancellationToken cancellationToken
    );

    public Task<Client?> GetClientByIdAsync(
        int id,
        CancellationToken cancellationToken
    );

    public Task<IndividualClient?> GetIndividualClientByClientIdAsync(
        int id,
        CancellationToken cancellationToken
    );

    public Task<CompanyClient?> GetCompanyClientByClientIdAsync(
        int id,
        CancellationToken cancellationToken
    );

    public Task<Client> InsertClientAsync(Client client,
        CancellationToken cancellationToken);

    public Task<IndividualClient> InsertIndividualClientAsync(IndividualClient client,
        CancellationToken cancellationToken);

    public Task<CompanyClient> InsertCompanyClientAsync(CompanyClient client,
        CancellationToken cancellationToken);

    public Task<bool> UpdateClientAsync(
        int id,
        Client client,
        CancellationToken cancellationToken
    );

    public Task<bool> UpdateIndividualClientAsync(
        int id,
        IndividualClient client,
        CancellationToken cancellationToken
    );

    public Task<bool> UpdateCompanyClientAsync(
        int id,
        CompanyClient client,
        CancellationToken cancellationToken
    );

    public Task<bool> SetIsLoyalAsync(Client client,
        bool isLoyal,
        CancellationToken cancellationToken);
    public Task<bool> IsDeletedByClientId(
        int id,
        CancellationToken cancellationToken
    );
    public Task<bool> SoftDeleteByClientId(
        int id,
        CancellationToken cancellationToken
    );
}