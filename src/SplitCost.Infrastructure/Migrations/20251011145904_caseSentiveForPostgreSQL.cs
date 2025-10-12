using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SplitCost.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class caseSentiveForPostgreSQL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Residences_ResidenceId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Incomes_Residences_ResidenceId",
                table: "Incomes");

            migrationBuilder.DropForeignKey(
                name: "FK_Members_Residences_ResidenceId",
                table: "Members");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSettings_Residences_ResidenceId",
                table: "UserSettings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSettings",
                table: "UserSettings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Residences",
                table: "Residences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Members",
                table: "Members");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Incomes",
                table: "Incomes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Expenses",
                table: "Expenses");

            migrationBuilder.RenameTable(
                name: "UserSettings",
                newName: "usersettings");

            migrationBuilder.RenameTable(
                name: "Residences",
                newName: "residences");

            migrationBuilder.RenameTable(
                name: "Members",
                newName: "members");

            migrationBuilder.RenameTable(
                name: "Incomes",
                newName: "incomes");

            migrationBuilder.RenameTable(
                name: "Expenses",
                newName: "expenses");

            migrationBuilder.RenameIndex(
                name: "IX_UserSettings_ResidenceId",
                table: "usersettings",
                newName: "ix_usersettings_residenceid");

            migrationBuilder.RenameIndex(
                name: "IX_Members_ResidenceId",
                table: "members",
                newName: "ix_members_residenceid");

            migrationBuilder.RenameIndex(
                name: "IX_Incomes_ResidenceId",
                table: "incomes",
                newName: "ix_incomes_residenceid");

            migrationBuilder.RenameIndex(
                name: "IX_Expenses_ResidenceId",
                table: "expenses",
                newName: "ix_expenses_residenceid");

            migrationBuilder.AddPrimaryKey(
                name: "pk_usersettings",
                table: "usersettings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_residences",
                table: "residences",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_members",
                table: "members",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_incomes",
                table: "incomes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_expenses",
                table: "expenses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "fk_expenses_residences_residenceid",
                table: "expenses",
                column: "ResidenceId",
                principalTable: "residences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_incomes_residences_residenceid",
                table: "incomes",
                column: "ResidenceId",
                principalTable: "residences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_members_residences_residenceid",
                table: "members",
                column: "ResidenceId",
                principalTable: "residences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_usersettings_residences_residenceid",
                table: "usersettings",
                column: "ResidenceId",
                principalTable: "residences",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_expenses_residences_residenceid",
                table: "expenses");

            migrationBuilder.DropForeignKey(
                name: "fk_incomes_residences_residenceid",
                table: "incomes");

            migrationBuilder.DropForeignKey(
                name: "fk_members_residences_residenceid",
                table: "members");

            migrationBuilder.DropForeignKey(
                name: "fk_usersettings_residences_residenceid",
                table: "usersettings");

            migrationBuilder.DropPrimaryKey(
                name: "pk_usersettings",
                table: "usersettings");

            migrationBuilder.DropPrimaryKey(
                name: "pk_residences",
                table: "residences");

            migrationBuilder.DropPrimaryKey(
                name: "pk_members",
                table: "members");

            migrationBuilder.DropPrimaryKey(
                name: "pk_incomes",
                table: "incomes");

            migrationBuilder.DropPrimaryKey(
                name: "pk_expenses",
                table: "expenses");

            migrationBuilder.RenameTable(
                name: "usersettings",
                newName: "UserSettings");

            migrationBuilder.RenameTable(
                name: "residences",
                newName: "Residences");

            migrationBuilder.RenameTable(
                name: "members",
                newName: "Members");

            migrationBuilder.RenameTable(
                name: "incomes",
                newName: "Incomes");

            migrationBuilder.RenameTable(
                name: "expenses",
                newName: "Expenses");

            migrationBuilder.RenameIndex(
                name: "ix_usersettings_residenceid",
                table: "UserSettings",
                newName: "IX_UserSettings_ResidenceId");

            migrationBuilder.RenameIndex(
                name: "ix_members_residenceid",
                table: "Members",
                newName: "IX_Members_ResidenceId");

            migrationBuilder.RenameIndex(
                name: "ix_incomes_residenceid",
                table: "Incomes",
                newName: "IX_Incomes_ResidenceId");

            migrationBuilder.RenameIndex(
                name: "ix_expenses_residenceid",
                table: "Expenses",
                newName: "IX_Expenses_ResidenceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSettings",
                table: "UserSettings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Residences",
                table: "Residences",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Members",
                table: "Members",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Incomes",
                table: "Incomes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Expenses",
                table: "Expenses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Residences_ResidenceId",
                table: "Expenses",
                column: "ResidenceId",
                principalTable: "Residences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Incomes_Residences_ResidenceId",
                table: "Incomes",
                column: "ResidenceId",
                principalTable: "Residences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Residences_ResidenceId",
                table: "Members",
                column: "ResidenceId",
                principalTable: "Residences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSettings_Residences_ResidenceId",
                table: "UserSettings",
                column: "ResidenceId",
                principalTable: "Residences",
                principalColumn: "Id");
        }
    }
}
