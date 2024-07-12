using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace basicApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MicrosoftAccountAccessToken",
                table: "AspNetUsers",
                newName: "MicrosoftAccessToken");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MicrosoftAccessToken",
                table: "AspNetUsers",
                newName: "MicrosoftAccountAccessToken");
        }
    }
}
