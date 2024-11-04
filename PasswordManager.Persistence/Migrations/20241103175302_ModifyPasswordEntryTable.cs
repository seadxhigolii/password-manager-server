using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PasswordManager.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ModifyPasswordEntryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "PasswordEntries",
                newName: "EncryptedPassword");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "PasswordEntries",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "EncryptedNotes",
                table: "PasswordEntries",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsFavorite",
                table: "PasswordEntries",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastAccessedOn",
                table: "PasswordEntries",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "PasswordEntries",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "PasswordEntries",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EncryptedNotes",
                table: "PasswordEntries");

            migrationBuilder.DropColumn(
                name: "IsFavorite",
                table: "PasswordEntries");

            migrationBuilder.DropColumn(
                name: "LastAccessedOn",
                table: "PasswordEntries");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "PasswordEntries");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "PasswordEntries");

            migrationBuilder.RenameColumn(
                name: "EncryptedPassword",
                table: "PasswordEntries",
                newName: "Password");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "PasswordEntries",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);
        }
    }
}
