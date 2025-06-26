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
                PhoneNumber = client.PhoneNumber,
                IsLoyal = client.IsLoyal
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
        if (client == null) return null;
        GetClientResponse dto = new GetClientResponse()
        {
            Id = client.Id,
            Address = client.Address,
            Email = client.Email,
            PhoneNumber = client.PhoneNumber,
            IsLoyal = client.IsLoyal
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

    public async Task<CreateClientRequest?> CreateClientAsync(
        CreateClientRequest request,
        CancellationToken cancellationToken
    )
    {
        CreateCompanyClientDto? companyInformation = request.CompanyInformation;
        CreateIndividualClientDto? individualInformation = request.IndividualInformation;
        // validate if only one client type is being created
        if (companyInformation != null && individualInformation != null)
            throw new BadRequestException("Can't create Client of type Individual and Company at the same time.");

        // validate if at least one type is selected
        if (companyInformation == null && individualInformation == null)
            throw new BadRequestException("Client type must be provided.");

        // validate email uniqueness
        if (await _repository.ExistsByEmailAsync(request.Email, cancellationToken))
            throw new AlreadyExistsException($"Email '{request.Email}' already exists.");

        // validate phone uniqueness
        if (await _repository.ExistsByPhoneNumberAsync(request.PhoneNumber, cancellationToken))
            throw new AlreadyExistsException($"Phone number '{request.PhoneNumber}' already exists.");

        CreateClientRequest? createdRequest = new CreateClientRequest()
        {
            Address = request.Address,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            CompanyInformation = companyInformation,
            IndividualInformation = individualInformation,
        };
        
        // ---- for individual
        if (individualInformation != null)
        {
            // validate pesel uniqueness
            string pesel = individualInformation.PESEL;
            if (await _repository.ExistsIndividualByPeselAsync(pesel, cancellationToken))
                throw new AlreadyExistsException($"PESEL '{pesel}' already exists.");

            await _unitOfWork.StartTransactionAsync(cancellationToken);
            try
            {
                Client? createdClient = await _repository.InsertClientAsync(new Client()
                {
                    Address = request.Address,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    IsLoyal = false,
                }, cancellationToken);

                IndividualClient? createdIndividual = await _repository.InsertIndividualClientAsync(
                    new IndividualClient()
                    {
                        ClientId = createdClient.Id,
                        FirstName = individualInformation.FirstName,
                        LastName = individualInformation.LastName,
                        PESEL = pesel,
                        IsDeleted = false,
                        Client = createdClient
                    }, cancellationToken);
                createdClient.IndividualClient = createdIndividual;
                await _unitOfWork.CommitTransactionAsync(cancellationToken);
                createdRequest.Id = createdClient.Id;
                createdRequest.IndividualInformation.Id = createdIndividual.Id;
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }
        // ---- for company
        else
        {
            // validate krs uniqueness
            string krs = companyInformation.KRS;
            if (await _repository.ExistsCompanyByKrsAsync(krs, cancellationToken))
                throw new AlreadyExistsException($"KRS '{krs}' already exists.");

            await _unitOfWork.StartTransactionAsync(cancellationToken);
            try
            {
                Client? createdClient = await _repository.InsertClientAsync(new Client()
                {
                    Address = request.Address,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    IsLoyal = false,
                }, cancellationToken);

                CompanyClient? createdCompany = await _repository.InsertCompanyClientAsync(
                    new CompanyClient()
                    {
                        ClientId = createdClient.Id,
                        Name = companyInformation.Name,
                        KRS = krs,
                        Client = createdClient
                    }, cancellationToken);
                createdClient.CompanyClient = createdCompany;
                await _unitOfWork.CommitTransactionAsync(cancellationToken);
                createdRequest.Id = createdClient.Id;
                createdRequest.CompanyInformation.Id = createdCompany.Id;
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }    
        }
        return createdRequest;
    }

    public async Task<bool> RemoveClientAsync(
        int id,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }

    public async Task<GetClientResponse> UpdateClientAsync(
        int id,
        UpdateClientDto dto,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }
}