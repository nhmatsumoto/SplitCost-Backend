using MapsterMapper;
using Moq;
using SplitCost.Application.Common;
using SplitCost.Application.DTOs;
using SplitCost.Application.UseCases;
using SplitCost.Domain.Entities;
using SplitCost.Domain.Enums;
using SplitCost.Domain.Factories;
using SplitCost.Domain.Interfaces;

namespace SplitCost.UnitTests.Application.UseCases;

public class ReadExpenseUseCaseTests
{
    private readonly Mock<IExpenseRepository> _mockExpenseRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly ReadExpenseUseCase _useCase;

    public ReadExpenseUseCaseTests()
    {
        _mockExpenseRepository = new Mock<IExpenseRepository>();
        _mockMapper = new Mock<IMapper>();
        _useCase = new ReadExpenseUseCase(_mockExpenseRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsSuccessResultWithExpenseDto()
    {
        // Arrange
        var expenseId = Guid.NewGuid();

        var expense = ExpenseFactory.Create(
            ExpenseType.Fixed,
            ExpenseCategory.Food,
            10.00m,
            DateTime.UtcNow,
            "Lunch",
            true,
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid()
         );

        var expenseDto = new ExpenseDto { Id = expenseId, Amount = 10.00m }; // Customize as needed

        _mockExpenseRepository.Setup(repo => repo.GetByIdAsync(expenseId)).ReturnsAsync(expense);
        _mockMapper.Setup(mapper => mapper.Map<ExpenseDto>(expense)).Returns(expenseDto);

        // Act
        var result = await _useCase.GetByIdAsync(expenseId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(expenseId, result.Data.Id);
        Assert.Equal(10.00m, result.Data.Amount);
        _mockExpenseRepository.Verify(repo => repo.GetByIdAsync(expenseId), Times.Once);
        _mockMapper.Verify(mapper => mapper.Map<ExpenseDto>(expense), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ReturnsFailureResult()
    {
        // Arrange
        var expenseId = Guid.NewGuid();
        _mockExpenseRepository.Setup(repo => repo.GetByIdAsync(expenseId)).ReturnsAsync((Expense)null);

        // Act
        var result = await _useCase.GetByIdAsync(expenseId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Expense not found.", result.ErrorMessage);
        Assert.Equal(ErrorType.NotFound, result.ErrorType);
        Assert.Null(result.Data);
        _mockExpenseRepository.Verify(repo => repo.GetByIdAsync(expenseId), Times.Once);
        _mockMapper.Verify(mapper => mapper.Map<ExpenseDto>(It.IsAny<Expense>()), Times.Never);
    }

    [Fact]
    public async Task GetExpensesByResidenceIdAsync_ExistingResidenceIdWithExpenses_ReturnsSuccessResultWithListOfExpenseDto()
    {
        // Arrange
        var residenceId = Guid.NewGuid();

        var expense1 = ExpenseFactory.Create(
            ExpenseType.Fixed,
            ExpenseCategory.Food,
            10.00m,
            DateTime.UtcNow,
            "Lunch",
            true,
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid()
         );

        var expense2 = ExpenseFactory.Create(
            ExpenseType.Variable,
            ExpenseCategory.Water,
            20.00m,
            DateTime.UtcNow,
            "Payment",
            true,
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid()
         );

        var expenseEntities = new List<Expense>
        {
            expense1,
            expense2
        };
        var expenseDtos = expenseEntities.Select(e => new ExpenseDto { Id = e.Id, Amount = e.Amount }).ToList(); // Customize as needed

        _mockExpenseRepository.Setup(repo => repo.GetByResidenceIdAsync(residenceId)).ReturnsAsync(expenseEntities);
        _mockMapper.Setup(mapper => mapper.Map<IEnumerable<ExpenseDto>>(expenseEntities)).Returns(expenseDtos);

        // Act
        var result = await _useCase.GetExpensesByResidenceIdAsync(residenceId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(2, result.Data.Count());
        Assert.Equal(expenseDtos.First().Id, result.Data.First().Id);
        Assert.Equal(expenseDtos.Last().Amount, result.Data.Last().Amount);
        _mockExpenseRepository.Verify(repo => repo.GetByResidenceIdAsync(residenceId), Times.Once);
        _mockMapper.Verify(mapper => mapper.Map<IEnumerable<ExpenseDto>>(expenseEntities), Times.Once);
    }

    [Fact]
    public async Task GetExpensesByResidenceIdAsync_ExistingResidenceIdWithoutExpenses_ReturnsFailureResult()
    {
        // Arrange
        var residenceId = Guid.NewGuid();
        _mockExpenseRepository.Setup(repo => repo.GetByResidenceIdAsync(residenceId)).ReturnsAsync(Enumerable.Empty<Expense>());

        // Act
        var result = await _useCase.GetExpensesByResidenceIdAsync(residenceId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal($"No expenses found for residence ID '{residenceId}'.", result.ErrorMessage);
        Assert.Equal(ErrorType.NotFound, result.ErrorType);
        Assert.Null(result.Data);
        _mockExpenseRepository.Verify(repo => repo.GetByResidenceIdAsync(residenceId), Times.Once);
        _mockMapper.Verify(mapper => mapper.Map<IEnumerable<ExpenseDto>>(It.IsAny<IEnumerable<ExpenseDto>>()), Times.Once);
    }

    [Fact]
    public async Task GetExpensesByResidenceIdAsync_NonExistingResidenceId_ReturnsFailureResult()
    {
        // Arrange
        var residenceId = Guid.NewGuid();
        _mockExpenseRepository.Setup(repo => repo.GetByResidenceIdAsync(residenceId)).ReturnsAsync((IEnumerable<Expense>)null);

        // Act
        var result = await _useCase.GetExpensesByResidenceIdAsync(residenceId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal($"No expenses found for residence ID '{residenceId}'.", result.ErrorMessage);
        Assert.Equal(ErrorType.NotFound, result.ErrorType);
        Assert.Null(result.Data);
        _mockExpenseRepository.Verify(repo => repo.GetByResidenceIdAsync(residenceId), Times.Once);
        _mockMapper.Verify(mapper => mapper.Map<IEnumerable<ExpenseDto>>(It.IsAny<IEnumerable<Expense>>()), Times.Never);
    }
}
