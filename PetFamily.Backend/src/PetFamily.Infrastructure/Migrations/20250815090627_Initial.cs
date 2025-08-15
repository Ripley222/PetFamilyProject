using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "species",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_species", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "volunteers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    years_of_experience = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    email_address = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    first_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    last_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    middle_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    phone_number = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    requisites = table.Column<string>(type: "jsonb", nullable: true),
                    socials = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_volunteers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "breeds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    species_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_breeds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_breeds_species_species_id",
                        column: x => x.species_id,
                        principalTable: "species",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "pets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    species_id = table.Column<Guid>(type: "uuid", nullable: false),
                    breed_id = table.Column<Guid>(type: "uuid", nullable: false),
                    color = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    is_neutered = table.Column<bool>(type: "boolean", nullable: false),
                    date_of_birth = table.Column<DateOnly>(type: "date", nullable: false),
                    is_vaccinated = table.Column<bool>(type: "boolean", nullable: false),
                    created = table.Column<DateOnly>(type: "date", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    volunteer_id = table.Column<Guid>(type: "uuid", nullable: true),
                    city = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    house = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    street = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    height = table.Column<double>(type: "double precision", maxLength: 300, nullable: false),
                    weight = table.Column<double>(type: "double precision", maxLength: 150, nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    health_information = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    help_status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    phone_number = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    position = table.Column<int>(type: "integer", nullable: false),
                    files = table.Column<string>(type: "jsonb", nullable: true),
                    requisites = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_pets_volunteers_volunteer_id",
                        column: x => x.volunteer_id,
                        principalTable: "volunteers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_breeds_species_id",
                table: "breeds",
                column: "species_id");

            migrationBuilder.CreateIndex(
                name: "IX_pets_volunteer_id",
                table: "pets",
                column: "volunteer_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "breeds");

            migrationBuilder.DropTable(
                name: "pets");

            migrationBuilder.DropTable(
                name: "species");

            migrationBuilder.DropTable(
                name: "volunteers");
        }
    }
}
