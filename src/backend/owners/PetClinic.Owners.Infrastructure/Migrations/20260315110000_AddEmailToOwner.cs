using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetClinic.Owners.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailToOwner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "owners",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_owners_email",
                table: "owners",
                column: "email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_owners_email",
                table: "owners");

            migrationBuilder.DropColumn(
                name: "email",
                table: "owners");
        }
    }
}
