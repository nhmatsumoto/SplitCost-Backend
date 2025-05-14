using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SplitCost.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class entitiesupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseShares_Expenses_ExpenseId",
                table: "ExpenseShares");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseShares_Users_UserId",
                table: "ExpenseShares");

            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ResidenceMembers",
                table: "ResidenceMembers");

            migrationBuilder.RenameColumn(
                name: "CreatedByUser",
                table: "Residences",
                newName: "CreatedByUserId");

            migrationBuilder.RenameColumn(
                name: "ExpenseId",
                table: "ExpenseShares",
                newName: "ResidenceExpenseId");

            migrationBuilder.RenameIndex(
                name: "IX_ExpenseShares_ExpenseId",
                table: "ExpenseShares",
                newName: "IX_ExpenseShares_ResidenceExpenseId");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "ResidenceMembers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ResidenceMembers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "ResidenceMembers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ExpenseShares",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "ExpenseShares",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResidenceMembers",
                table: "ResidenceMembers",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ResidenceExpenses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsSharedAmongMembers = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResidenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegisteredByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaidByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResidenceExpenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResidenceExpenses_Residences_ResidenceId",
                        column: x => x.ResidenceId,
                        principalTable: "Residences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResidenceExpenses_Users_PaidByUserId",
                        column: x => x.PaidByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResidenceExpenses_Users_RegisteredByUserId",
                        column: x => x.RegisteredByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Residences_CreatedByUserId",
                table: "Residences",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ResidenceMembers_UserId",
                table: "ResidenceMembers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ResidenceExpenses_PaidByUserId",
                table: "ResidenceExpenses",
                column: "PaidByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ResidenceExpenses_RegisteredByUserId",
                table: "ResidenceExpenses",
                column: "RegisteredByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ResidenceExpenses_ResidenceId",
                table: "ResidenceExpenses",
                column: "ResidenceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseShares_ResidenceExpenses_ResidenceExpenseId",
                table: "ExpenseShares",
                column: "ResidenceExpenseId",
                principalTable: "ResidenceExpenses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseShares_Users_UserId",
                table: "ExpenseShares",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Residences_Users_CreatedByUserId",
                table: "Residences",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseShares_ResidenceExpenses_ResidenceExpenseId",
                table: "ExpenseShares");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseShares_Users_UserId",
                table: "ExpenseShares");

            migrationBuilder.DropForeignKey(
                name: "FK_Residences_Users_CreatedByUserId",
                table: "Residences");

            migrationBuilder.DropTable(
                name: "ResidenceExpenses");

            migrationBuilder.DropIndex(
                name: "IX_Residences_CreatedByUserId",
                table: "Residences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ResidenceMembers",
                table: "ResidenceMembers");

            migrationBuilder.DropIndex(
                name: "IX_ResidenceMembers_UserId",
                table: "ResidenceMembers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ResidenceMembers");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ResidenceMembers");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ResidenceMembers");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ExpenseShares");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ExpenseShares");

            migrationBuilder.RenameColumn(
                name: "CreatedByUserId",
                table: "Residences",
                newName: "CreatedByUser");

            migrationBuilder.RenameColumn(
                name: "ResidenceExpenseId",
                table: "ExpenseShares",
                newName: "ExpenseId");

            migrationBuilder.RenameIndex(
                name: "IX_ExpenseShares_ResidenceExpenseId",
                table: "ExpenseShares",
                newName: "IX_ExpenseShares_ExpenseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResidenceMembers",
                table: "ResidenceMembers",
                columns: new[] { "UserId", "ResidenceId" });

            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaidByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResidenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsShared = table.Column<bool>(type: "bit", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Expenses_Residences_ResidenceId",
                        column: x => x.ResidenceId,
                        principalTable: "Residences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Expenses_Users_PaidByUserId",
                        column: x => x.PaidByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_PaidByUserId",
                table: "Expenses",
                column: "PaidByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_ResidenceId",
                table: "Expenses",
                column: "ResidenceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseShares_Expenses_ExpenseId",
                table: "ExpenseShares",
                column: "ExpenseId",
                principalTable: "Expenses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseShares_Users_UserId",
                table: "ExpenseShares",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
