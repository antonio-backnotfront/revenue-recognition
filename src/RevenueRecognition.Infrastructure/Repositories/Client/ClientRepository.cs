using Microsoft.EntityFrameworkCore;
using RevenueRecognition.Infrastructure.DAL;

namespace RevenueRecognition.Infrastructure.Repositories.Client;

using Models.Client;

public class ClientRepository : IClientRepository
{
    private readonly CompanyDbContext _context;

    public ClientRepository(CompanyDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await _context.Clients
            .AnyAsync(
                client =>
                    !string.IsNullOrEmpty(client.Email)
                    &&
                    client.Email.Equals(email)
                , cancellationToken
            );
    }

    public async Task<bool> ExistsByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken)
    {
        return await _context.Clients
            .AnyAsync(
                client =>
                    !string.IsNullOrEmpty(client.PhoneNumber)
                    &&
                    client.PhoneNumber.Equals(phoneNumber),
                cancellationToken
            );
    }

    public async Task<bool> ExistsIndividualByPeselAsync(string pesel, CancellationToken cancellationToken)
    {
        return await _context.IndividualClients
            .AnyAsync(
                client =>
                    !string.IsNullOrEmpty(client.PESEL)
                    &&
                    client.PESEL.Equals(pesel),
                cancellationToken
            );
    }

    public async Task<bool> ExistsIndividualByClientIdAsync(int clientId, CancellationToken cancellationToken)
    {
        return await _context.IndividualClients
            .AnyAsync(
                client =>
                    client.ClientId == clientId,
                cancellationToken
            );
    }

    public async Task<bool> ExistsCompanyByKrsAsync(string krs, CancellationToken cancellationToken)
    {
        return await _context.CompanyClients
            .AnyAsync(
                client =>
                    client.KRS == krs,
                cancellationToken
            );
    }

    public async Task<bool> ExistsCompanyByClientIdAsync(int clientId, CancellationToken cancellationToken)
    {
        return await _context.CompanyClients
            .AnyAsync(
                client =>
                    client.ClientId == clientId,
                cancellationToken
            );
    }

    public async Task<List<Client>> GetClientsAsync(CancellationToken cancellationToken)
    {
        return await _context.Clients.ToListAsync(cancellationToken);
    }

    public async Task<Client?> GetClientByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Clients
            .Include(c => c.IndividualClient)
            .Include(c => c.CompanyClient)
            .FirstOrDefaultAsync(client => client.Id == id, cancellationToken);
    }

    public async Task<IndividualClient?> GetIndividualClientByClientIdAsync(
        int id,
        CancellationToken cancellationToken
    )
    {
        return await _context.IndividualClients
            .FirstOrDefaultAsync(client => client.ClientId == id, cancellationToken);
    }

    public async Task<CompanyClient?> GetCompanyClientByClientIdAsync(
        int id,
        CancellationToken cancellationToken
    )
    {
        return await _context.CompanyClients
            .FirstOrDefaultAsync(client => client.ClientId == id, cancellationToken);
    }

    public async Task<Client> InsertClientAsync(
        Client client,
        CancellationToken cancellationToken
    )
    {
        Client createdClient = (await _context.Clients.AddAsync(client, cancellationToken)).Entity;
        await _context.SaveChangesAsync(cancellationToken);
        return createdClient;
    }

    public async Task<IndividualClient> InsertIndividualClientAsync(
        IndividualClient client,
        CancellationToken cancellationToken
    )
    {
        IndividualClient createdClient = (await _context.IndividualClients.AddAsync(client, cancellationToken)).Entity;
        await _context.SaveChangesAsync(cancellationToken);
        return createdClient;
    }

    public async Task<CompanyClient> InsertCompanyClientAsync(
        CompanyClient client,
        CancellationToken cancellationToken
    )
    {
        CompanyClient createdClient = (await _context.CompanyClients.AddAsync(client, cancellationToken)).Entity;
        await _context.SaveChangesAsync(cancellationToken);
        return createdClient;
    }

    public async Task<bool> UpdateClientAsync(
        int id,
        Client client,
        CancellationToken cancellationToken
    )
    {
        Client? c = await GetClientByIdAsync(id, cancellationToken);
        if (c != null)
        {
            c.Address = client.Address;
            c.Email = client.Email;
            c.PhoneNumber = client.PhoneNumber;
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        return false;
    }

    public async Task<bool> UpdateIndividualClientAsync(
        int id,
        IndividualClient client,
        CancellationToken cancellationToken
    )
    {
        IndividualClient? c = await GetIndividualClientByClientIdAsync(id, cancellationToken);
        if (c != null)
        {
            c.PESEL = client.PESEL;
            c.FirstName = client.FirstName;
            c.LastName = client.LastName;
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        return false;
    }

    public async Task<bool> UpdateCompanyClientAsync(
        int id,
        CompanyClient client,
        CancellationToken cancellationToken
    )
    {
        CompanyClient? c = await GetCompanyClientByClientIdAsync(id, cancellationToken);
        if (c != null)
        {
            c.KRS = client.KRS;
            c.Name = client.Name;
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        return false;
    }

    public async Task<bool> SetIsLoyalAsync(Client client,
        bool isLoyal,
        CancellationToken cancellationToken)
    {
        client.IsLoyal = isLoyal;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> IsDeletedByClientId(
        int id,
        CancellationToken cancellationToken
    )
    {
        IndividualClient? client =
            await _context.IndividualClients
                .FirstOrDefaultAsync(client => client.ClientId == id, cancellationToken);
        if (client != null)
        {
            return client.IsDeleted;
        }

        return true;
    }

    public async Task<bool> SoftDeleteByClientId(
        int id,
        CancellationToken cancellationToken
    )
    {
        IndividualClient? client =
            await _context.IndividualClients
                .FirstOrDefaultAsync(client => client.ClientId == id, cancellationToken);
        if (client != null)
        {
            client.IsDeleted = true;
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        return false;
    }
}