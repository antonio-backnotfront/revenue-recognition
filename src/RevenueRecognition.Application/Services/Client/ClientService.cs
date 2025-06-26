using Microsoft.IdentityModel.Tokens;
using RevenueRecognition.Application.Exceptions;
using RevenueRecognition.Infrastructure.Repositories.UnitOfWork;

namespace RevenueRecognition.Application.Services.Client;

using RevenueRecognition.Application.DTOs.Client;
using RevenueRecognition.Infrastructure.Repositories.Client;
using Models.Client;

public class ClientService : IClientService
{
    private readonly IClientRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ClientService(
        IClientRepository repository,
        IUnitOfWork unitOfWork
    )
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
    }

    public async Task<List<GetClientResponse>> GetClientsAsync(
        CancellationToken cancellationToken
    )
    {
        List<Client> clients = await _repository.GetClientsAsync(cancellationToken);
        List<GetClientResponse> dtos = new List<GetClientResponse>();
        foreach (Client client in clients)
        {
            GetClientResponse dto = new GetClientResponse()
            {
                Id = client.Id,
                Address = client.Address,
                Email = client.Email,
                PhoneNumber = client.PhoneNumber
            };
            CompanyClient? companyInformation =
                await _repository.GetCompanyClientByClientIdAsync(client.Id, cancellationToken);
            if (companyInformation != null)
            {
                dto.CompanyInformation = new GetCompanyClientDto()
                {
                    Id = companyInformation.Id,
                    Name = companyInformation.Name,
                    KRS = companyInformation.KRS,
                };
            }

            IndividualClient? individualInformation =
                await _repository.GetIndividualClientByClientIdAsync(client.Id, cancellationToken);
            if (individualInformation != null)
            {
                if (individualInformation.IsDeleted) continue;
                dto.IndividualInformation = new GetIndividualClientDto()
                {
                    Id = individualInformation.Id,
                    FirstName = individualInformation.FirstName,
                    LastName = individualInformation.LastName,
                    PESEL = individualInformation.PESEL,
                };
            }

            dtos.Add(dto);
        }

        return dtos;
    }

    public async Task<GetClientResponse?> GetClientByIdAsync(
        int id,
        CancellationToken cancellationToken
    )
    {
        Client? client = await _repository.GetClientByIdAsync(id, cancellationToken);
        if (client == null)
            throw new NotFoundException($"Client with id {id} not found");

        GetClientResponse dto = new GetClientResponse()
        {
            Id = client.Id,
            Address = client.Address,
            Email = client.Email,
            PhoneNumber = client.PhoneNumber
        };
        CompanyClient? companyInformation =
            await _repository.GetCompanyClientByClientIdAsync(client.Id, cancellationToken);
        if (companyInformation != null)
        {
            dto.CompanyInformation = new GetCompanyClientDto()
            {
                Id = companyInformation.Id,
                Name = companyInformation.Name,
                KRS = companyInformation.KRS,
            };
        }

        IndividualClient? individualInformation =
            await _repository.GetIndividualClientByClientIdAsync(client.Id, cancellationToken);
        if (individualInformation != null)
        {
            if (await _repository.IsDeletedByClientId(id, cancellationToken))
                throw new NotFoundException($"Client with id {id} not found");

            dto.IndividualInformation = new GetIndividualClientDto()
            {
                Id = individualInformation.Id,
                FirstName = individualInformation.FirstName,
                LastName = individualInformation.LastName,
                PESEL = individualInformation.PESEL,
            };
        }

        return dto;
    }

    public async Task<GetClientResponse> CreateClientAsync(
        CreateClientRequest request,
        CancellationToken cancellationToken
    )
    {
        CreateCompanyClientDto? companyInformation = request.CompanyInformation;
        CreateIndividualClientDto? individualInformation = request.IndividualInformation;

        if (companyInformation != null && individualInformation != null)
            throw new BadRequestException("Can't create Client of type Individual and Company at the same time.");

        if (companyInformation == null && individualInformation == null)
            throw new BadRequestException("Client type must be provided.");

        if (await _repository.ExistsByEmailAsync(request.Email, cancellationToken))
            throw new AlreadyExistsException($"Email '{request.Email}' already exists.");

        if (await _repository.ExistsByPhoneNumberAsync(request.PhoneNumber, cancellationToken))
            throw new AlreadyExistsException($"Phone number '{request.PhoneNumber}' already exists.");

        int createdId = 0;

        // ---- for individual ----
        if (individualInformation != null)
        {
            string pesel = individualInformation.PESEL;
            if (await _repository.ExistsIndividualByPeselAsync(pesel, cancellationToken))
                throw new AlreadyExistsException($"PESEL '{pesel}' already exists.");

            await _unitOfWork.StartTransactionAsync(cancellationToken);
            try
            {
                Client? createdClient = await InsertBaseClientAsync(request, cancellationToken);
                createdId = createdClient.Id;
                IndividualClient? createdIndividual = await InsertIndividualClientAsync(request, createdId, cancellationToken);
                createdClient.IndividualClient = createdIndividual;
                await _unitOfWork.CommitTransactionAsync(cancellationToken);
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }

        // ---- for company ----
        else if (companyInformation != null)
        {
            string krs = companyInformation.KRS;
            if (await _repository.ExistsCompanyByKrsAsync(krs, cancellationToken))
                throw new AlreadyExistsException($"KRS '{krs}' already exists.");

            await _unitOfWork.StartTransactionAsync(cancellationToken);
            try
            {
                Client? createdClient = await InsertBaseClientAsync(request, cancellationToken);
                createdId = createdClient.Id;
                CompanyClient? createdCompany = await InsertCompanyClientAsync(request, createdId, cancellationToken);
                createdClient.CompanyClient = createdCompany;
                await _unitOfWork.CommitTransactionAsync(cancellationToken);
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }
        
        return await GetClientByIdAsync(createdId, cancellationToken);
    }

    public async Task<bool> RemoveClientAsync(
        int id,
        CancellationToken cancellationToken
    )
    {
        Client? client = await _repository.GetClientByIdAsync(id, cancellationToken);
        if (client == null)
            throw new NotFoundException($"Client with id {id} not found.");

        if (client.CompanyClient != null)
            throw new ForbidException("Can't delete company.");

        if (await _repository.IsDeletedByClientId(id, cancellationToken))
            throw new NotFoundException($"Client with id {id} not found.");

        await _repository.SoftDeleteByClientId(id, cancellationToken);

        return true;
    }

    public async Task<GetClientResponse> UpdateClientAsync(
        int id,
        UpdateClientDto dto,
        CancellationToken cancellationToken
    )
    {
        Client? client = await _repository.GetClientByIdAsync(id, cancellationToken);
        if (client == null)
            throw new NotFoundException($"Client with id {id} not found.");

        await _unitOfWork.StartTransactionAsync(cancellationToken);
        try
        {
            await _repository.UpdateClientAsync(
                id,
                new Client()
                {
                    Address = dto.Address == null ? client.Address : dto.Address,
                    Email = dto.Email == null ? client.Email : dto.Email,
                    PhoneNumber = dto.PhoneNumber == null ? client.PhoneNumber : dto.PhoneNumber
                },
                cancellationToken);

            IndividualClient? individualClient = client.IndividualClient;
            CompanyClient? companyClient = client.CompanyClient;

            // ---- if individual ----
            if (individualClient != null)
            {
                if (await _repository.IsDeletedByClientId(id, cancellationToken))
                    throw new NotFoundException($"Client with id {id} not found.");

                if (!(dto.PESEL == null))
                    throw new ForbidException($"PESEL can't be changed.");


                await _repository.UpdateIndividualClientAsync(
                    id,
                    new IndividualClient()
                    {
                        PESEL = dto.PESEL == null ? individualClient.PESEL : dto.PESEL,
                        FirstName = dto.FirstName == null ? individualClient.FirstName : dto.FirstName,
                        LastName = dto.LastName == null ? individualClient.LastName : dto.LastName
                    },
                    cancellationToken);
            }
            // ---- if company ----
            else if (companyClient != null)
            {
                if (!(dto.KRS == null))
                    throw new ForbidException($"KRS can't be changed.");


                await _repository.UpdateCompanyClientAsync(
                    id,
                    new CompanyClient()
                    {
                        Name = dto.Name == null ? companyClient.Name : dto.Name,
                        KRS = dto.KRS == null ? companyClient.KRS : dto.KRS,
                    },
                    cancellationToken);
            }

            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return await GetClientByIdAsync(client.Id, cancellationToken);
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    private async Task<Client> InsertBaseClientAsync(CreateClientRequest request, CancellationToken cancellationToken)
    {
        return await _repository.InsertClientAsync(new Client
        {
            Address = request.Address,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            IsLoyal = false
        }, cancellationToken);
    }

    private async Task<IndividualClient> InsertIndividualClientAsync(
        CreateClientRequest request,
        int clientId,
        CancellationToken cancellationToken
    )
    {
        return await _repository.InsertIndividualClientAsync(new IndividualClient()
        {
            FirstName = request.IndividualInformation.FirstName,
            LastName = request.IndividualInformation.LastName,
            ClientId = clientId,
            PESEL = request.IndividualInformation.PESEL,
            IsDeleted = false
        }, cancellationToken);
    }

    private async Task<CompanyClient> InsertCompanyClientAsync(
        CreateClientRequest request,
        int clientId,
        CancellationToken cancellationToken
    )
    {
        return await _repository.InsertCompanyClientAsync(new CompanyClient()
        {
            Name = request.CompanyInformation.Name,
            KRS = request.CompanyInformation.KRS,
            ClientId = clientId
        }, cancellationToken);
    }
}