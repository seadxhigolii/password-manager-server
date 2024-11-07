using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PasswordManager.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AdditionalFieldsForVaultTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Vaults",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CardholderName",
                table: "Vaults",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Vaults",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Vaults",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EncryptedCardNumber",
                table: "Vaults",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EncryptedSecurityCode",
                table: "Vaults",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ExpirationDate",
                table: "Vaults",
                type: "character varying(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Vaults",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ItemType",
                table: "Vaults",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Modified",
                table: "Vaults",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PasswordHistory",
                table: "Vaults",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Vaults",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Modified",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Logs",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Modified",
                table: "Logs",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "CustomField",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VaultId = table.Column<Guid>(type: "uuid", nullable: false),
                    FieldName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    EncryptedFieldValue = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ChangedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false),
                    Modified = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomField", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomField_Vaults_VaultId",
                        column: x => x.VaultId,
                        principalTable: "Vaults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomField_VaultId",
                table: "CustomField",
                column: "VaultId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomField");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Vaults");

            migrationBuilder.DropColumn(
                name: "CardholderName",
                table: "Vaults");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Vaults");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Vaults");

            migrationBuilder.DropColumn(
                name: "EncryptedCardNumber",
                table: "Vaults");

            migrationBuilder.DropColumn(
                name: "EncryptedSecurityCode",
                table: "Vaults");

            migrationBuilder.DropColumn(
                name: "ExpirationDate",
                table: "Vaults");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Vaults");

            migrationBuilder.DropColumn(
                name: "ItemType",
                table: "Vaults");

            migrationBuilder.DropColumn(
                name: "Modified",
                table: "Vaults");

            migrationBuilder.DropColumn(
                name: "PasswordHistory",
                table: "Vaults");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Vaults");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Modified",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "Modified",
                table: "Logs");
        }
    }
}
