using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NexusChat.Migrations
{
    /// <inheritdoc />
    public partial class MudancaDeCampos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RefreshTokenExpiryTime",
                table: "AspNetUsers",
                newName: "RefreshTokenTempoExpiracao");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RefreshTokenTempoExpiracao",
                table: "AspNetUsers",
                newName: "RefreshTokenExpiryTime");
        }
    }
}
