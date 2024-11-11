using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PasswordManager.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangesonUsertable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TwoFactorEnabled",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "EncryptedAESKey",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EncryptedPrivateKey",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PublicKey",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Salt",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EncryptedAESKey",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EncryptedPrivateKey",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PublicKey",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Salt",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Users",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "TwoFactorEnabled",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }
    }
}
