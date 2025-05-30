using Mapster;
using MapsterMapper;
using Moq;
using SplitCost.Application.Common;
using SplitCost.Application.DTOs;
using SplitCost.Application.UseCases;
using SplitCost.Domain.Entities;
using SplitCost.Domain.Enums;
using SplitCost.Domain.Factories;
using SplitCost.Domain.Interfaces;
using SplitCost.Infrastructure.Entities;

namespace SplitCost.UnitTests.Application.UseCases;

public class CreateExpenseUseCaseTests
{
    private readonly Mock<IExpenseRepository> _mockExpenseRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IResidenceRepository> _mockResidenceRepository;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly CreateExpenseUseCase _useCase;
   
    public CreateExpenseUseCaseTests()
    {
        _mockExpenseRepository = new Mock<IExpenseRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockResidenceRepository = new Mock<IResidenceRepository>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockMapper = new Mock<IMapper>();
        _useCase = new CreateExpenseUseCase(
            _mockExpenseRepository.Object,
            _mockUnitOfWork.Object,
            _mockResidenceRepository.Object,
            _mockUserRepository.Object,
            _mockMapper.Object
        );
    }

    [Fact]
    public async Task CreateExpense_ValidDto_ReturnsSuccessResultWithExpenseDto()
    {
        // Arrange
        var expenseDto = new CreateExpenseDto
        {
            Id = Guid.NewGuid(),
            Amount = 10.00m,
            Category = ExpenseCategory.Food,
            Date = DateTime.UtcNow,
            Description = "Lunch",
            IsSharedAmongMembers = true,
            PaidByUserId = Guid.NewGuid(),
            RegisterByUserId = Guid.NewGuid(),
            ResidenceId = Guid.NewGuid(),
            Type = ExpenseType.Fixed
        };

        var expenseEntity = ExpenseFactory.Create(
               Guid.NewGuid(),
               expenseDto.Type,
               expenseDto.Category,
               expenseDto.Amount,
               expenseDto.Date,
               expenseDto.Description,
               expenseDto.IsSharedAmongMembers,
               expenseDto.ResidenceId,
               expenseDto.RegisterByUserId,
               expenseDto.PaidByUserId
           );

        _mockMapper.Setup(m => m.Map<Expense>(expenseDto)).Returns(expenseEntity);
        _mockResidenceRepository.Setup(repo => repo.ExistsAsync(expenseDto.ResidenceId)).ReturnsAsync(true);
        _mockUserRepository.Setup(repo => repo.ExistsAsync(expenseDto.PaidByUserId)).ReturnsAsync(true);
        _mockUserRepository.Setup(repo => repo.ExistsAsync(expenseDto.RegisterByUserId)).ReturnsAsync(true);
        _mockExpenseRepository.Setup(repo => repo.AddAsync(It.IsAny<Expense>())).Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(uow => uow.SaveChangesAsync()).ReturnsAsync(1);

        // Act
        var result = await _useCase.CreateExpense(expenseDto);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data.Amount);
        Assert.Equal(expenseDto.Amount, result.Data.Amount);
        Assert.Equal(expenseDto.Category, result.Data.Category);
        Assert.Equal(expenseDto.Date, result.Data.Date);
        Assert.Equal(expenseDto.Description, result.Data.Description);
        Assert.Equal(expenseDto.IsSharedAmongMembers, result.Data.IsSharedAmongMembers);
        Assert.Equal(expenseDto.PaidByUserId, result.Data.PaidByUserId);
        Assert.Equal(expenseDto.RegisterByUserId, result.Data.RegisterByUserId);
        Assert.Equal(expenseDto.ResidenceId, result.Data.ResidenceId);
        Assert.Equal(expenseDto.Type, result.Data.Type);
        Assert.NotEqual(Guid.Empty, result.Data.Id);
        _mockExpenseRepository.Verify(repo => repo.AddAsync(It.IsAny<Expense>()), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateExpense_NonExistingResidence_ReturnsFailureResult()
    {
        // Arrange
        var expenseDto = new CreateExpenseDto
        {
            ResidenceId = Guid.NewGuid(),
        };

        var expense = ExpenseFactory.Create(
            Guid.NewGuid(),
            ExpenseType.Fixed,
            ExpenseCategory.Food,
            10.00m,
            DateTime.UtcNow,
            "Test Expense",
            false,
            expenseDto.ResidenceId,
            Guid.NewGuid(),
            Guid.NewGuid()
        );

        _mockMapper.Setup(m => m.Map<Expense>(expenseDto)).Returns(expense);
        _mockResidenceRepository.Setup(repo => repo.ExistsAsync(expenseDto.ResidenceId)).ReturnsAsync(false);

        // Act
        var result = await _useCase.CreateExpense(expenseDto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Residence not found.", result.ErrorMessage);
        Assert.Equal(ErrorType.NotFound, result.ErrorType);
        _mockExpenseRepository.Verify(repo => repo.AddAsync(It.IsAny<Expense>()), Times.Never);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task CreateExpense_NonExistingPayingUser_ReturnsFailureResult()
    {
        // Arrange
        var expenseDto = new CreateExpenseDto
        {
            ResidenceId = Guid.NewGuid(),
            PaidByUserId = Guid.NewGuid()
        };

        var expense = ExpenseFactory.Create(
            Guid.NewGuid(),
            ExpenseType.Fixed,
            ExpenseCategory.Food,
            10.00m,
            DateTime.UtcNow,
            "Test Expense",
            false,
            expenseDto.ResidenceId,
            Guid.NewGuid(),
            expenseDto.PaidByUserId
        );

        _mockMapper.Setup(m => m.Map<Expense>(expenseDto)).Returns(expense);
        _mockResidenceRepository.Setup(repo => repo.ExistsAsync(expenseDto.ResidenceId)).ReturnsAsync(true);
        _mockUserRepository.Setup(repo => repo.ExistsAsync(expenseDto.PaidByUserId)).ReturnsAsync(false);

        // Act
        var result = await _useCase.CreateExpense(expenseDto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Paying user not found.", result.ErrorMessage);
        Assert.Equal(ErrorType.NotFound, result.ErrorType);
        _mockExpenseRepository.Verify(repo => repo.AddAsync(It.IsAny<Expense>()), Times.Never);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task CreateExpense_NonExistingRegisteredUser_ReturnsFailureResult()
    {
        // Arrange
        var expenseDto = new CreateExpenseDto
        {
            ResidenceId = Guid.NewGuid(),
            PaidByUserId = Guid.NewGuid(),
            RegisterByUserId = Guid.NewGuid()
        };

        var expense = ExpenseFactory.Create(
            Guid.NewGuid(),
            ExpenseType.Fixed,
            ExpenseCategory.Food,
            10.00m,
            DateTime.UtcNow,
            "Test Expense",
            false,
            expenseDto.ResidenceId,
            expenseDto.RegisterByUserId,
            expenseDto.PaidByUserId
        );

        _mockMapper.Setup(m => m.Map<Expense>(expenseDto)).Returns(expense);
        _mockResidenceRepository.Setup(repo => repo.ExistsAsync(expenseDto.ResidenceId)).ReturnsAsync(true);
        _mockUserRepository.Setup(repo => repo.ExistsAsync(expenseDto.PaidByUserId)).ReturnsAsync(true);
        _mockUserRepository.Setup(repo => repo.ExistsAsync(expenseDto.RegisterByUserId)).ReturnsAsync(false);

        // Act
        var result = await _useCase.CreateExpense(expenseDto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Registered user not found.", result.ErrorMessage);
        Assert.Equal(ErrorType.NotFound, result.ErrorType);
        _mockExpenseRepository.Verify(repo => repo.AddAsync(It.IsAny<Expense>()), Times.Never);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Never);
    }
}
