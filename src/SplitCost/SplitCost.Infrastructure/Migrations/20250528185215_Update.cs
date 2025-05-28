using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SplitCost.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseShares_Expenses_ResidenceExpenseId",
                table: "ExpenseShares");

            migrationBuilder.DropForeignKey(
                name: "FK_ResidenceMembers_Residences_ResidenceId",
                table: "ResidenceMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_ResidenceMembers_Users_UserId",
                table: "ResidenceMembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ResidenceMembers",
                table: "ResidenceMembers");

            migrationBuilder.RenameTable(
                name: "ResidenceMembers",
                newName: "Members");

            migrationBuilder.RenameColumn(
                name: "ResidenceExpenseId",
                table: "ExpenseShares",
                newName: "ExpenseId");

            migrationBuilder.RenameIndex(
                name: "IX_ExpenseShares_ResidenceExpenseId",
                table: "ExpenseShares",
                newName: "IX_ExpenseShares_ExpenseId");

            migrationBuilder.RenameIndex(
                name: "IX_ResidenceMembers_UserId",
                table: "Members",
                newName: "IX_Members_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ResidenceMembers_ResidenceId",
                table: "Members",
                newName: "IX_Members_ResidenceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Members",
                table: "Members",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseShares_Expenses_ExpenseId",
                table: "ExpenseShares",
                column: "ExpenseId",
                principalTable: "Expenses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Residences_ResidenceId",
                table: "Members",
                column: "ResidenceId",
                principalTable: "Residences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Users_UserId",
                table: "Members",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseShares_Expenses_ExpenseId",
                table: "ExpenseShares");

            migrationBuilder.DropForeignKey(
                name: "FK_Members_Residences_ResidenceId",
                table: "Members");

            migrationBuilder.DropForeignKey(
                name: "FK_Members_Users_UserId",
                table: "Members");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Members",
                table: "Members");

            migrationBuilder.RenameTable(
                name: "Members",
                newName: "ResidenceMembers");

            migrationBuilder.RenameColumn(
                name: "ExpenseId",
                table: "ExpenseShares",
                newName: "ResidenceExpenseId");

            migrationBuilder.RenameIndex(
                name: "IX_ExpenseShares_ExpenseId",
                table: "ExpenseShares",
                newName: "IX_ExpenseShares_ResidenceExpenseId");

            migrationBuilder.RenameIndex(
                name: "IX_Members_UserId",
                table: "ResidenceMembers",
                newName: "IX_ResidenceMembers_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Members_ResidenceId",
                table: "ResidenceMembers",
                newName: "IX_ResidenceMembers_ResidenceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResidenceMembers",
                table: "ResidenceMembers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseShares_Expenses_ResidenceExpenseId",
                table: "ExpenseShares",
                column: "ResidenceExpenseId",
                principalTable: "Expenses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ResidenceMembers_Residences_ResidenceId",
                table: "ResidenceMembers",
                column: "ResidenceId",
                principalTable: "Residences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ResidenceMembers_Users_UserId",
                table: "ResidenceMembers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
