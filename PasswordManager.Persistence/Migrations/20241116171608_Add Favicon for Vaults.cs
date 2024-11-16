using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PasswordManager.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFaviconforVaults : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "FavIcon",
                table: "Vaults",
                type: "bytea",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FavIcon",
                table: "Vaults");
        }
    }
}
