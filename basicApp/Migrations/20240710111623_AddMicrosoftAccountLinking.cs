using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace basicApp.Migrations
{
    /// <inheritdoc />
    public partial class AddMicrosoftAccountLinking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MicrosoftAccountAccessToken",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MicrosoftAccountEmail",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MicrosoftAccountId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MicrosoftAccountAccessToken",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MicrosoftAccountEmail",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MicrosoftAccountId",
                table: "AspNetUsers");
        }
    }
}
