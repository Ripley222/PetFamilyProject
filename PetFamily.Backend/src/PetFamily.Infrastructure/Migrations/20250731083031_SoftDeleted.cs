using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SoftDeleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_pets_volunteers_volunteer_id",
                table: "pets");

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "volunteers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "pets",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_pets_volunteers_volunteer_id",
                table: "pets",
                column: "volunteer_id",
                principalTable: "volunteers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_pets_volunteers_volunteer_id",
                table: "pets");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "volunteers");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "pets");

            migrationBuilder.AddForeignKey(
                name: "FK_pets_volunteers_volunteer_id",
                table: "pets",
                column: "volunteer_id",
                principalTable: "volunteers",
                principalColumn: "Id");
        }
    }
}
