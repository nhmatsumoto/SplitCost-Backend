using Mapster;
using SplitCost.Application.UseCases.Dtos;
using SplitCost.Domain.Entities;
using SplitCost.Domain.Factories;

namespace SplitCost.Infrastructure.Mapster;
public static class MapsterConfig
{
    public static void ConfigureMappings()
    {
        // Input -> Domain
        TypeAdapterConfig<CreateExpenseInput, Expense>.NewConfig()
            .MapWith(src => ExpenseFactory.Create(
                src.Type,
                src.Category,
                src.Amount,
                src.Date,
                src.Description,
                src.IsSharedAmongMembers,
                src.ResidenceId,
                src.RegisteredByUserId,
                src.PaidByUserId
            ));

        // Domain -> Output
        TypeAdapterConfig<Expense, CreateExpenseOutput>.NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Type, src => src.Type)
            .Map(dest => dest.Category, src => src.Category)
            .Map(dest => dest.Amount, src => src.Amount)
            .Map(dest => dest.Date, src => src.Date)
            .Map(dest => dest.Description, src => src.Description ?? "")
            .Map(dest => dest.ResidenceId, src => src.ResidenceId)
            .Map(dest => dest.RegisteredByUserId, src => src.RegisteredByUserId)
            .Map(dest => dest.PaidByUserId, src => src.PaidByUserId);

        // Domain -> Entity
        TypeAdapterConfig<Expense, Expense>.NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Amount, src => src.Amount)
            .Map(dest => dest.Category, src => src.Category)
            .Map(dest => dest.Description, src => src.Description ?? "")
            .Map(dest => dest.PaidByUserId, src => src.PaidByUserId)
            .Map(dest => dest.RegisteredByUserId, src => src.RegisteredByUserId)
            .Map(dest => dest.ResidenceId, src => src.ResidenceId)
            .Map(dest => dest.Date, src => src.Date);

        // Entity -> Domain
        TypeAdapterConfig<Expense, Expense>.NewConfig()
            .MapWith(src => ExpenseFactory.Create()
            .SetType(src.Type)
            .SetCategory(src.Category)
            .SetAmount(src.Amount)
            .SetDate(src.Date)
            .SetDescription(src.Description ?? string.Empty)
            .SetResidenceId(src.ResidenceId)
            .SetWhoRegistered(src.RegisteredByUserId)
            .SetWhoPaid(src.PaidByUserId)
            .SetId(src.Id));

        // Domain -> Entity
        TypeAdapterConfig<User, User>.NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.Id, src => src.Id);

        // Entity -> Domain
        TypeAdapterConfig<User, User>.NewConfig()
            .MapWith(src => UserFactory.Create(
                src.Id,
                src.Username,
                src.Name,
                src.Email,
                src.AvatarUrl));

        TypeAdapterConfig<Residence, GetResidenceByIdOutput>.NewConfig()
           .Map(dest => dest.Id, src => src.Id)
           .Map(dest => dest.CreatedByUserId, src => src.CreatedByUserId)
           .Map(dest => dest.Name, src => src.Name)
           .Map(dest => dest.Street, src => src.Street)
           .Map(dest => dest.Number, src => src.Number)
           .Map(dest => dest.Apartment, src => src.Apartment)
           .Map(dest => dest.City, src => src.City)
           .Map(dest => dest.Prefecture, src => src.Prefecture)
           .Map(dest => dest.Country, src => src.Country)
           .Map(dest => dest.PostalCode, src => src.PostalCode)
           .Map(dest => dest.Members, src => src.Members)
           .Map(dest => dest.Expenses, src => src.Expenses);

        // Domain -> Entity
        TypeAdapterConfig<Residence, Residence>.NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.CreatedByUserId, src => src.CreatedByUserId)
            .Map(dest => dest.Street, src => src.Street)
            .Map(dest => dest.Number, src => src.Number)
            .Map(dest => dest.Apartment, src => src.Apartment)
            .Map(dest => dest.City, src => src.City)
            .Map(dest => dest.Prefecture, src => src.Prefecture)
            .Map(dest => dest.Country, src => src.Country)
            .Map(dest => dest.PostalCode, src => src.PostalCode)
            .Map(dest => dest.CreatedAt, src => src.CreatedAt)
            .Map(dest => dest.UpdatedAt, src => src.UpdatedAt);

        // Entity -> Domain
        TypeAdapterConfig<Residence, Residence>.NewConfig().MapWith(src => ResidenceFactory
            .Create()
            .SetId(src.Id)
            .SetName(src.Name)
            .SetCreatedByUser(src.CreatedByUserId)
            .SetStreet(src.Street)
            .SetNumber(src.Number)
            .SetApartment(src.Apartment)
            .SetCity(src.City)
            .SetPrefecture(src.Prefecture)
            .SetCountry(src.Country)
            .SetPostalCode(src.PostalCode)
        );


        // Domain -> Entity
        TypeAdapterConfig<Member, Member>.NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest.ResidenceId, src => src.ResidenceId)
            .Map(dest => dest.JoinedAt, src => src.JoinedAt);

        // Entity -> Domain
        //src.Id, src.ResidenceId,
        TypeAdapterConfig<Member, Member>.NewConfig()
            .MapWith(src => Domain.Factories.MemberFactory.Create(src.JoinedAt));

        TypeAdapterConfig<Income, Income>.NewConfig()
               .Map(dest => dest.Id, src => src.Id)
               .Map(dest => dest.Amount, src => src.Amount)
               .Map(dest => dest.Category, src => src.Category)
               .Map(dest => dest.Description, src => src.Description) // Removido ?? "", pois SetDescription já valida
               .Map(dest => dest.UserId, src => src.UserId)
               .Map(dest => dest.ResidenceId, src => src.ResidenceId)
               .Map(dest => dest.Date, src => src.Date)
               .Ignore(dest => dest.User)
               .Ignore(dest => dest.Residence);

        TypeAdapterConfig<CreateIncomeInput, Income>.NewConfig()
            .Map(dest => dest.Amount, src => src.Amount)
            .Map(dest => dest.Category, src => src.Category)
             .Map(dest => dest.Date, src => src.Date)
            .Map(dest => dest.Description, src => src.Description) // Removido ?? "", pois SetDescription já valida
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest.ResidenceId, src => src.ResidenceId);


        //// Entity -> Domain
        //config.NewConfig<IncomeEntity, Income>()
        //    .ConstructUsing(src => IncomeFactory.Create(
        //        amount: src.Amount,
        //        category: src.Category,
        //        date: src.Date,
        //        description: src.Description ?? string.Empty, // Garante que Description não seja null
        //        residenceId: src.ResidenceId)
        //    .Map(dest => dest.Id, src => src.Id)
        //    // Ignora propriedades de navegação para evitar mapeamento automático
        //    .Ignore(dest => dest.Residence)
        //    .Ignore(dest => dest.RegisteredBy);




        // Mapeamento de CreateIncomeInput para Income
        TypeAdapterConfig<CreateIncomeInput, Income>.NewConfig()
            .Map(dest => dest.Id, src => Guid.NewGuid()) // Gera um novo Guid para o Id
            .Map(dest => dest.CreatedAt, src => DateTime.UtcNow) // Define CreatedAt como a data atual
            .Map(dest => dest.Amount, src => src.Amount) // Mapeia Amount diretamente
            .Map(dest => dest.Category, src => src.Category) // Mapeia Category diretamente
            .Map(dest => dest.Date, src => src.Date) // Mapeia Date diretamente
            .Map(dest => dest.Description, src => src.Description) // Mapeia Description diretamente
            .Map(dest => dest.ResidenceId, src => src.ResidenceId) // Mapeia ResidenceId diretamente
            .Map(dest => dest.UserId, src => src.UserId) // Mapeia RegisteredByUserId diretamente
            .Ignore(dest => dest.Residence) // Ignora a propriedade de navegação Residence
            .Ignore(dest => dest.UserId) // Ignora a propriedade de navegação RegisteredBy
            .Ignore(dest => dest.UpdatedAt); // Ignora UpdatedAt (herdado de BaseEntity)

       

    }
}