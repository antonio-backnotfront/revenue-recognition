namespace RevenueRecognition.Application.Services.Client;

using Exceptions;
using Infrastructure.Repositories.Discount;
using Infrastructure.Repositories.UnitOfWork;
using Application.DTOs.Client;
using Infrastructure.Repositories.Client;
using Models.Client;

public class ClientService : IClientService
{
    private readonly IClientRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDiscountRepository _discountRepository;

    public ClientService(
        IClientRepository repository,
        IUnitOfWork unitOfWork,
        IDiscountRepository discountRepository
    )
    {
        _discountRepository = discountRepository;
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
            CompanyClient? companyInformation =
                await _repository.GetCompanyClientByClientIdAsync(client.Id, cancellationToken);
            IndividualClient? individualInformation =
                await _repository.GetIndividualClientByClientIdAsync(client.Id, cancellationToken);
            if (individualInformation != null && individualInformation.IsDeleted) continue;
            GetClientResponse dto = MapToDto(client, companyInformation, individualInformation);

            dtos.Add(dto);
        }

        return dtos;
    }

    public async Task<GetClientResponse> GetClientByIdAsync(
        int id,
        CancellationToken cancellationToken
    )
    {
        Client? client = await _repository.GetClientByIdAsync(id, cancellationToken);
        if (client == null)
            throw new NotFoundException($"Client with id {id} not found.");

        CompanyClient? companyInformation =
            await _repository.GetCompanyClientByClientIdAsync(client.Id, cancellationToken);
        IndividualClient? individualInformation =
            await _repository.GetIndividualClientByClientIdAsync(client.Id, cancellationToken);
        if (individualInformation != null && individualInformation.IsDeleted)
            throw new NotFoundException($"Client with id {id} not found.");

        GetClientResponse dto = MapToDto(client, companyInformation, individualInformation);

        return dto;
    }

    public async Task<bool> IsClientLoyalById(int id, CancellationToken cancellationToken)
    {
        Client? client = await _repository.GetClientByIdAsync(id, cancellationToken);
        if (client == null)
            throw new NotFoundException($"Client with id {id} not found.");
        return client.IsLoyal;
    }

    public async Task<bool> SetIsClientLoyalById(int clientId, bool isLoyal, CancellationToken cancellationToken)
    {
        Client? client = await _repository.GetClientByIdAsync(clientId, cancellationToken);
        if (client == null)
            throw new NotFoundException($"Client with id {clientId} not found.");
        return await _repository.SetIsLoyalAsync(client, isLoyal, cancellationToken);
    }


    public async Task<GetClientResponse> CreateClientAsync(
        CreateClientRequest request,
        CancellationToken cancellationToken)
    {
        var companyInfo = request.CompanyInformation;
        var individualInfo = request.IndividualInformation;

        if (companyInfo != null && individualInfo != null)
            throw new BadRequestException("Client can't be both Individual and Company.");

        if (companyInfo == null && individualInfo == null)
            throw new BadRequestException("Client type must be provided.");

        await ValidateUniquenessOfEmail(request.Email, cancellationToken);
        await ValidateUniquenessOfPhone(request.PhoneNumber, cancellationToken);

        if (individualInfo != null)
            return await CreateIndividualClientAsync(request, cancellationToken);

        return await CreateCompanyClientAsync(request, cancellationToken);
    }

    private async Task<GetClientResponse> CreateIndividualClientAsync(
        CreateClientRequest request,
        CancellationToken cancellationToken)
    {
        var individual = request.IndividualInformation!;
        await ValidateUniquenessOfPesel(individual.PESEL, cancellationToken);

        await _unitOfWork.StartTransactionAsync(cancellationToken);
        try
        {
            var baseClient = await InsertBaseClientAsync(request, cancellationToken);
            var individualClient = await InsertIndividualClientAsync(request, baseClient.Id, cancellationToken);
            baseClient.IndividualClient = individualClient;

            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return await GetClientByIdAsync(baseClient.Id, cancellationToken);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    private async Task<GetClientResponse> CreateCompanyClientAsync(
        CreateClientRequest request,
        CancellationToken cancellationToken)
    {
        var company = request.CompanyInformation!;
        await ValidateUniquenessOfKrs(company.KRS, cancellationToken);

        await _unitOfWork.StartTransactionAsync(cancellationToken);
        try
        {
            var baseClient = await InsertBaseClientAsync(request, cancellationToken);
            var companyClient = await InsertCompanyClientAsync(request, baseClient.Id, cancellationToken);
            baseClient.CompanyClient = companyClient;

            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return await GetClientByIdAsync(baseClient.Id, cancellationToken);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
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

        if (dto.Email != null) await ValidateUniquenessOfEmail(dto.Email, cancellationToken);
        if (dto.PhoneNumber != null && dto.PhoneNumber != client.PhoneNumber)
            await ValidateUniquenessOfPhone(dto.PhoneNumber, cancellationToken);

        await _unitOfWork.StartTransactionAsync(cancellationToken);
        try
        {
            await _repository.UpdateClientAsync(
                id,
                new Client()
                {
                    Address = dto.Address == null ? client.Address : dto.Address,
                    Email = dto.Email == null ? client.Email : dto.Email,
                    PhoneNumber = dto.PhoneNumber == null ? client.PhoneNumber : dto.PhoneNumber,
                },
                cancellationToken);

            IndividualClient? individualClient = client.IndividualClient;
            CompanyClient? companyClient = client.CompanyClient;

            // ---- if individual ----
            if (individualClient != null)
            {
                if (individualClient.IsDeleted)
                    throw new NotFoundException($"Client with id {id} not found.");

                if (dto.PESEL != individualClient.PESEL)
                    throw new ForbidException($"PESEL can't be changed.");


                await _repository.UpdateIndividualClientAsync(
                    id,
                    new IndividualClient()
                    {
                        FirstName = dto.FirstName == null ? individualClient.FirstName : dto.FirstName,
                        LastName = dto.LastName == null ? individualClient.LastName : dto.LastName,
                        PESEL = individualClient.PESEL,
                    },
                    cancellationToken);
            }
            // ---- if company ----
            else if (companyClient != null)
            {
                if (dto.KRS != null && dto.KRS != companyClient.KRS)
                    throw new ForbidException($"KRS can't be changed.");


                await _repository.UpdateCompanyClientAsync(
                    id,
                    new CompanyClient()
                    {
                        Name = dto.Name == null ? companyClient.Name : dto.Name,
                        KRS = companyClient.KRS,
                    },
                    cancellationToken);
            }

            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return await GetClientByIdAsync(client.Id, cancellationToken);
        }
        catch
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


    private async Task ValidateUniquenessOfEmail(string email, CancellationToken cancellationToken)
    {
        if (await _repository.ExistsByEmailAsync(email, cancellationToken))
            throw new AlreadyExistsException($"Email '{email}' already exists.");
    }


    private async Task ValidateUniquenessOfPhone(string phone, CancellationToken cancellationToken)
    {
        if (await _repository.ExistsByPhoneNumberAsync(phone, cancellationToken))
            throw new AlreadyExistsException($"Phone number '{phone}' already exists.");
    }


    private async Task ValidateUniquenessOfPesel(string pesel, CancellationToken cancellationToken)
    {
        if (await _repository.ExistsIndividualByPeselAsync(pesel, cancellationToken))
            throw new AlreadyExistsException($"PESEL '{pesel}' already exists.");
    }


    private async Task ValidateUniquenessOfKrs(string krs, CancellationToken cancellationToken)
    {
        if (await _repository.ExistsCompanyByKrsAsync(krs, cancellationToken))
            throw new AlreadyExistsException($"KRS '{krs}' already exists.");
    }


    private GetClientResponse MapToDto(Client client, CompanyClient? company, IndividualClient? individual)
    {
        return new GetClientResponse()
        {
            Id = client.Id,
            Address = client.Address,
            Email = client.Email,
            PhoneNumber = client.PhoneNumber,
            IsLoyal = client.IsLoyal,
            CompanyInformation = company == null
                ? null
                : new GetCompanyClientDto()
                {
                    Id = company.Id,
                    Name = company.Name,
                    KRS = company.KRS,
                },
            IndividualInformation = individual == null
                ? null
                : new GetIndividualClientDto()
                {
                    Id = individual.Id,
                    FirstName = individual.FirstName,
                    LastName = individual.LastName,
                    PESEL = individual.PESEL,
                }
        };
    }
}