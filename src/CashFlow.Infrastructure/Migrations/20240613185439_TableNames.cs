using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CashFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_User_UserId",
                table: "Expenses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Expenses",
                table: "Expenses");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "T_USERS");

            migrationBuilder.RenameTable(
                name: "Expenses",
                newName: "T_EXPENSES");

            migrationBuilder.RenameIndex(
                name: "IX_Expenses_UserId",
                table: "T_EXPENSES",
                newName: "IX_T_EXPENSES_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_T_USERS",
                table: "T_USERS",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_T_EXPENSES",
                table: "T_EXPENSES",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_T_EXPENSES_T_USERS_UserId",
                table: "T_EXPENSES",
                column: "UserId",
                principalTable: "T_USERS",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_EXPENSES_T_USERS_UserId",
                table: "T_EXPENSES");

            migrationBuilder.DropPrimaryKey(
                name: "PK_T_USERS",
                table: "T_USERS");

            migrationBuilder.DropPrimaryKey(
                name: "PK_T_EXPENSES",
                table: "T_EXPENSES");

            migrationBuilder.RenameTable(
                name: "T_USERS",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "T_EXPENSES",
                newName: "Expenses");

            migrationBuilder.RenameIndex(
                name: "IX_T_EXPENSES_UserId",
                table: "Expenses",
                newName: "IX_Expenses_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Expenses",
                table: "Expenses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_User_UserId",
                table: "Expenses",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
