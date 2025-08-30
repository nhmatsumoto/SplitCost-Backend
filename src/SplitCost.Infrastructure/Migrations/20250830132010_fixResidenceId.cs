using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SplitCost.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixResidenceId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSettings_Residences_ResidenceId",
                table: "UserSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSettings_Users_UserId1",
                table: "UserSettings");

            migrationBuilder.DropIndex(
                name: "IX_UserSettings_UserId1",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "UserSettings");

            migrationBuilder.AlterColumn<Guid>(
                name: "ResidenceId",
                table: "UserSettings",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSettings_Residences_ResidenceId",
                table: "UserSettings",
                column: "ResidenceId",
                principalTable: "Residences",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSettings_Residences_ResidenceId",
                table: "UserSettings");

            migrationBuilder.AlterColumn<Guid>(
                name: "ResidenceId",
                table: "UserSettings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "UserSettings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSettings_UserId1",
                table: "UserSettings",
                column: "UserId1",
                unique: true,
                filter: "[UserId1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSettings_Residences_ResidenceId",
                table: "UserSettings",
                column: "ResidenceId",
                principalTable: "Residences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSettings_Users_UserId1",
                table: "UserSettings",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
