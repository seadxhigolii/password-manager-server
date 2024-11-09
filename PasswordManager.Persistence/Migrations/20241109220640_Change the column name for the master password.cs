using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PasswordManager.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Changethecolumnnameforthemasterpassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordSalt",
                table: "Users",
                newName: "MasterPasswordHint");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Users",
                newName: "MasterPassword");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MasterPasswordHint",
                table: "Users",
                newName: "PasswordSalt");

            migrationBuilder.RenameColumn(
                name: "MasterPassword",
                table: "Users",
                newName: "PasswordHash");
        }
    }
}
