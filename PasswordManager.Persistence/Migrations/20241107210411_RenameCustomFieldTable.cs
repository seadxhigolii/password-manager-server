using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PasswordManager.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RenameCustomFieldTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomField_Vaults_VaultId",
                table: "CustomField");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomField",
                table: "CustomField");

            migrationBuilder.RenameTable(
                name: "CustomField",
                newName: "CustomFields");

            migrationBuilder.RenameIndex(
                name: "IX_CustomField_VaultId",
                table: "CustomFields",
                newName: "IX_CustomFields_VaultId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomFields",
                table: "CustomFields",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomFields_Vaults_VaultId",
                table: "CustomFields",
                column: "VaultId",
                principalTable: "Vaults",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomFields_Vaults_VaultId",
                table: "CustomFields");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomFields",
                table: "CustomFields");

            migrationBuilder.RenameTable(
                name: "CustomFields",
                newName: "CustomField");

            migrationBuilder.RenameIndex(
                name: "IX_CustomFields_VaultId",
                table: "CustomField",
                newName: "IX_CustomField_VaultId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomField",
                table: "CustomField",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomField_Vaults_VaultId",
                table: "CustomField",
                column: "VaultId",
                principalTable: "Vaults",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
